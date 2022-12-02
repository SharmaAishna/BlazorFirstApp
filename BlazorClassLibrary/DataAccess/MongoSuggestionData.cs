

using Microsoft.Extensions.Caching.Memory;

namespace BlazorClassLibrary.DataAccess;
public class MongoSuggestionData : ISuggestionData
{
    private readonly IDbConnection _db;
    private readonly IUserData _user;
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<SuggestionModel> _suggestions;
    private const string CacheName = "SuggestionData";

    public MongoSuggestionData(IDbConnection db, IUserData userData, IMemoryCache cache)
    {
        _db = db;
        _user = userData;
        _cache = cache;
        _suggestions = db.SuggestionCollection;
    }
    public async Task<List<SuggestionModel>> GetAllSuggestion()
    {
        var output = _cache.Get<List<SuggestionModel>>(CacheName);
        if (output == null)
        {
            var result = await _suggestions.FindAsync(sugg => sugg.Archived == false);
            output = result.ToList();
            _cache.Set(CacheName, output, TimeSpan.FromMinutes(1));

        }
        return output;
    }
    public async Task<List<SuggestionModel>> GetAllApprovedSuggestions()
    {
        var output = await GetAllSuggestion();
        return output.Where(a => a.ApprovedForRelease).ToList();
    }
    public async Task<SuggestionModel> GetSuggestion(string id)
    {
        var results = await _suggestions.FindAsync(sug => sug.Id == id);
        return results.FirstOrDefault();
    }
    public async Task<List<SuggestionModel>> GetAllSuggestionWaitingForApproval()
    {
        var output = await GetAllSuggestion();
        return output.Where(app => app.ApprovedForRelease == false && app.Rejected == false).ToList();
    }
    public async Task UpdateSuggestion(SuggestionModel suggestion)
    {
        await _suggestions.ReplaceOneAsync(sug => sug.Id == suggestion.Id, suggestion);
        _cache.Remove(CacheName);
    }
    public async Task UpVoteSuggestion(string suggestionId, string userId)
    {
        var client = _db.Client;
        using var session = await client.StartSessionAsync();
        session.StartTransaction();
        try
        {
            var db = client.GetDatabase(_db.DbName);
            var suggestionInTransaction = db.GetCollection<SuggestionModel>(_db.SuggestionsCollectionName);
            var suggestion = (await suggestionInTransaction.FindAsync(s => s.Id == suggestionId)).First();
            bool isUpVote = suggestion.UserVotes.Add(userId);
            if (isUpVote == false)
            {
                suggestion.UserVotes.Remove(userId);
            }
            await suggestionInTransaction.ReplaceOneAsync(s => s.Id == suggestionId, suggestion);
            var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
            var user = await _user.GetUser(suggestion.Author.Id);
            if (isUpVote)
            {
                user.VotedOnSuggestions.Add(new BasicSuggestionModel(suggestion));
            }
            else
            {
                var suggestionToRemove = user.VotedOnSuggestions.Where(s => s.Id == suggestion.Id).First();
                user.VotedOnSuggestions.Remove(suggestionToRemove);
            }
            await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);
            await session.CommitTransactionAsync();
            _cache.Remove(CacheName);
        }
        catch (Exception ex)
        {
            await session.AbortTransactionAsync();
            throw;
        }

    }
    public async Task CreateSuggestion(SuggestionModel suggestion)
    {
        var client = _db.Client;
        using var session = await client.StartSessionAsync();
        session.StartTransaction();
        try
        {
            var db = client.GetDatabase(_db.DbName);
            var SugestionInTransaction = db.GetCollection<SuggestionModel>(_db.SuggestionsCollectionName);
            await SugestionInTransaction.InsertOneAsync(suggestion);
            var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
            var user = await _user.GetUser(suggestion.Author.Id);
            user.AuthoredSuggestions.Add(new BasicSuggestionModel(suggestion));
            await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);
            await session.CommitTransactionAsync();

        }
        catch (Exception ex)
        {
            await session.AbortTransactionAsync();
            throw;
        }
    }
}
