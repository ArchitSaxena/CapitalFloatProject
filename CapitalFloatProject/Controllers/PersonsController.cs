using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapitalFloatProject.DataAccess;
using CapitalFloatProject.Models;

namespace CapitalFloatProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly CapitalFloatDataContext _context;

        public PersonsController(CapitalFloatDataContext context)
        {
            _context = context;
        }

        // GET: api/Persons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Persons>>> Getpersons()
        {
            return await _context.persons.ToListAsync();
        }

        // GET: api/Persons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Persons>> GetPersons(int id)
        {
            var persons = await _context.persons.FindAsync(id);
            var personsFound = _context.persons.Where(x=> x.PersonID==id);
            if (persons == null)
            {
                return NotFound();
            }

            return persons;
        }

        // PUT: api/Persons/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersons(int id, Persons persons)
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

        // POST: api/Persons
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Persons>> PostPersons(Persons persons)
        {
            _context.persons.Add(persons);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersons", new { id = persons.PersonID }, persons);
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
