using AcademyProject.DTOs;
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
    [Authorize(Roles = "Administrators, Lecturers")]
    public class TrackController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IGenericService<Track> trackService;
        private readonly IGenericService<Step> stepService;
        public TrackController(IMapper mapper, IGenericService<Track> trackService, IGenericService<Step> stepService)
        {
            this.mapper = mapper;
            this.trackService = trackService;
            this.stepService = stepService;
        }

        // GET: api/<TrackController>
        //[HttpGet]
        //public async Task<ActionResult<List<TrackDTO>>> Get()
        //{
        //    var list = await trackService.GetAll();
        //    var listTrack = list.Select(x => mapper.Map<TrackDTO>(x)).ToList();
        //    return Ok(listTrack);
        //}

        // GET: api/<TrackController>/{id}
        //[HttpGet("{id}")]
        //public async Task<ActionResult<TrackDTO>> Get(int id)
        //{
        //    var track = await trackService.GetById(id);
        //    if (track == null)
        //    {
        //        return NotFound();
        //    }
        //    var trackDTO = mapper.Map<TrackDTO>(track);
        //    return Ok(trackDTO);
        //}

        // GET: api/<TrackController>/{id}/[Action]
        [HttpGet("{id}/[action]")]
        public async Task<ActionResult<List<StepDTO>>> Steps(int id)
        {
            var list = await stepService.GetList(x => x.TrackId == id && x.IsDeleted == false);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // POST: api/<TrackController>
        [HttpPost]
        public async Task<ActionResult<TrackDTO>> Post([FromBody] TrackDTO trackDTO)
        {
            var track = mapper.Map<Track>(trackDTO);
            track = await trackService.Insert(track);
            trackDTO = mapper.Map<TrackDTO>(track);
            return Ok(trackDTO);
        }

        // PUT: api/<TrackController>/{id}
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

            return Ok(trackDTO);
        }

        // DELETE: api/<TrackController>/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var track = await trackService.GetById(id);
            if (track == null)
            {
                return NotFound();
            }
            track.IsDeleted = true;
            await trackService.Update(track);
            return Ok();
        }
    }
}
