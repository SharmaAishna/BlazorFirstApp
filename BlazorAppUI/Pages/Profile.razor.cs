
namespace BlazorAppUI.Pages;

public partial class Profile
{
    private UserModel loggedInUser;
    private List<SuggestionModel> submissions;
    private List<SuggestionModel> approved;
    private List<SuggestionModel> archived;
    private List<SuggestionModel> pending;
    private List<SuggestionModel> rejected;
    protected async override Task OnInitializedAsync()
    {
        loggedInUser = await authProvider.GetUserFromAuth(userData);
        var results = await suggestionData.GetUsersSuggestions(loggedInUser.Id);
        if (loggedInUser is not null && results is not null)
        {
            submissions = results.OrderByDescending(sug => sug.DateCreated).ToList();
            approved = submissions.Where(sug => sug.ApprovedForRelease && sug.Archived == false && sug.Rejected == false).ToList();
            archived = submissions.Where(sug => sug.Archived && sug.Rejected == false).ToList();
            pending = submissions.Where(sug => sug.ApprovedForRelease == false && sug.Rejected == false).ToList();
            rejected = submissions.Where(sug => sug.Rejected).ToList();
        }
    }

    private void ClosePage()
    {
        navManager.NavigateTo("/");
    }
}