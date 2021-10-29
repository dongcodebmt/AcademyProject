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

namespace AcademyProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IPictureService pictureService;

        public UserController(IMapper mapper, IConfiguration configuration, IUserService userService, IPictureService pictureService)
        {
            this.mapper = mapper;
            this.configuration = configuration;
            this.userService = userService;
            this.pictureService = pictureService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<User2DTO>> Me()
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            int id = Convert.ToInt32(userId);
            var users = await userService.GetAll();
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return BadRequest();
            }
            var pictures = await pictureService.GetAll();
            var picture = pictures.FirstOrDefault(i => i.Id == user.PictureId);
            //Detect is local images or gg, fb
            string pictureUrl = picture.PicturePath;
            if (pictureUrl.Substring(0, 1) == "/")
            {
                pictureUrl = configuration["ServerHostName"] + picture.PicturePath;
            }
            User2DTO eUser = new User2DTO();
            eUser.Id = user.Id;
            eUser.Email = user.Email;
            eUser.FirstName = user.FirstName;
            eUser.LastName = user.LastName;
            eUser.Picture = pictureUrl;

            return Ok(eUser);
        }

        [HttpPost]
        public async Task<ActionResult<User2DTO>> Register([FromBody] UserDTO userDTO)
        {
            var list = await userService.GetAll();
            var user = list.FirstOrDefault(u => u.Email == userDTO.Email);
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

            User2DTO eUser = new User2DTO();
            eUser.Id = newUser.Id;
            eUser.Email = newUser.Email;
            eUser.FirstName = newUser.FirstName;
            eUser.LastName = newUser.LastName;

            return Ok(eUser);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Password([FromBody] Password password)
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            if (password.NewPassword.Length < 8)
            {
                return BadRequest(new { message = "Mật khẩu mới cần lớn hơn 8 ký tự!" });
            }
            int id = Convert.ToInt32(userId);
            var users = await userService.GetAll();
            var user = users.FirstOrDefault(u => u.Id == id);
            bool verified = BCrypt.Net.BCrypt.Verify(password.OldPassword, user.PasswordHash);
            if (!verified)
            {
                return BadRequest(new { message = "Mật khẩu cũ không đúng!" });
            }
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password.NewPassword);
            user = await userService.Update(user);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<User2DTO>> Infomation([FromBody] User2DTO userDTO)
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            int id = Convert.ToInt32(userId);
            var users = await userService.GetAll();
            var user = users.FirstOrDefault(u => u.Id == id);
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.Email = userDTO.Email;
            user = await userService.Update(user);
            return Ok(user);
        }
    }
}
