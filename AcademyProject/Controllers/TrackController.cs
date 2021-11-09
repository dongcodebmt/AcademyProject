using AcademyProject.DTOs;
using AcademyProject.Models;
using AcademyProject.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet]
        public async Task<ActionResult<List<TrackDTO>>> Get()
        {
            var list = await trackService.GetAll();
            var listTrack = list.Select(x => mapper.Map<TrackDTO>(x)).ToList();
            return Ok(listTrack);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrackDTO>> Get(int id)
        {
            var track = await trackService.GetById(id);
            if (track == null)
            {
                return NotFound();
            }
            var trackDTO = mapper.Map<TrackDTO>(track);
            return Ok(trackDTO);
        }

        [HttpGet("{id}/Steps")]
        public async Task<ActionResult<List<StepDTO>>> Steps(int id)
        {
            var list = await stepService.GetList(x => x.TrackId == id);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult<TrackDTO>> Post([FromBody] TrackDTO trackDTO)
        {
            var track = mapper.Map<Track>(trackDTO);
            track = await trackService.Insert(track);
            trackDTO = mapper.Map<TrackDTO>(track);
            return Ok(trackDTO);
        }

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
