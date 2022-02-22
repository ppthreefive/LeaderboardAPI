namespace LeaderboardAPI.Models
{
    public record Entry
    {
        public string username { get; init; }
        public int score { get; init; }
        public int index { get; init; }
    }
}