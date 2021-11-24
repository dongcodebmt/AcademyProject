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
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrators")]
    public class CategoryController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IGenericService<Category> categoryService;
        public CategoryController(IMapper mapper, IGenericService<Category> categoryService)
        {
            this.mapper = mapper;
            this.categoryService = categoryService;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<CategoryDTO>> Get()
        {
            var list = await categoryService.GetList(x => x.IsDeleted == false);
            var categories = list.Select(x => mapper.Map<CategoryDTO>(x)).ToList();
            return Ok(categories);
        }

        // GET: api/<CategoryController>/{id}
        //[HttpGet("{id}")]
        //public async Task<ActionResult<CategoryDTO>> Get(int id)
        //{
        //    var category = await categoryService.GetById(id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }
        //    var categoryDTO = mapper.Map<CategoryDTO>(category);
        //    return Ok(categoryDTO);
        //}

        // POST: api/<CategoryController>
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Post([FromBody] CategoryDTO categoryDTO)
        {
            var category = mapper.Map<Category>(categoryDTO);
            category = await categoryService.Insert(category);
            categoryDTO = mapper.Map<CategoryDTO>(category);
            return Ok(categoryDTO);
        }

        // PUT: api/<CategoryController>/{id}
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

        // DELETE: api/<CategoryController>/{id}
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
