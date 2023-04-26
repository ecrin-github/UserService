namespace UserService.Contracts.Access;

public class AccessResponse
{
    public IList<StudyResponse> Studies { get; set; } = new List<StudyResponse>();
}