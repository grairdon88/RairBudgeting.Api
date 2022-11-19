using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs;
public class Payment {
    public int Id { get; set; }
    public float Amount { get; set; }
    public bool IsDeleted { get; set; }
}
