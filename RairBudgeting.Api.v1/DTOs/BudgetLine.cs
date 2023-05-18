namespace RairBudgeting.Api.v1.DTOs;
public class BudgetLine {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid BudgetCategoryId { get; set; }
    public decimal Amount { get; set; }
    public bool IsDeleted { get; set; }
    public Guid BudgetId { get; set; }
    public decimal PaymentAmount { get; set; }
}
