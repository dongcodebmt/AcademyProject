using AcademyProject.DTOs;
using AcademyProject.Entities;
using AcademyProject.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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

        [HttpPost]
        public async Task<ActionResult<JWT>> Refresh(JWT jwt)
        {
            int id = ValidateRefreshToken(jwt.Token);
            if (id != 0)
            {
                var accessToken = await GetAccessToken(id);

                JWT tokenResult = new JWT(
                    new JwtSecurityTokenHandler().WriteToken(accessToken),
                    accessToken.ValidTo
                );

                return Ok(tokenResult);
            }
            return Unauthorized();
        }

        [HttpPost]
        public async Task<ActionResult<JWT>> RefreshToken([FromBody] Login login)
        {
            // Verify account
            var user = await GetUser(login.Email);
            if (user == null)
            {
                return BadRequest();
            }
            bool verified = VerifyPassword(login.Password, user.PasswordHash);
            if (verified == false)
            {
                return BadRequest();
            }

            var refreshToken = await GetRefreshToken(user.Id);

            JWT tokenResult = new JWT(
                new JwtSecurityTokenHandler().WriteToken(refreshToken),
                refreshToken.ValidTo
            );

            return Ok(tokenResult);
        }

        [HttpPost]
        public async Task<ActionResult<JWT>> AccessToken([FromBody] Login login)
        {
            // Verify account
            var user = await GetUser(login.Email);
            if (user == null)
            {
                return BadRequest();
            }
            bool verified = VerifyPassword(login.Password, user.PasswordHash);
            if (verified == false)
            {
                return BadRequest();
            }

            var accessToken = await GetAccessToken(user.Id);

            JWT tokenResult = new JWT(
                new JwtSecurityTokenHandler().WriteToken(accessToken), 
                accessToken.ValidTo
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
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: accessTokenCredentials,
                claims: claims
            );
            return accessToken;
        }

        private async Task<JwtSecurityToken> GetRefreshToken(int id)
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

        private async Task<UserDTO> GetUser(string email)
        {
            var list = await userService.GetAll();
            var user = list.Select(x => mapper.Map<UserDTO>(x)).FirstOrDefault(u => u.Email == email);
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
    }
}