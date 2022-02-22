namespace LeaderboardAPI.Dtos
{
    public record EntryDto
    {
        public string username { get; init; }
        public int score { get; init; }
        public int index { get; init; }
    }
}