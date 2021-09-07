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
            _logger.LogInformation("Invoke GetPersons");
            var startTime = DateTime.Now;
            var result = await _context.persons.ToListAsync();
            _logger.LogInformation($"GetPersons completed in {(DateTime.Now - startTime).TotalSeconds}");
            _logger.LogInformation($"Count of records- {result.Count}");
            return result;
        }

        // GET: api/Persons/5
        [HttpGet("GetPersonsById/{id}")]
        public async Task<ActionResult<Persons>> GetPersonsById(int id)
        {
            _logger.LogInformation("Invoke GetPersonsById");
            var startTime = DateTime.Now;
            var persons = await _context.persons.FindAsync(id);
            if (persons == null)
            {
               return NotFound();
            }
            _logger.LogInformation($"GetPersonsById completed in {(DateTime.Now - startTime).TotalSeconds}");
            return persons;
        }

        [HttpPost("AddPerson")]
        public async Task<ActionResult<Persons>> AddPerson(Persons persons)
        {
            _logger.LogInformation("Invoke AddPerson");
            var startTime = DateTime.Now;
            try
            {
                _context.persons.Add(persons);
                await _context.SaveChangesAsync();
                return CreatedAtAction("AddPerson", new { id = persons.PersonID }, persons);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception- {ex}");
            }
            _logger.LogInformation($"AddPerson completed in {(DateTime.Now - startTime).TotalSeconds}");
            return NoContent();
        }

        [HttpPost("AddPeople")]
        public async Task<ActionResult> AddPeople(List<Persons> persons)
        {
            _logger.LogInformation("Invoke AddPeople");
            var startTime = DateTime.Now;
            try
            {
                _context.persons.AddRange(persons);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception- {ex}");
            }
            _logger.LogInformation($"AddPeople completed in {(DateTime.Now - startTime).TotalSeconds}");
            return NoContent();
        }

        [HttpPut("UpdatePersons/{id}")]
        public async Task<IActionResult> UpdatePersons(int id, Persons persons)
        {
            _logger.LogInformation("Invoke UpdatePersons");
            var startTime = DateTime.Now;
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
            _logger.LogInformation($"UpdatePersons completed in {(DateTime.Now - startTime).TotalSeconds}");
            return NoContent();
        }

        // DELETE: api/Persons/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePersons(int id)
        {
            _logger.LogInformation("Invoke DeletePersons");
            var persons = await _context.persons.FindAsync(id);
            if (persons == null)
            {
                return NotFound();
            }
            _context.persons.Remove(persons);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool PersonsExists(int id)
        {
            return _context.persons.Any(e => e.PersonID == id);
        }
    }
}
