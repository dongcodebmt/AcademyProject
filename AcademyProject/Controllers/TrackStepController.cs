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
    public class TrackStepController : ControllerBase
    {
        private readonly IGenericService<TrackStep> trackStepService;
        private readonly IMapper mapper;
        public TrackStepController(IGenericService<TrackStep> trackStepService, IMapper mapper)
        {
            this.trackStepService = trackStepService;
            this.mapper = mapper;
        }
        // GET: api/<TrackStepController>
        [HttpGet]
        public async Task<ActionResult<TrackStepDTO>> Get()
        {
            var list = await trackStepService.GetAll();
            var listTrackStep = list.Select(x => mapper.Map<TrackStepDTO>(x)).ToList();
            return Ok(new { listTrackStep });
        }

        // GET api/<TrackStepController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrackStepDTO>> Get(int id)
        {
            var trackStep = await trackStepService.GetById(id);
            if (trackStep == null)
            {
                return NotFound();
            }
            var trackStepDTO = mapper.Map<TrackStepDTO>(trackStep);
            return Ok(new { trackStepDTO });
        }

        // POST api/<TrackStepController>
        [HttpPost]
        public async Task<ActionResult<TrackStepDTO>> Post([FromBody] TrackStepDTO trackStepDTO)
        {
            var trackStep = mapper.Map<TrackStep>(trackStepDTO);
            trackStep = await trackStepService.Insert(trackStep);
            trackStepDTO = mapper.Map<TrackStepDTO>(trackStep);
            return Ok(new { trackStep });
        }

        // PUT api/<TrackStepController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<TrackStepDTO>> Put(int id, [FromBody] TrackStepDTO trackStepDTO)
        {
            var trackStep = await trackStepService.GetById(id);
            if (trackStep == null)
            {
                return NotFound();
            }
            trackStep.TrackId = trackStepDTO.TrackId;
            trackStep.Title = trackStepDTO.Title;
            trackStep.Duration = trackStepDTO.Duration;
            trackStep = await trackStepService.Update(trackStep);

            trackStepDTO = mapper.Map<TrackStepDTO>(trackStep);

            return Ok(new { trackStepDTO });
        }

        // DELETE api/<TrackStepController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await trackStepService.Delete(id);
            return Ok();
        }
    }
}
