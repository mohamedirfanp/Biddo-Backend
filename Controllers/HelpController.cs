using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biddo.Models;

namespace Biddo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpController : ControllerBase
    {
        private readonly BiddoContext _context;

        public HelpController(BiddoContext context)
        {
            _context = context;
        }

        // GET: api/Help
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QueryModel>>> GetQueryTable()
        {
          if (_context.QueryTable == null)
          {
              return NotFound();
          }
            return await _context.QueryTable.ToListAsync();
        }

        // GET: api/Help/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QueryModel>> GetQueryModel(int id)
        {
          if (_context.QueryTable == null)
          {
              return NotFound();
          }
            var queryModel = await _context.QueryTable.FindAsync(id);

            if (queryModel == null)
            {
                return NotFound();
            }

            return queryModel;
        }

        // PUT: api/Help/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQueryModel(int id, QueryModel queryModel)
        {
            if (id != queryModel.QueryId)
            {
                return BadRequest();
            }

            _context.Entry(queryModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QueryModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Help
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<QueryModel>> PostQueryModel(QueryModel queryModel)
        {
          if (_context.QueryTable == null)
          {
              return Problem("Entity set 'BiddoContext.QueryTable'  is null.");
          }
            _context.QueryTable.Add(queryModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQueryModel", new { id = queryModel.QueryId }, queryModel);
        }

        // DELETE: api/Help/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQueryModel(int id)
        {
            if (_context.QueryTable == null)
            {
                return NotFound();
            }
            var queryModel = await _context.QueryTable.FindAsync(id);
            if (queryModel == null)
            {
                return NotFound();
            }

            _context.QueryTable.Remove(queryModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QueryModelExists(int id)
        {
            return (_context.QueryTable?.Any(e => e.QueryId == id)).GetValueOrDefault();
        }
    }
}
