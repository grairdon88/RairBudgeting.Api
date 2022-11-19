using RairBudgeting.Api.Domain.Interfaces.Entities;

namespace RairBudgeting.Api.Domain.Entities;
public class Payment : Entity, IPayment {
    public float Amount { get; set; }
    public bool IsDeleted { get; set; } = false;
}
