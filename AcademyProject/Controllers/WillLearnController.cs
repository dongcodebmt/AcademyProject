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
    public class WillLearnController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IGenericService<WillLearn> willLearnService;
        public WillLearnController(IMapper mapper, IGenericService<WillLearn> willLearnService)
        {
            this.mapper = mapper;
            this.willLearnService = willLearnService;
        }

        // GET: api/<WillLearnController>
        //[HttpGet]
        //public async Task<ActionResult<List<WillLearnDTO>>> Get()
        //{
        //    var list = await willLearnService.GetAll();
        //    var listWillLearn = list.Select(x => mapper.Map<WillLearnDTO>(x)).ToList();
        //    return Ok(listWillLearn);
        //}

        // GET: api/<WillLearnController>/{id}
        //[HttpGet("{id}")]
        //public async Task<ActionResult<WillLearnDTO>> Get(int id)
        //{
        //    var willLearn = await willLearnService.GetById(id);
        //    if (willLearn == null)
        //    {
        //        return NotFound();
        //    }
        //    var willLearnDTO = mapper.Map<WillLearnDTO>(willLearn);
        //    return Ok(willLearnDTO);
        //}

        // POST: api/<WillLearnController>
        [HttpPost]
        public async Task<ActionResult<WillLearnDTO>> Post([FromBody] WillLearnDTO willLearnDTO)
        {
            var willLearn = mapper.Map<WillLearn>(willLearnDTO);
            willLearn = await willLearnService.Insert(willLearn);
            willLearnDTO = mapper.Map<WillLearnDTO>(willLearn);
            return Ok(willLearnDTO);
        }

        // PUT: api/<WillLearnController>/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<WillLearnDTO>> Put(int id, [FromBody] WillLearnDTO willLearnDTO)
        {
            var willLearn = await willLearnService.GetById(id);
            if (willLearn == null)
            {
                return NotFound();
            }
            willLearn.CourseId = willLearnDTO.CourseId;
            willLearn.Content = willLearnDTO.Content;

            willLearn = await willLearnService.Update(willLearn);

            willLearnDTO = mapper.Map<WillLearnDTO>(willLearn);

            return Ok(willLearnDTO);
        }

        // DELETE: api/<WillLearnController>/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await willLearnService.Delete(id);
            return Ok();
        }
    }
}
