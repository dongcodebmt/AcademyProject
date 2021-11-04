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
    public class TrackController : ControllerBase
    {
        private readonly IGenericService<Track> trackService;
        private readonly IMapper mapper;
        public TrackController(IGenericService<Track> trackService, IMapper mapper)
        {
            this.trackService = trackService;
            this.mapper = mapper;
        }
        // GET: api/<TrackController>
        [HttpGet]
        public async Task<ActionResult<TrackDTO>> Get()
        {
            var list = await trackService.GetAll();
            var listTrack = list.Select(x => mapper.Map<TrackDTO>(x)).ToList();
            return Ok(new { listTrack });
        }

        // GET api/<TrackController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrackDTO>> Get(int id)
        {
            var track = await trackService.GetById(id);
            if (track == null)
            {
                return NotFound();
            }
            var trackDTO = mapper.Map<TrackDTO>(track);
            return Ok(new { trackDTO });
        }

        // POST api/<TrackController>
        [HttpPost]
        public async Task<ActionResult<TrackDTO>> Post([FromBody] TrackDTO trackDTO)
        {
            var track = mapper.Map<Track>(trackDTO);
            track = await trackService.Insert(track);
            trackDTO = mapper.Map<TrackDTO>(track);
            return Ok(new { track });
        }

        // PUT api/<TrackController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<TrackDTO>> Put(int id, [FromBody] TrackDTO trackDTO)
        {
            var track = await trackService.GetById(id);
            if (track == null)
            {
                return NotFound();
            }
            track.CourseId = trackDTO.CourseId;
            track.Title = trackDTO.Title;
            track = await trackService.Update(track);

            trackDTO = mapper.Map<TrackDTO>(track);

            return Ok(new { trackDTO });
        }

        // DELETE api/<TrackController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await trackService.Delete(id);
            return Ok();
        }
    }
}
