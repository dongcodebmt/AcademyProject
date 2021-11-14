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
    public class CategoryController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IGenericService<Category> categoryService;
        public CategoryController(IMapper mapper, IGenericService<Category> categoryService)
        {
            this.mapper = mapper;
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<CategoryDTO>> Get()
        {
            var list = await categoryService.GetList(x => x.IsDeleted == false);
            var categories = list.Select(x => mapper.Map<CategoryDTO>(x)).ToList();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> Get(int id)
        {
            var category = await categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDTO = mapper.Map<CategoryDTO>(category);
            return Ok(categoryDTO);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Post([FromBody] CategoryDTO categoryDTO)
        {
            var category = mapper.Map<Category>(categoryDTO);
            category = await categoryService.Insert(category);
            categoryDTO = mapper.Map<CategoryDTO>(category);
            return Ok(categoryDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDTO>> Put(int id, [FromBody] CategoryDTO categoryDTO)
        {
            var category = await categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            category.Name = categoryDTO.Name;
            category = await categoryService.Update(category);

            categoryDTO = mapper.Map<CategoryDTO>(category);

            return Ok(categoryDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            category.IsDeleted = true;
            await categoryService.Update(category);
            return Ok();
        }
    }
}
