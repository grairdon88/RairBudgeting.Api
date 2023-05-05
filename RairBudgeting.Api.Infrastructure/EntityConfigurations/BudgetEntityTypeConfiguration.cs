using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RairBudgeting.Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.Infrastructure.EntityConfigurations;
public class BudgetEntityTypeConfiguration : IEntityTypeConfiguration<Budget> {
    public void Configure(EntityTypeBuilder<Budget> builder) {
        builder.ToTable("Budget", "dbo");

        builder.HasKey(b => b.Id);

        //builder.Property(o => o.Id)
        //    .UseHiLo("BudgetSequence", "dbo");

        builder.Property(b => b.BudgetTime)
            //.UsePropertyAccessMode(PropertyAccessMode.Property)
            .IsRequired();


        builder.Property(b => b.Amount)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(b => b.IsDeleted)
            //.UsePropertyAccessMode(PropertyAccessMode.Property)
            .IsRequired();

        // DDD Patterns comment:
        //Set as field (New since EF 1.1) to access the OrderItem collection property through its field

        //.HasForeignKey("BudgetId");

        //builder.HasMany(b => b.Lines)
        //    .WithOne(b => b.Budget)
        //    .HasForeignKey(b => b.BudgetId);
    }
}
