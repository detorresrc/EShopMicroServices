using System.Text.Json.Serialization;

namespace SharedKernel;

public class CustomErrorDetails
{
    [JsonPropertyName("titlee")]
    public string Title { get; set; } = default!;
    [JsonPropertyName("detail")]
    public string Detail { get; set; } = default!;
    [JsonPropertyName("statusCode")]
    public int StatusCode { get; set; } = default!;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("instance")]
    public string? Instance { get; set; } = default!;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("traceId")]
    public string? TraceId { get; set; } = default!;
    [JsonPropertyName("errors")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<ValidationError>? Errors { get; set; } = default!;
}

public class ValidationError
{
    public string PropertyName { get; set; } = default!;
    public string ErrorMessage { get; set; } = default!;
}