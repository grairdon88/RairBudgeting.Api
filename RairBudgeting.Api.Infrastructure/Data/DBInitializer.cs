using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.Infrastructure.Data;
public static class DBInitializer {
    public static void Init(BudgetContext context) {
        context.Database.Migrate();
    }
}
