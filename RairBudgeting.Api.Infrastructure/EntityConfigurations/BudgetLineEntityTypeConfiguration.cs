using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RairBudgeting.Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.Infrastructure.EntityConfigurations;
public class BudgetLineEntityTypeConfiguration : IEntityTypeConfiguration<BudgetLine> {
    public void Configure(EntityTypeBuilder<BudgetLine> builder) {
        builder.ToTable("BudgetLine", "dbo");

        builder.HasKey(b => b.Id);

        //builder.Property(o => o.Id)
        //    .UseHiLo("BudgetLineSequence", "dbo");

        builder.Property(b => b.Name)
            .UsePropertyAccessMode(PropertyAccessMode.Property)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(b => b.Amount)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(b => b.IsDeleted)
            .UsePropertyAccessMode(PropertyAccessMode.Property) 
            .IsRequired();

        // builder.Property<int>("BudgetForeignKey");

        //var navigation = builder.Metadata.FindNavigation(nameof(BudgetLine.BudgetCategory));

        //// DDD Patterns comment:
        ////Set as field (New since EF 1.1) to access the OrderItem collection property through its field
        //navigation.SetPropertyAccessMode(PropertyAccessMode.Property);

        ////builder.HasOne<Budget>()
        ////.WithMany();

        //builder.HasOne(o => o.BudgetCategory)
        //    .WithMany()
        //    .HasForeignKey("BudgetCategoryId")
        //    .IsRequired(true);
    }
}
