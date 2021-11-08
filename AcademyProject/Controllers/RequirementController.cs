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
    public class RequirementController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IGenericService<Requirement> requirementrService;
        public RequirementController(IMapper mapper, IGenericService<Requirement> requirementrService)
        {
            this.mapper = mapper;
            this.requirementrService = requirementrService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RequirementDTO>>> Get()
        {
            var list = await requirementrService.GetAll();
            var listRequirement = list.Select(x => mapper.Map<RequirementDTO>(x)).ToList();
            return Ok(listRequirement);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RequirementDTO>> Get(int id)
        {
            var requirement = await requirementrService.GetById(id);
            if (requirement == null)
            {
                return NotFound();
            }
            var requirementDTO = mapper.Map<AnswerDTO>(requirement);
            return Ok(requirementDTO);
        }

        [HttpPost]
        public async Task<ActionResult<RequirementDTO>> Post([FromBody] RequirementDTO requirementDTO)
        {
            var requirement = mapper.Map<Requirement>(requirementDTO);
            requirement = await requirementrService.Insert(requirement);
            requirementDTO = mapper.Map<RequirementDTO>(requirement);
            return Ok(requirementDTO);
        }

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

            return Ok(requirementDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await requirementrService.Delete(id);
            return Ok();
        }
    }
}
