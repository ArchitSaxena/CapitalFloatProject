using CapitalFloatProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapitalFloatProject.DataAccess
{
    public class CapitalFloatDataContext : DbContext
    {
        public CapitalFloatDataContext( DbContextOptions<CapitalFloatDataContext> dbContextOptions): base(dbContextOptions){ }
        public DbSet<Persons> persons { get; set; }
        public DbSet<Student> students { get; set; }
        public DbSet<StudentContactInfo> studentsContactInfo { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persons>(builder =>
            {
                builder.ToTable("Persons").HasKey(x => x.PersonID);
            });
            modelBuilder.Entity<Student>(builder =>
            {
                builder.ToTable("Student").HasKey(x => x.StudentID);
            });
            modelBuilder.Entity<StudentContactInfo>(builder =>
            {
                builder.ToTable("StudentContactInfo").HasKey(x => x.StudentID);
            });
        }
    }
}
