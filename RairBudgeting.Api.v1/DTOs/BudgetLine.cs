namespace RairBudgeting.Api.v1.DTOs;
public class BudgetLine {
    public int Id { get; set; }
    public string Name { get; set; }
    public BudgetCategory BudgetCategory { get; set; }
    public decimal Amount { get; set; }
    public IEnumerable<Note> Notes { get; set; }
    public bool IsDeleted { get; set; }
}
