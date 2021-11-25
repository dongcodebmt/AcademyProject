using AcademyProject.DTOs;
using AcademyProject.Entities;
using AcademyProject.Models;
using AcademyProject.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly SharedComponent component;
        private readonly IGenericService<User> userService;
        private readonly IGenericService<UserRole> userRoleService;
        private readonly IGenericService<Role> roleService;

        public UserController(
            IMapper mapper,
            SharedComponent component, 
            IGenericService<User> userService, 
            IGenericService<Picture> pictureService, 
            IGenericService<UserRole> userRoleService, 
            IGenericService<Role> roleService
        )
        {
            this.mapper = mapper;
            this.component = component;
            this.userService = userService;
            this.userRoleService = userRoleService;
            this.roleService = roleService;
        }

        protected int GetCurrentUserId()
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            int id = Convert.ToInt32(userId);
            return id;
        }

        // GET: api/<UserController>
        [HttpGet]
        [Authorize(Roles = "Administrators")]
        public async Task<ActionResult<List<User2DTO>>> Get()
        {
            var list = await userService.GetAll();
            var users = list.Select(x => mapper.Map<User2DTO>(x)).ToList();
            foreach (var item in users)
            {
                item.Picture = await component.GetImageAsync(item.PictureId);
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

        // GET: api/<UserController>/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrators")]
        public async Task<ActionResult<User2DTO>> Get(int id)
        {
            //Get user
            var user = await userService.Get(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            User2DTO user2DTO = mapper.Map<User2DTO>(user);
            user2DTO.Picture = await component.GetImageAsync(user.PictureId);
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

        // GET: api/<UserController>/[Action]
        [Authorize]
        [HttpGet("[action]")]
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
            User2DTO user2DTO = mapper.Map<User2DTO>(user);
            user2DTO.Picture = await component.GetImageAsync(user.PictureId);
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

        // GET: api/<UserController>/[Action]/{id}
        [HttpGet("[action]/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<User2DTO>> Public(int id)
        {
            //Get user
            var user = await userService.Get(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            User2DTO user2DTO = mapper.Map<User2DTO>(user);
            user2DTO.Email = null;
            user2DTO.Picture = await component.GetImageAsync(user.PictureId);
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

        // GET: api/<UserController>/[Action]
        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<RoleDTO>>> Roles()
        {
            var roles = await roleService.GetAll();
            var roleDTOs = roles.Select(x => mapper.Map<RoleDTO>(x)).ToList();
            return Ok(roleDTOs);
        }

        // GET: api/<UserController>/{id}/[Action]
        [HttpGet("{id}/[action]")]
        [Authorize(Roles = "Administrators, Moderators")]
        public async Task<ActionResult<List<int>>> Roles(int id)
        {
            var userRoles = await userRoleService.GetList(x => x.UserId == id);
            List<int> list = new List<int>();
            foreach (var item in userRoles)
            {
                list.Add(item.RoleId);
            }
            return Ok(list);
        }

        // GET: api/<UserController>/{id}/[Action]
        [HttpPost("{id}/[action]")]
        [Authorize(Roles = "Administrators")]
        public async Task<IActionResult> Roles(int id, [FromBody] List<int> roles)
        {
            if (roles.Count < 1)
            {
                return BadRequest("Vui lòng chọn ít nhất 1 quyền!");
            }
            var user = await userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await userRoleService.GetList(x => x.UserId == id);
            foreach (var item in userRoles)
            {
                await userRoleService.Delete(item);
            }
            foreach (var item in roles)
            {
                UserRole UR = new UserRole();
                UR.UserId = user.Id;
                UR.RoleId = item;
                await userRoleService.Insert(UR);
            }
            return Ok();
        }
        
        // POST: api/<UserController>/[Action]
        [HttpPost("[action]")]
        [AllowAnonymous]
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
            UserRole userRole = new UserRole();
            userRole.UserId = newUser.Id;
            userRole.RoleId = 4;
            await userRoleService.Insert(userRole);

            User2DTO user2DTO = mapper.Map<User2DTO>(newUser);

            return Ok(user2DTO);
        }

        // POST: api/<UserController>/[Action]
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> Forgot([FromBody] string email)
        {
            var user = await userService.Get(u => u.Email == email);
            if (user == null)
            {
                return Ok(false);
            }
            string newPassword = component.CreatePassword(10);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user = await userService.Update(user);

            //Send mail
            string subject = "Đặt lại mật khẩu - Academy";
            string body = "Mật khẩu mới của bạn là: " + newPassword;
            bool success = component.SendMail(user.Email, subject, body);

            return Ok(success);
        }

        // POST: api/<UserController>/{id}/[Action]
        [HttpPost("{id}/[action]")]
        [Authorize(Roles = "Administrators")]
        public async Task<IActionResult> Password(int id, [FromBody] string newPassword)
        {
            if (newPassword.Length < 8)
            {
                return BadRequest(new { message = "Mật khẩu mới cần lớn hơn 8 ký tự!" });
            }

            var user = await userService.Get(u => u.Id == id);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.UpdatedAt = DateTime.Now;
            user = await userService.Update(user);
            return Ok();
        }

        // POST: api/<UserController>/[Action]
        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> Password([FromBody] Password password)
        {
            int userId = GetCurrentUserId();
            if (password.NewPassword.Length < 8)
            {
                return BadRequest(new { message = "Mật khẩu mới cần lớn hơn 8 ký tự!" });
            }
            var user = await userService.Get(u => u.Id == userId);
            bool verified = false;
            if (user.PasswordHash != null)
            {
                verified = BCrypt.Net.BCrypt.Verify(password.OldPassword, user.PasswordHash);
            } 
            else
            {
                verified = true;
            }
            if (!verified)
            {
                return BadRequest(new { message = "Mật khẩu cũ không đúng!" });
            }
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password.NewPassword);
            user.UpdatedAt = DateTime.Now;
            user = await userService.Update(user);
            return Ok();
        }

        // PUT: api/<UserController>/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrators")]
        public async Task<ActionResult<User2DTO>> Put(int id, [FromBody] User2DTO userDTO)
        {
            var user = await userService.Get(u => u.Id == id);
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.Email = userDTO.Email;
            user.UpdatedAt = DateTime.Now;
            if (userDTO.PictureId != null && userDTO.PictureId != 0)
            {
                user.PictureId = userDTO.PictureId;
            }
            user = await userService.Update(user);
            userDTO = mapper.Map<User2DTO>(user);
            return Ok(userDTO);
        }

        // PUT: api/<UserController>/[Action]
        [Authorize]
        [HttpPut("[action]")]
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
            user.UpdatedAt = DateTime.Now;
            user = await userService.Update(user);
            userDTO = mapper.Map<User2DTO>(user);
            return Ok(userDTO);
        }
    }
}
