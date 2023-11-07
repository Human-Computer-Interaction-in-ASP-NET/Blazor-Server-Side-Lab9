using Microsoft.EntityFrameworkCore;
using ServerSide.Models;

public class StudentService
{
  private SchoolDbContext _context;

  public StudentService(SchoolDbContext context)
  {
    _context = context;
  }

  public async Task<List<Student>> GetStudentsAsync()
  {
    return await _context.Students.ToListAsync();
  }

  public async Task<Student?> GetStudentByIdAsync(int id)
  {
    return await _context.Students.FindAsync(id) ?? null;
  }

  public async Task<Student?> InsertStudentAsync(Student student)
  {
    _context.Students.Add(student);
    await _context!.SaveChangesAsync();

    return student;
  }

  public async Task<List<SchoolStudentCount>> GetStudentCountsBySchoolAsync()
  {
    var studentCounts = await _context.Students
        .GroupBy(s => s.School)
        .Select(group => new SchoolStudentCount
        {
          School = group.Key, // School name
          StudentCount = group.Count() // Number of students in the school
        })
        .ToListAsync();
    return studentCounts;
  }

public async Task<List<Student>> GetStudentsBySchoolAsync(string school)
{
      if (school != null){
        return await _context.Students
        .Where(s => s.School == school)
        .ToListAsync();
      } else {
        return await _context.Students
        .ToListAsync();
      }
}


  public async Task<List<string?>> GetSchoolsAsync(){
    return await _context.Students
        .Select(s => s.School)
        .Distinct()
        .ToListAsync();
  }

  public async Task<Student> UpdateStudentAsync(int id, Student s)
  {
    var student = await _context.Students!.FindAsync(id);

    if (student == null)
      return null!;

    student.FirstName = s.FirstName;
    student.LastName = s.LastName;
    student.School = s.School;

    _context.Students.Update(student);
    await _context.SaveChangesAsync();

    return student!;
  }

  public async Task<Student> DeleteStudentAsync(int id)
  {
    var student = await _context.Students!.FindAsync(id);

    if (student == null)
      return null!;

    _context.Students.Remove(student);
    await _context.SaveChangesAsync();

    return student!;
  }

  private bool StudentExists(int id)
  {
    return _context.Students!.Any(e => e.StudentId == id);
  }
}
