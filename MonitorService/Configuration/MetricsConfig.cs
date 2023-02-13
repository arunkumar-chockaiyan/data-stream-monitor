namespace MonitorService.Configuration
{
    public class MetricsConfig
    {
        public static readonly string SECTION_NAME = "Metrics";
        public TagsConfig? Tags { get; set; }
        public class TagsConfig
        {
            public static readonly int DEFAULT_TOPLIST_SIZE = 10;
            public int TopListSize { get; set; } = 10;
        }
    }
}
