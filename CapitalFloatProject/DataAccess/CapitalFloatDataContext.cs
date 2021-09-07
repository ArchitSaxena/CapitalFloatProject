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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persons>(builder =>
            {
                builder.ToTable("Persons").HasKey(x => x.PersonID);
            });
        }
    }
}
