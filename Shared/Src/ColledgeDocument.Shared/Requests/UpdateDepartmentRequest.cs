namespace ColledgeDocument.Shared.Requests;

public class UpdateDepartmentRequest
{
    [JsonPropertyName("title")]
    public string Title { get; set; }
}
