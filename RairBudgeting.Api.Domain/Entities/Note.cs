using RairBudgeting.Api.Domain.Interfaces.Entities;

namespace RairBudgeting.Api.Domain.Entities;
public class Note : Entity, INote {
    public string Text { get; set; } = string.Empty;

    public bool IsDeleted { get; set; } = false;
}
