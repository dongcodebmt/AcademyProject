using AcademyProject.DTOs;
using AcademyProject.Models;
using AcademyProject.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrators, Lecturers")]
    public class StepController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IGenericService<Step> stepService;
        private readonly IGenericService<Progress> progressService;
        public StepController(IMapper mapper, IGenericService<Step> stepService, IGenericService<Progress> progressService)
        {
            this.mapper = mapper;
            this.stepService = stepService;
            this.progressService = progressService;
        }

        protected int GetCurrentUserId()
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            int id = Convert.ToInt32(userId);
            return id;
        }

        // GET: api/<StepController>
        //[HttpGet]
        //public async Task<ActionResult<StepDTO>> Get()
        //{
        //    var list = await stepService.GetAll();
        //    var stepDTOs = list.Select(x => mapper.Map<StepDTO>(x)).ToList();
        //    return Ok(stepDTOs);
        //}

        // GET: api/<StepController>/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<StepDTO>> Get(int id)
        {
            var step = await stepService.GetById(id);
            if (step == null)
            {
                return NotFound();
            }
            var stepDTO = mapper.Map<StepDTO>(step);
            return Ok(stepDTO);
        }

        // POST: api/<StepController>
        [HttpPost]
        public async Task<ActionResult<StepDTO>> Post([FromBody] StepDTO stepDTO)
        {
            var step = mapper.Map<Step>(stepDTO);
            step = await stepService.Insert(step);
            stepDTO = mapper.Map<StepDTO>(step);
            return Ok(stepDTO);
        }

        // POST: api/<StepController>/{id}/[Action]
        [Authorize]
        [HttpPost("{id}/[action]")]
        public async Task<ActionResult<bool>> Progress(int id)
        {
            var step = await stepService.GetById(id);
            if (step == null)
            {
                return NotFound();
            }
            int userId = GetCurrentUserId();
            var progress = await progressService.Get(x => x.UserId == userId && x.StepId == id);
            if (progress != null)
            {
                return Ok(false);
            }
            Progress p = new Progress();
            p.UserId = userId;
            p.StepId = step.Id;
            await progressService.Insert(p);
            return Ok(true);
        }

        // PUT: api/<StepController>/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<StepDTO>> Put(int id, [FromBody] StepDTO stepDTO)
        {
            var step = await stepService.GetById(id);
            if (step == null)
            {
                return NotFound();
            }
            step.TrackId = stepDTO.TrackId;
            step.Title = stepDTO.Title;
            step.Duration = stepDTO.Duration;
            step.Content = stepDTO.Content;
            step.EmbedLink = stepDTO.EmbedLink;
            step = await stepService.Update(step);

            stepDTO = mapper.Map<StepDTO>(step);

            return Ok(stepDTO);
        }

        // DELETE: api/<StepController>/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var step = await stepService.GetById(id);
            if (step == null)
            {
                return NotFound();
            }
            step.IsDeleted = true;
            await stepService.Update(step);
            return Ok();
        }
    }
}
