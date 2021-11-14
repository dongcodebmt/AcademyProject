using AcademyProject.DTOs;
using AcademyProject.Entities;
using AcademyProject.Models;
using AcademyProject.Services;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AcademyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IGenericService<User> userService;
        private readonly IGenericService<Picture> pictureService;
        private readonly IGenericService<UserRole> userRoleService;
        private readonly IGenericService<Role> roleService;

        public UserController(IMapper mapper, IConfiguration configuration, IGenericService<User> userService, 
            IGenericService<Picture> pictureService, IGenericService<UserRole> userRoleService, IGenericService<Role> roleService)
        {
            this.mapper = mapper;
            this.configuration = configuration;
            this.userService = userService;
            this.pictureService = pictureService;
            this.userRoleService = userRoleService;
            this.roleService = roleService;
        }

        protected int GetCurrentUserId()
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            int id = Convert.ToInt32(userId);
            return id;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User2DTO>> Get(int id)
        {
            //Get user
            var user = await userService.Get(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            var picture = await pictureService.Get(i => i.Id == user.PictureId);

            //Detect is local images or other host
            string pictureUrl = picture.PicturePath;
            if (pictureUrl != "/" && pictureUrl.Substring(0, 1) == "/")
            {
                pictureUrl = configuration["ServerHostName"] + picture.PicturePath;
            }
            User2DTO user2DTO = mapper.Map<User2DTO>(user);
            user2DTO.Picture = pictureUrl;
            //Get role
            var userRoles = await userRoleService.GetList(x => x.UserId == id);
            List<string> scope = new List<string>();
            foreach (var i in userRoles)
            {
                var role = await roleService.GetById(i.RoleId);
                scope.Add(role.Name);
            }
            user2DTO.Scope = scope;

            return Ok(user2DTO);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult<User2DTO>> Me()
        {
            int userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }

            //Get user
            var user = await userService.Get(u => u.Id == userId);
            if (user == null)
            {
                return BadRequest();
            }
            var picture = await pictureService.Get(i => i.Id == user.PictureId);

            //Detect is local images or other host
            string pictureUrl = picture.PicturePath;
            if (pictureUrl != "/" && pictureUrl.Substring(0, 1) == "/")
            {
                pictureUrl = configuration["ServerHostName"] + picture.PicturePath;
            }
            User2DTO user2DTO = mapper.Map<User2DTO>(user);
            user2DTO.Picture = pictureUrl;
            //Get role
            var userRoles = await userRoleService.GetList(x => x.UserId == userId);
            List<string> scope = new List<string>();
            foreach (var i in userRoles)
            {
                var role = await roleService.GetById(i.RoleId);
                scope.Add(role.Name);
            }
            user2DTO.Scope = scope;

            return Ok(user2DTO);
        }
        
        [HttpPost("[action]")]
        public async Task<ActionResult<User2DTO>> Register([FromBody] UserDTO userDTO)
        {
            var user = await userService.Get(u => u.Email == userDTO.Email);
            if (user != null)
            {
                return BadRequest(new { message = "Tài khoản đã tồn tại!" });
            }
            if (userDTO.Password.Length < 8)
            {
                return BadRequest(new { message = "Mật khẩu cần lớn hơn 8 ký tự!" });
            }
            //Hash password
            userDTO.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
            //Mapper DTO to User and insert to db
            var newUser = mapper.Map<User>(userDTO);
            newUser = await userService.Insert(newUser);

            User2DTO user2DTO = mapper.Map<User2DTO>(newUser);

            return Ok(user2DTO);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> Password([FromBody] Password password)
        {
            int userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }
            if (password.NewPassword.Length < 8)
            {
                return BadRequest(new { message = "Mật khẩu mới cần lớn hơn 8 ký tự!" });
            }

            var user = await userService.Get(u => u.Id == userId);
            bool verified = BCrypt.Net.BCrypt.Verify(password.OldPassword, user.PasswordHash);
            if (!verified)
            {
                return BadRequest(new { message = "Mật khẩu cũ không đúng!" });
            }
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password.NewPassword);
            user = await userService.Update(user);
            return Ok();
        }

        [HttpPut("[action]")]
        [Authorize]
        public async Task<ActionResult<User2DTO>> Infomation([FromBody] User2DTO userDTO)
        {
            int userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }

            var user = await userService.Get(u => u.Id == userId);
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.Email = userDTO.Email;
            user = await userService.Update(user);
            userDTO = mapper.Map<User2DTO>(user);
            return Ok(userDTO);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<User2DTO>>> GetAll()
        {
            var list = await userService.GetAll();
            var users = list.Select(x => mapper.Map<User2DTO>(x)).ToList();
            foreach (var item in users)
            {
                //Get picture
                var picture = await pictureService.Get(i => i.Id == item.PictureId);
                string pictureUrl = picture.PicturePath;
                if (pictureUrl != "/" && pictureUrl.Substring(0, 1) == "/")
                {
                    pictureUrl = configuration["ServerHostName"] + picture.PicturePath;
                }
                item.Picture = pictureUrl;
                //Get role
                var userRoles = await userRoleService.GetList(x => x.UserId == item.Id);
                List<string> scope = new List<string>();
                foreach (var i in userRoles)
                {
                    var role = await roleService.GetById(i.RoleId);
                    scope.Add(role.Name);
                }
                item.Scope = scope;
            }
            return Ok(users);
        }
    }
}
