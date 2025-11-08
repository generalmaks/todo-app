using Microsoft.AspNetCore.Mvc;
using TodoAppBackend.Application.DTOs.Category;
using TodoAppBackend.Application.Services;
using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Controllers;

[ApiController]
[Route("/api/categories")]
public class CategoryController(CategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetCategory(int categoryId)
    {
        var category = await categoryService.GetCategoryByIdAsync(categoryId);
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryDto category)
    {
        await categoryService.CreateCategoryAsync(category);
        return Created();
    }
}