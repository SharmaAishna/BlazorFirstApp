namespace BlazorClassLibrary.DataAccess;

public interface ISuggestionData
{
    Task CreateSuggestion(SuggestionModel suggestion);
    Task<List<SuggestionModel>> GetAllApprovedSuggestions();
    Task<List<SuggestionModel>> GetAllSuggestion();
    Task<List<SuggestionModel>> GetAllSuggestionWaitingForApproval();
    Task<SuggestionModel> GetSuggestion(string id);
    Task UpdateSuggestion(SuggestionModel suggestion);
    Task UpVoteSuggestion(string suggestionId, string userId);
}