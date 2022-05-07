using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdfTemplator.Server.Data;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Server.Controllers
{
    [ApiController]
    [Route("api/category")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _db.Categories.ToListAsync();
            return Ok(await Result<List<Category>>.SuccessAsync(categories));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return Ok(await Result<Category>.FailAsync("Not found!"));

            return Ok(await Result<Category>.SuccessAsync(category));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryParameters categoryParams)
        {
            var category = categoryParams.ToCategory();
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return Ok(await Result<Category>.SuccessAsync(category, "Category created"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(CategoryParameters categoryParams, int id)
        {
            var dbCategory = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (dbCategory == null)
                return Ok(await Result<int>.FailAsync("Not found!"));

            dbCategory.Name = categoryParams.Name;

            _db.Update(dbCategory);
            await _db.SaveChangesAsync();

            return Ok(await Result<Category>.SuccessAsync(dbCategory, "Category updated"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbCategory = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (dbCategory == null)
                return Ok(await Result<int>.FailAsync("Not found!"));

            _db.Categories.Remove(dbCategory);
            await _db.SaveChangesAsync();

            return Ok(await Result<int>.SuccessAsync(dbCategory.Id, "Category deleted"));
        }

        [HttpGet("{id}/pdfTemplates")]
        public async Task<IActionResult> GetPdfTemplates(int id)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return Ok(await Result<PdfTemplate>.FailAsync("Category not found!"));

            var pdfTemplates = _db.PdfTemplates.Where(x => x.CategoryId == id).ToList() ?? new();
            return Ok(await Result<List<PdfTemplate>>.SuccessAsync(pdfTemplates));
        }
    }
}