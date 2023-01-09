using BlazorAppUI.Models;

namespace BlazorAppUI.Pages;

public partial class Create
{
    private CreateSuggestionModel suggestion = new();
    private List<CategoryModel> categories;
    private UserModel loggedInUser;
    protected async override Task OnInitializedAsync()
    {
        categories = await categoryData.GetAllCategories();
        loggedInUser = await authProvider.GetUserFromAuth(userData);
    }

    private void ClosePage()
    {
        navManager.NavigateTo("/");
    }

    private async Task CreateSuggestion()
    {
        SuggestionModel sug = new();
        sug.Suggestion = suggestion.Suggestion;
        sug.Description = suggestion.Description;
        sug.Author = new BasicUserModel(loggedInUser);
        sug.Category = categories.Where(cat => cat.Id == suggestion.CategoryId).FirstOrDefault();
        if (sug.Category is null)
        {
            suggestion.CategoryId = "";
            return;
        }

        await suggestionData.CreateSuggestion(sug);
        suggestion = new();
        ClosePage();
    }
}