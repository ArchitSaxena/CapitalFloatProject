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

        [HttpGet("GetStudentInRange/{startStudentId}/{endStudentId}")]
        public async Task<List<Student>> GetStudentInRange(int startStudentId, int endStudentId)
        {
            _logger.LogInformation("Invoke GetStudentInRange");
            var startTime = DateTime.Now;
            var studentList = await _context.students.Where(x => x.StudentID >= startStudentId && x.StudentID <= endStudentId).ToListAsync();
            _logger.LogInformation($"GetStudentInRange completed in {(DateTime.Now - startTime).TotalSeconds}");
            _logger.LogInformation($"Count of records- {studentList.Count}");
            return studentList;
        }

        [HttpGet("GetStudentDetails/{studentId}")]
        public async Task<ActionResult<StudentInfo>> GetStudentDetails(int studentId)
        {
            _logger.LogInformation("Invoke GetStudentDetails");
            var startTime = DateTime.Now;
            var studentContact = await _context.studentsContactInfo.Where(x => x.StudentID == studentId).ToListAsync();
            if( !studentContact.Any())
            {
                _logger.LogInformation("Contact details missing!!");
                var studentDetail = await (from student in _context.students.Where(x => x.StudentID == studentId)
                                         join person in _context.persons on student.PersonID equals person.PersonID
                                         select new StudentInfo
                                         {
                                             StudentID = student.StudentID,
                                             StudentName = person.FirstName + " " + person.LastName,
                                             PhoneNo = 0,
                                             Email = "N/A",
                                             Course = student.Course,
                                             DOB = student.DOB,
                                             FatherName = "N/A",
                                             Address = person.Address,
                                             City = person.City
                                         }).FirstOrDefaultAsync();
                return studentDetail;
            }    
            var studentinfo = await( from student in _context.students.Where(x => x.StudentID == studentId)
                                join person in _context.persons on student.PersonID equals person.PersonID
                                join contact in _context.studentsContactInfo on student.StudentID equals contact.StudentID
                                select new StudentInfo
                                {
                                    StudentID = student.StudentID,
                                    StudentName = person.FirstName + " " + person.LastName,
                                    PhoneNo = contact.PhoneNo,
                                    Email = contact.Email,
                                    Course = student.Course,
                                    DOB = student.DOB,
                                    FatherName = contact.FatherName,
                                    Address = contact.Address,
                                    City = person.City
                                }).FirstOrDefaultAsync();

            _logger.LogInformation($"GetStudentDetails completed in {(DateTime.Now - startTime).TotalSeconds}");
            return studentinfo;
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

        [HttpPut("UpdateStudentContactDetails/{id}")]
        public async Task<IActionResult> UpdateStudentContactDetails(int id, StudentContactInfo studentContact)
        {
            _logger.LogInformation("Invoke UpdateStudentContactDetails");
            var startTime = DateTime.Now;
            if (id != studentContact.StudentID)
            {
                return BadRequest();
            }

            _context.Entry(studentContact).State = EntityState.Modified;

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
            _logger.LogInformation($"UpdateStudentContactDetails completed in {(DateTime.Now - startTime).TotalSeconds}");
            return Ok();
        }

        [HttpPost("AddStudent")]
        public async Task<ActionResult> AddStudent(Student student)
        {
            _logger.LogInformation("Invoke AddStudent");
            try { 
                var x= _context.persons.Any(e => e.PersonID == student.PersonID);
                if (x)
                {
                    _context.students.Add(student);
                    await _context.SaveChangesAsync();
                    return Ok();
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

        [HttpPost("AddStudentContact")]
        public async Task<ActionResult> AddStudentContact(StudentContactInfo studentContact)
        {
            _logger.LogInformation("Invoke AddStudentContact");
            try
            {
                var x = _context.students.Any(e => e.StudentID == studentContact.StudentID);
                if (x)
                {
                    _context.studentsContactInfo.Add(studentContact);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    _logger.LogInformation("Record for this student doesn't exist in the Student Table");
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
            var studentContact = await _context.studentsContactInfo.FindAsync(id);
            if (studentContact != null)
                _context.studentsContactInfo.Remove(studentContact);
            await _context.SaveChangesAsync();
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
                var studentContact = await _context.studentsContactInfo.Where(x => x.StudentID >= startId && x.StudentID <= endId).ToListAsync();
                if (studentContact != null)
                    _context.studentsContactInfo.RemoveRange(studentContact);
                await _context.SaveChangesAsync();
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
