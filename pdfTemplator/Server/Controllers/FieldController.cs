using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdfTemplator.Server.Data;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Server.Controllers
{
    [ApiController]
    [Route("api/field")]
    [Authorize]
    public class FieldController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public FieldController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int id)
        {
            var fields = await _db.Fields.Include(x => x.Templates).ToListAsync();
            return Ok(await Result<List<Field>>.SuccessAsync(fields));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var field = await _db.Fields.Include(x => x.Templates).FirstOrDefaultAsync(x => x.Id == id);

            if (field == null)
                return Ok(await Result<Field>.FailAsync("Field not found!"));

            return Ok(await Result<Field>.SuccessAsync(field));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Field field)
        {
            _db.Fields.Add(field);
            await _db.SaveChangesAsync();

            return Ok(await Result<Field>.SuccessAsync(field, "Field created"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Field field, int id)
        {
            var dbField = await _db.Fields.Include(x => x.Templates).FirstOrDefaultAsync(x => x.Id == id);

            if (dbField == null)
                return Ok(await Result<int>.FailAsync("Field not found!"));

            dbField.Key = field.Key;
            dbField.Type = field.Type;
            dbField.ParamsJSON = field.ParamsJSON;

            if (field.Templates != null)
            {
                dbField.Templates!.Clear();
                foreach (var item in field.Templates)
                {
                    var template = _db.Templates.FirstOrDefault(x => x.Id == item.Id);

                    if (template == null)
                        return Ok(await Result<int>.FailAsync("Template not found!"));

                    dbField.Templates.Add(template);
                }
            }

            _db.Update(dbField);
            await _db.SaveChangesAsync();

            return Ok(await Result<Field>.SuccessAsync(dbField, "Field updated"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbField = await _db.Fields.FirstOrDefaultAsync(x => x.Id == id);

            if (dbField == null)
                return Ok(await Result<int>.FailAsync("Not found!"));

            _db.Fields.Remove(dbField);
            await _db.SaveChangesAsync();

            return Ok(await Result<int>.SuccessAsync(dbField.Id, "Field deleted"));
        }
    }
}