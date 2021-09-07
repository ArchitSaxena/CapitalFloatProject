using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapitalFloatProject.DataAccess;
using CapitalFloatProject.Models;
using Microsoft.Extensions.Logging;

namespace CapitalFloatProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly CapitalFloatDataContext _context;
        private readonly ILogger<PersonsController> _logger;
        public PersonsController(CapitalFloatDataContext context, ILogger<PersonsController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("GetPersons")]
        public async Task<ActionResult<IEnumerable<Persons>>> GetPersons()
        {
            return await _context.persons.ToListAsync();
        }

        // GET: api/Persons/5
        [HttpGet("GetPersonsById/{id}")]
        public async Task<ActionResult<Persons>> GetPersonsById(int id)
        {
            var persons = await _context.persons.FindAsync(id);
            if (persons == null)
            {
               return NotFound();
            }
            return persons;
        }

        [HttpPut("UpdatePersons/{id}")]
        public async Task<IActionResult> UpdatePersons(int id, Persons persons)
        {
            if (id != persons.PersonID)
            {
                return BadRequest();
            }

            _context.Entry(persons).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonsExists(id))
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


        [HttpPost("PostPersons")]
        public async Task<ActionResult<Persons>> PostPersons(Persons persons)
        {
            try
            {
                _context.persons.Add(persons);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetPersons", new { id = persons.PersonID }, persons);
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"Exception- {ex}");
            }
            return NoContent();
        }

        // DELETE: api/Persons/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Persons>> DeletePersons(int id)
        {
            var persons = await _context.persons.FindAsync(id);
            if (persons == null)
            {
                return NotFound();
            }

            _context.persons.Remove(persons);
            await _context.SaveChangesAsync();

            return persons;
        }

        private bool PersonsExists(int id)
        {
            return _context.persons.Any(e => e.PersonID == id);
        }
    }
}
