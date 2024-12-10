using RairBudgeting.Api.Domain.Interfaces;

namespace RairBudgeting.Api.Domain;
public class Entity : IEntity {
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;
}
