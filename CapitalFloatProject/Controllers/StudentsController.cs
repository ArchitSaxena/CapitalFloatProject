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
    public class StudentsController : ControllerBase
    {
        private readonly CapitalFloatDataContext _context;
        private readonly ILogger<StudentsController> _logger;
        public StudentsController(CapitalFloatDataContext context, ILogger<StudentsController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Students
        [HttpGet("Getstudents")]
        public async Task<ActionResult<IEnumerable<Student>>> Getstudents()
        {

            _logger.LogInformation("Invoke Getstudents");
            var startTime = DateTime.Now;
            var result= await _context.students.ToListAsync();
            _logger.LogInformation($"Getstudents completed in {(DateTime.Now - startTime).TotalSeconds}");
            return result;
        }

        [HttpGet("GetStudentOfCourse/{course}")]
        public async Task<List<Student>> GetStudentOfCourse(string course)
        {
            _logger.LogInformation("Invoke GetStudentOfCourse");
            var startTime = DateTime.Now;
            var studentList = await _context.students.Where(x => x.Course.Equals(course)).ToListAsync();
            _logger.LogInformation($"GetStudentOfCourse completed in {(DateTime.Now - startTime).TotalSeconds}");
            _logger.LogInformation($"Count of records- {studentList.Count}");
            return studentList;
        }

        [HttpGet("GetStudentOfCourse/{startStudentId}/{endStudentId}")]
        public async Task<List<Student>> GetStudentInRange(int startStudentId, int endStudentId)
        {
            _logger.LogInformation("Invoke GetStudentInRange");
            var startTime = DateTime.Now;
            var studentList = await _context.students.Where(x => x.StudentID >= startStudentId && x.StudentID <= endStudentId).ToListAsync();
            _logger.LogInformation($"GetStudentInRange completed in {(DateTime.Now - startTime).TotalSeconds}");
            _logger.LogInformation($"Count of records- {studentList.Count}");
            return studentList;
        }

        [HttpPut("UpdateStudentDetails/{id}")]
        public async Task<IActionResult> UpdatetStudentDetails(int id, Student student)
        {
            _logger.LogInformation("Invoke UpdatetStudentDetails");
            var startTime = DateTime.Now;
            if (id != student.StudentID)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _logger.LogInformation($"UpdatetStudentDetails completed in {(DateTime.Now - startTime).TotalSeconds}");
            return NoContent();
        }


        [HttpPost("AddStudent")]
        public async Task<ActionResult<Student>> AddStudent(Student student)
        {
            _logger.LogInformation("Invoke AddStudent");
            try { 
                var x= _context.persons.Any(e => e.PersonID == student.PersonID);
                if (x)
                {
                    _context.students.Add(student);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetStudent", new { id = student.StudentID }, student);
                }
                else
                {
                    _logger.LogInformation("Record for this student doesn't exist in the Person Table");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception- {ex}");
            }
            return NoContent();
        }

        // DELETE: api/Students/5
        [HttpDelete("DeleteStudent/{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {
            _logger.LogInformation("Invoke DeleteStudent");
            var student = await _context.students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.students.Remove(student);
            await _context.SaveChangesAsync();

            return student;
        }
        [HttpDelete("DeleteStudent/{startId}/{endId}")]
        public async Task<ActionResult> DeleteStudents(int startId, int endId)
        {
            _logger.LogInformation("Invoke DeleteStudents");
            var students = await _context.students.Where(x => x.StudentID >= startId && x.StudentID <= endId).ToListAsync();
            if (students == null)
            {
                return NotFound();
            }
            try
            {
                _context.students.RemoveRange(students);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error-{ex}");
            }
            return Ok();
        }
        private bool StudentExists(int id)
        {
            return _context.students.Any(e => e.StudentID == id);
        }
    }
}
