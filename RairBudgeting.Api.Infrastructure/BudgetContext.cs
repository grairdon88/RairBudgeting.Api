using Microsoft.EntityFrameworkCore;
using RairBudgeting.Api.Domain.Entities;
using RairBudgeting.Api.Infrastructure.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.Infrastructure;
public class BudgetContext : DbContext {
    public BudgetContext(DbContextOptions<BudgetContext> options) : base(options) {

    }

    public DbSet<BudgetCategory> BudgetCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        modelBuilder.ApplyConfiguration(new BudgetCategoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BudgetEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BudgetLineEntityTypeConfiguration());
        //modelBuilder.Entity<BudgetCategory>().ToTable("BudgetCategory");
        modelBuilder.Entity<Note>().ToTable("Note");
    }
}
