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
            return Ok(new { categories });
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
            return Ok(new { category });
        }

        // PUT api/<CategoryController>/5
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

            return Ok(new { categoryDTO });
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await categoryService.Delete(id);
            return Ok();
        }
    }
}
