using RairBudgeting.Api.Domain.Entities;

namespace RairBudgeting.Api.Domain.Specifications;
public class BudgetWithLinesSpecification : BaseSpecification<Budget> {
    public BudgetWithLinesSpecification() : base(){
        AddInclude(x => x.Lines);
    }

    public BudgetWithLinesSpecification(int id) : base(x => x.Id == id) {
        AddInclude(x => x.Lines);
    }

    public BudgetWithLinesSpecification(int id, IEnumerable<string> includedEntities) : base(x => x.Id == id) {
        foreach (var includedEntity in includedEntities) {
            AddInclude($"{includedEntity}");
        }
    }

    //public BudgetWithLinesSpecification(IEnumerable<int> id) : base(x => id.SelectMany(j => j) {
    //    AddInclude(x => x.Lines);
    //}
}
