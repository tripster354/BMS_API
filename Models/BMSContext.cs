using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BudgetManagement.Models
{
    public partial class BMSContext : DbContext
    {
        public BMSContext()
        {
        }

        public BMSContext(DbContextOptions<BMSContext> options)
            : base(options)
        {
        }

        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer("server=44.225.98.134;uid=sa;pwd=pass@1234;database=BudgetManagement__Demo");
                optionsBuilder.UseSqlServer("server=43.204.37.101;uid=dbBookMySkill;pwd=cId1NzffgIg;database=dbBookMySkill");
            }
        }
        //Data Source=.;Initial Catalog=dbBookMySkill;Integrated Security=True;
        //"server=43.204.37.101;uid=sa;pwd=cId1NzffgIg;database=dbBookMySkill"
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
