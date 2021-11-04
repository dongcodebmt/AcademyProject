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
        private readonly IGenericService<User> userService;
        private readonly IGenericService<Picture> pictureService;
        //private readonly IUserService userService;
        //private readonly IPictureService pictureService;

        public UserController(IMapper mapper, IConfiguration configuration, IGenericService<User> userService, IGenericService<Picture> pictureService)
        {
            this.mapper = mapper;
            this.configuration = configuration;
            this.userService = userService;
            this.pictureService = pictureService;
        }

        protected int GetCurrentUserId()
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            int id = Convert.ToInt32(userId);
            return id;
        }

        [HttpGet]
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

        [HttpPut]
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
            return Ok(user);
        }
    }
}
