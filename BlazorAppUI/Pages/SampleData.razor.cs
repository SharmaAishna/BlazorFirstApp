

namespace BlazorAppUI.Pages;

public partial class SampleData
{
    private bool categoriesCreated = false;
    private bool StatusesCreated = false;
    private bool SampleDataCreated = false;
    private async Task CreateCategories()
    {
        var categories = await categoryData.GetAllCategories();
        if (categories?.Count > 0)
        {
            return;
        }

        CategoryModel cat = new()
        {CategoryName = "courses", CategoryDescription = "Full paid couses"};
        await categoryData.CreateCategory(cat);
        cat = new()
        {CategoryName = "Questions", CategoryDescription = "Advice"};
        await categoryData.CreateCategory(cat);
        cat = new()
        {CategoryName = "In-depth Tutorial", CategoryDescription = "video how to use a topic"};
        await categoryData.CreateCategory(cat);
        cat = new()
        {CategoryName = "10-Minute Training", CategoryDescription = "A quick how do I use this? Video "};
        await categoryData.CreateCategory(cat);
        cat = new()
        {CategoryName = "Other", CategoryDescription = "Not sure which category this fit."};
        await categoryData.CreateCategory(cat);
        categoriesCreated = true;
    }

    private async Task CreateStatuses()
    {
        var statuses = await statusData.GetAllStatuses();
        if (statuses?.Count > 0)
        {
            return;
        }

        StatusModel stat = new()
        {StatusName = "Completed", StatusDescription = "The status is completed"};
        await statusData.CreateStatus(stat);
        stat = new()
        {StatusName = "Watching", StatusDescription = "The status is interesting. we are watching to see how much interested by Admin"};
        await statusData.CreateStatus(stat);
        stat = new()
        {StatusName = "Upcoming", StatusDescription = "The suggestion was accepted and it will be released soon."};
        await statusData.CreateStatus(stat);
        stat = new()
        {StatusName = "Dismissed", StatusDescription = "The status is rejected by Admin"};
        await statusData.CreateStatus(stat);
        StatusesCreated = true;
    }

    private async Task GenerateSampleData()
    {
        UserModel user = new()
        {FirstName = "Aishna", LastName = "Sharma", EmailAddress = "aish@gmail.com", DisplayName = " sample ASharma", ObjectIdentifier = "abc-123"};
        await userData.CreateUser(user);
        var foundUser = await userData.GetUsersFromAuthentication("abc-123");
        var categories = await categoryData.GetAllCategories();
        var statuses = await statusData.GetAllStatuses();
        HashSet<string> votes = new();
        votes.Add("1");
        votes.Add("2");
        votes.Add("3");
        SuggestionModel suggestion = new()
        {Author = new BasicUserModel(foundUser), Category = categories[0], Suggestion = "Our First Suggestion", Description = "Suggestion by sample data", ApprovedForRelease = true};
        await suggestData.CreateSuggestion(suggestion);
        suggestion = new()
        {Author = new BasicUserModel(foundUser), Category = categories[1], Suggestion = "Our second Suggestion", Description = "Suggestion by sample data", SuggestionsStatus = statuses[0], OwnerNotes = "This is    note for status.", ApprovedForRelease = true};
        await suggestData.CreateSuggestion(suggestion);
        suggestion = new()
        {Author = new BasicUserModel(foundUser), Category = categories[2], Suggestion = "Our Third Suggestion", Description = "Suggestion by sample data", SuggestionsStatus = statuses[1], OwnerNotes = "This is note for status.", ApprovedForRelease = true};
        await suggestData.CreateSuggestion(suggestion);
        suggestion = new()
        {Author = new BasicUserModel(foundUser), Category = categories[3], Suggestion = "Our Fourth Suggestion", Description = "Suggestion by sample data", SuggestionsStatus = statuses[2], UserVotes = votes, OwnerNotes = "This is note for status.", ApprovedForRelease = true};
        await suggestData.CreateSuggestion(suggestion);
        votes.Add("4");
        suggestion = new()
        {Author = new BasicUserModel(foundUser), Category = categories[4], Suggestion = "Our Fifth Suggestion", Description = "Suggestion by sample data", SuggestionsStatus = statuses[3], UserVotes = votes, OwnerNotes = "This is note for status.", ApprovedForRelease = true};
        await suggestData.CreateSuggestion(suggestion);
        SampleDataCreated = true;
    }
}