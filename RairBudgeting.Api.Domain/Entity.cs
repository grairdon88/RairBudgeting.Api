using Newtonsoft.Json;
using RairBudgeting.Api.Domain.Interfaces;
using System.Text.Json.Serialization;

namespace RairBudgeting.Api.Domain;
public abstract class Entity : IEntity {
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("partitionKey")]

    public string PartitionKey { get { return Id.ToString(); } }
}
