using AcademyProject.DTOs;
using AcademyProject.Models;
using AcademyProject.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AcademyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StepController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IGenericService<Step> stepService;
        public StepController(IMapper mapper, IGenericService<Step> stepService)
        {
            this.mapper = mapper;
            this.stepService = stepService;
        }
        // GET: api/<TrackStepController>
        [HttpGet]
        public async Task<ActionResult<StepDTO>> Get()
        {
            var list = await stepService.GetAll();
            var stepDTOs = list.Select(x => mapper.Map<StepDTO>(x)).ToList();
            return Ok(stepDTOs);
        }

        // GET api/<TrackStepController>/5
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

        // POST api/<TrackStepController>
        [HttpPost]
        public async Task<ActionResult<StepDTO>> Post([FromBody] StepDTO stepDTO)
        {
            var step = mapper.Map<Step>(stepDTO);
            step = await stepService.Insert(step);
            stepDTO = mapper.Map<StepDTO>(step);
            return Ok(stepDTO);
        }

        // PUT api/<TrackStepController>/5
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

        // DELETE api/<TrackStepController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await stepService.Delete(id);
            return Ok();
        }
    }
}
