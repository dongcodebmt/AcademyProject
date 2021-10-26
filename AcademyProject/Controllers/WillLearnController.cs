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
    public class WillLearnController : ControllerBase
    {
        private readonly IWillLearnService willLearnService;
        private readonly IMapper mapper;
        public WillLearnController(IWillLearnService willLearnService, IMapper mapper)
        {
            this.willLearnService = willLearnService;
            this.mapper = mapper;
        }
        // GET: api/<WillLearnController>
        [HttpGet]
        public async Task<ActionResult<WillLearnDTO>> Get()
        {
            var list = await willLearnService.GetAll();
            var listWillLearn = list.Select(x => mapper.Map<WillLearnDTO>(x)).ToList();
            return Ok(new { listWillLearn });
        }

        // GET api/<WillLearnController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WillLearnDTO>> Get(int id)
        {
            var willLearn = await willLearnService.GetById(id);
            if (willLearn == null)
            {
                return NotFound();
            }
            var willLearnDTO = mapper.Map<WillLearnDTO>(willLearn);
            return Ok(new { willLearnDTO });
        }

        // POST api/<WillLearnController>
        [HttpPost]
        public async Task<ActionResult<WillLearnDTO>> Post([FromBody] WillLearnDTO willLearnDTO)
        {
            var willLearn = mapper.Map<WillLearn>(willLearnDTO);
            willLearn = await willLearnService.Insert(willLearn);
            willLearnDTO = mapper.Map<WillLearnDTO>(willLearn);
            return Ok(new { willLearn });
        }

        // PUT api/<WillLearnController>/5
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

            return Ok(new { willLearnDTO });
        }

        // DELETE api/<WillLearnController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await willLearnService.Delete(id);
            return Ok();
        }
    }
}
