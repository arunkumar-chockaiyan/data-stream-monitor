namespace MonitorService.Dto
{
    public class TagDto
    {
        public string Tag { get; set; }
        public long count { get; set; }

        public TagDto(string tag, long count)
        {
            Tag = tag;
            this.count = count;
        }
    }
}
