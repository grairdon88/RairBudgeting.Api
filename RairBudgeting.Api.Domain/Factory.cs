using RairBudgeting.Api.Domain.Interfaces;

namespace RairBudgeting.Api.Domain;
public class Factory<T> : IFactory<T> {
    private readonly Func<T> _objectBuilder;
    public Factory(Func<T> objectBuilder) {
        _objectBuilder = objectBuilder;
    }

    public T Create() {
        return _objectBuilder();
    }
}
