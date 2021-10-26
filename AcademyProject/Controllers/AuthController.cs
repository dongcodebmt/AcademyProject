using AcademyProject.DTOs;
using AcademyProject.Entities;
using AcademyProject.Models;
using AcademyProject.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcademyProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IUserRoleService userRoleService;
        private readonly IRoleService roleService;

        public AuthController(IConfiguration config, IMapper mapper, IUserService userService, IUserRoleService userRoleService, IRoleService roleService)
        {
            configuration = config;
            this.mapper = mapper;
            this.userService = userService;
            this.userRoleService = userRoleService;
            this.roleService = roleService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDTO>> Me()
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            int id = Convert.ToInt32(userId);
            var list = await userService.GetAll();
            var user = list.Select(x => mapper.Map<UserDTO>(x)).FirstOrDefault(u => u.Id == id);
            user.PasswordHash = null;
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Register([FromBody] UserDTO userDTO)
        {
            var user = await GetUser(userDTO.Email);
            if (user != null)
            {
                return BadRequest(new { message = "Tài khoản đã tồn tại!" });
            }
            if (userDTO.Password.Length < 8)
            {
                return BadRequest(new { message = "Mật khẩu cần lớn hơn 8 ký tự!" });
            }

            userDTO.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
            var newUser = mapper.Map<User>(userDTO);
            newUser = await userService.Insert(newUser);

            var accessToken = await GetAccessToken(newUser.Id);
            var refreshToken = GetRefreshToken(newUser.Id);

            JWT tokenResult = new JWT(
                new JwtSecurityTokenHandler().WriteToken(accessToken),
                accessToken.ValidTo,
                new JwtSecurityTokenHandler().WriteToken(refreshToken),
                refreshToken.ValidTo
            );

            return Ok(tokenResult);
        }

        [HttpPost]
        public async Task<ActionResult<JWT>> Facebook(string token)
        {
            string result = "";
            try
            {
                //Get facebook user info from access token Oauth2
                string url = "https://graph.facebook.com/me?fields=id,first_name,last_name,name,email,picture&access_token=" + token;
                result = await GetAsync(url);
            }
            catch
            {
                return BadRequest();
            }

            //Parse json
            FacebookUser fbUser = JsonConvert.DeserializeObject<FacebookUser>(result);
            dynamic data = JObject.Parse(result);
            fbUser.picture_url = data.picture.data.url;

            if (fbUser.email == null)
            {
                return BadRequest();
            }
            var user = await GetUser(fbUser.email);
            if (user == null)
            {
                //Create new User
                User newUser = new User();
                newUser.FirstName = fbUser.first_name;
                newUser.LastName = fbUser.last_name;
                newUser.Email = fbUser.email;
                newUser.PasswordHash = null;

                user = await userService.Insert(newUser);
            }

            var accessToken = await GetAccessToken(user.Id);
            var refreshToken = GetRefreshToken(user.Id);

            JWT tokenResult = new JWT(
                new JwtSecurityTokenHandler().WriteToken(accessToken),
                accessToken.ValidTo,
                new JwtSecurityTokenHandler().WriteToken(refreshToken),
                refreshToken.ValidTo
            );

            return Ok(tokenResult);
        }

        [HttpPost]
        public async Task<ActionResult<JWT>> Google(string token)
        {
            string result = "";
            try
            {
                //Get google user info from access token Oauth2
                string url = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + token;
                result = await GetAsync(url);
            } catch
            {
                return BadRequest();
            }

            GoogleUser googleUser = JsonConvert.DeserializeObject<GoogleUser>(result);

            if (googleUser.email == null)
            {
                return BadRequest();
            }
            var user = await GetUser(googleUser.email);
            if (user == null)
            {
                //Create new User
                User newUser = new User();
                newUser.FirstName = googleUser.given_name;
                newUser.LastName = googleUser.family_name;
                newUser.Email = googleUser.email;
                newUser.PasswordHash = null;

                user = await userService.Insert(newUser);
            }

            var accessToken = await GetAccessToken(user.Id);
            var refreshToken = GetRefreshToken(user.Id);

            JWT tokenResult = new JWT(
                new JwtSecurityTokenHandler().WriteToken(accessToken),
                accessToken.ValidTo,
                new JwtSecurityTokenHandler().WriteToken(refreshToken),
                refreshToken.ValidTo
            );

            return Ok(tokenResult);
        }

        [HttpPost]
        public async Task<ActionResult<JWT>> Refresh(JWT jwt)
        {
            int id = ValidateRefreshToken(jwt.RefreshToken);
            if (id != 0)
            {
                var accessToken = await GetAccessToken(id);

                JWT tokenResult = new JWT(
                    new JwtSecurityTokenHandler().WriteToken(accessToken),
                    accessToken.ValidTo,
                    null,
                    null
                );

                return Ok(tokenResult);
            }
            return Unauthorized();
        }

        [HttpPost]
        public async Task<ActionResult<JWT>> Token([FromBody] Login login)
        {
            // Verify account
            var user = await GetUser(login.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Tài khoản không tồn tại!" });
            }
            if (user.PasswordHash == null)
            {
                return BadRequest(new { message = "Vui lòng đăng nhập bằng tài khoản mạng xã hội!" });
            }
            bool verified = VerifyPassword(login.Password, user.PasswordHash);
            if (verified == false)
            {
                return BadRequest(new { message = "Mật khẩu không khớp!" });
            }

            var accessToken = await GetAccessToken(user.Id);
            var refreshToken = GetRefreshToken(user.Id);

            JWT tokenResult = new JWT(
                new JwtSecurityTokenHandler().WriteToken(accessToken),
                accessToken.ValidTo,
                new JwtSecurityTokenHandler().WriteToken(refreshToken),
                refreshToken.ValidTo
            );

            return Ok(tokenResult);
        }

        private int ValidateRefreshToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["JWTConfig:RefreshTokenSecret"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWTConfig:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JWTConfig:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "Id").Value);

                return accountId;
            }
            catch
            {
                return 0;
            }
        }

        private async Task<JwtSecurityToken> GetAccessToken(int id)
        {
            //create claims details based on the user information
            var claims = new List<Claim>();
            claims.Add(new Claim("Id", id.ToString()));

            var list = await userRoleService.GetAll();
            var userRoles = list.Where(x => x.UserId == id).ToList();
            foreach (var item in userRoles)
            {
                var role = await roleService.GetById(item.UserId);
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var accessTokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTConfig:AccessTokenSecret"]));
            var accessTokenCredentials = new SigningCredentials(accessTokenKey, SecurityAlgorithms.HmacSha256Signature);

            var accessToken = new JwtSecurityToken(
                issuer: configuration["JWTConfig:Issuer"],
                audience: configuration["JWTConfig:Audience"],
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: accessTokenCredentials,
                claims: claims
            );
            return accessToken;
        }

        private JwtSecurityToken GetRefreshToken(int id)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("Id", id.ToString()));

            var refreshTokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTConfig:RefreshTokenSecret"]));
            var refreshTokenCredentials = new SigningCredentials(refreshTokenKey, SecurityAlgorithms.HmacSha256Signature);

            var refreshToken = new JwtSecurityToken(
                issuer: configuration["JWTConfig:Issuer"],
                audience: configuration["JWTConfig:Audience"],
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: refreshTokenCredentials,
                claims: claims
            );
            return refreshToken;
        }

        private async Task<User> GetUser(string email)
        {
            var list = await userService.GetAll();
            var user = list.FirstOrDefault(u => u.Email == email);
            return user;
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            bool verified = BCrypt.Net.BCrypt.Verify(password, passwordHash);
            if (verified == true)
            {
                return true;
            }
            return false;
        }

        private async Task<string> GetAsync(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}