namespace MonitorService.Dto
{
    public class TweetDto
    {
        public string Id { get; }
        public string Text { get; }

        public TweetDto(string id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}
