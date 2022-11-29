
namespace BlazorClassLibrary.Models
{
    public class SuggestionModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Suggestion { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public CategoryModel Category { get; set; }
        public BasicUserModel Author { get; set; }
        public HashSet<string> UserVotes { get; set; } = new();
        public StatusModel SuggestionsStatus { get; set; }
        public string OwnerNotes { get; set; }
        public bool ApprovedForRelease { get; set; }
        public bool Archived { get; set; }
        public bool Rejected { get; set; }
    }
}
