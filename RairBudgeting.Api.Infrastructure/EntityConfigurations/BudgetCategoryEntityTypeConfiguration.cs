using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RairBudgeting.Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.Infrastructure.EntityConfigurations;
public class BudgetCategoryEntityTypeConfiguration : IEntityTypeConfiguration<BudgetCategory> {
    public void Configure(EntityTypeBuilder<BudgetCategory> builder) {
        builder.ToTable("BudgetCategory", "dbo");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .UsePropertyAccessMode(PropertyAccessMode.Property)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(b => b.Description)
            .UsePropertyAccessMode(PropertyAccessMode.Property)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(b => b.IsDeleted)
            .UsePropertyAccessMode(PropertyAccessMode.Property)
            .IsRequired();

        //var navigation = builder.Metadata.FindNavigation(nameof(BudgetLine.BudgetCategory));

        //navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
