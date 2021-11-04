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
    public class RequirementController : ControllerBase
    {
        private readonly IGenericService<Requirement> requirementrService;
        private readonly IMapper mapper;
        public RequirementController(IGenericService<Requirement> requirementrService, IMapper mapper)
        {
            this.requirementrService = requirementrService;
            this.mapper = mapper;
        }
        // GET: api/<RequirementController>
        [HttpGet]
        public async Task<ActionResult<RequirementDTO>> Get()
        {
            var list = await requirementrService.GetAll();
            var listRequirement = list.Select(x => mapper.Map<RequirementDTO>(x)).ToList();
            return Ok(new { listRequirement });
        }

        // GET api/<RequirementController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequirementDTO>> Get(int id)
        {
            var requirement = await requirementrService.GetById(id);
            if (requirement == null)
            {
                return NotFound();
            }
            var requirementDTO = mapper.Map<AnswerDTO>(requirement);
            return Ok(new { requirementDTO });
        }

        // POST api/<RequirementController>
        [HttpPost]
        public async Task<ActionResult<RequirementDTO>> Post([FromBody] RequirementDTO requirementDTO)
        {
            var requirement = mapper.Map<Requirement>(requirementDTO);
            requirement = await requirementrService.Insert(requirement);
            requirementDTO = mapper.Map<RequirementDTO>(requirement);
            return Ok(new { requirement });
        }

        // PUT api/<RequirementController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<RequirementDTO>> Put(int id, [FromBody] RequirementDTO requirementDTO)
        {
            var requirement = await requirementrService.GetById(id);
            if (requirement == null)
            {
                return NotFound();
            }
            requirement.CourseId = requirementDTO.CourseId;
            requirement.Content = requirementDTO.Content;
            requirement = await requirementrService.Update(requirement);

            requirementDTO = mapper.Map<RequirementDTO>(requirement);

            return Ok(new { requirementDTO });
        }

        // DELETE api/<RequirementController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await requirementrService.Delete(id);
            return Ok();
        }
    }
}
