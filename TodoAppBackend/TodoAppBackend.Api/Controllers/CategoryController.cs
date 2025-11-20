using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAppBackend.Application.DTOs.Category;
using TodoAppBackend.Application.Interfaces;
using TodoAppBackend.Application.Services;
using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Controllers;

[ApiController]
[Route("/api/categories")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [Authorize]
    [HttpGet("{categoryId:int}")]
    public async Task<ActionResult> GetCategory(int categoryId)
    {
        var senderEmail = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        if (string.IsNullOrEmpty(senderEmail))
            return Unauthorized();

        var foundCategory = await categoryService.GetCategoryByIdAsync(categoryId);

        if (foundCategory?.UserEmailId != senderEmail)
            return Forbid();
        
        return Ok(foundCategory);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateCategoryAsync([FromBody] CreateCategoryDto category)
    {
        var senderEmail = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        if (string.IsNullOrEmpty(senderEmail))
            return Unauthorized();

        if (senderEmail != category.UserEmailId)
            return Forbid();
        
        await categoryService.CreateCategoryAsync(category);
        return Created();
    }

    [Authorize]
    [HttpGet("{userEmailId}")]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesByUserEmailId(string userEmailId)
    {
        var senderEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(senderEmail))
            return Unauthorized();

        if (senderEmail != userEmailId)
            return Forbid();

        var categories = await categoryService.GetCategoryByUserEmailIdAsync(userEmailId);
        return Ok(categories);
    }
    
    [Authorize]
    [HttpPut("{categoryId:int}")]
    public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] UpdateCategoryDto dto)
    {
        var senderEmail = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        if (string.IsNullOrEmpty(senderEmail))
            return Unauthorized();
        
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var foundCategory = await categoryService.GetCategoryByIdAsync(categoryId);

        if (foundCategory?.UserEmailId != senderEmail)
            return Forbid();

        await categoryService.UpdateCategoryAsync(categoryId, dto);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{categoryId:int}")]
    public async Task<IActionResult> DeleteCategory(int categoryId)
    {
        var senderEmail = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        if (string.IsNullOrEmpty(senderEmail))
            return Unauthorized();

        var foundCategory = await categoryService.GetCategoryByIdAsync(categoryId);

        if (foundCategory?.UserEmailId != senderEmail)
            return Forbid();
        
        await categoryService.DeleteCategoryAsync(categoryId);
        return NoContent();
    }
}