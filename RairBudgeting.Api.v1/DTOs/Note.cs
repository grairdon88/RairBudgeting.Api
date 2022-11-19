using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs;
public class Note {
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;

    public bool IsDeleted { get; set; } = false;
}
