using AcademyProject.DTOs;
using AcademyProject.Entities;
using AcademyProject.Services;
using AutoMapper;
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
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IUserRoleService userRoleService;
        private readonly IRoleService roleService;

        public TokenController(IConfiguration config, IMapper mapper, IUserService userService, IUserRoleService userRoleService, IRoleService roleService)
        {
            configuration = config;
            this.mapper = mapper;
            this.userService = userService;
            this.userRoleService = userRoleService;
            this.roleService = roleService;
        }

        [HttpPost]
        public async Task<ActionResult<Token>> Post([FromBody] Login login)
        {
            var user = await GetUser(login.Email, login.Password);

            if (user == null)
            {
                return BadRequest();
            }

            //create claims details based on the user information
            var claims = new List<Claim>();
            claims.Add(new Claim("Id", user.Id.ToString()));

            var list = await userRoleService.GetAll();
            var userRoles = list.Where(x => x.UserId == user.Id).ToList();
            foreach (var item in userRoles)
            {
                var role = await roleService.GetById(item.UserId);
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTConfig:AccessTokenSecret"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: configuration["JWTConfig:Issuer"],
                audience: configuration["JWTConfig:Audience"],
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signingCredentials,
                claims: claims
            );

            Token tokenResult = new Token(new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);

            return Ok(tokenResult);
        }

        private async Task<UserDTO> GetUser(string email, string password)
        {
            var list = await userService.GetAll();
            var user = list.Select(x => mapper.Map<UserDTO>(x)).FirstOrDefault(u => u.Email == email && u.PasswordHash == password);

            return user;
        }
    }
}