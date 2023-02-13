using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;
using MonitorService.Dto;
using MonitorService.Repository;
using MonitorService.Configuration;

namespace MonitorService.Service
{
    public class TagService : ITagService
    {
        private static Regex tagPattern = new Regex(@"#[a-zA-Z/_0-9]+");

        private readonly ITagRepository tagRepository;
        private readonly MetricsConfig metricsConfig;

        private readonly int topListSize;

        public TagService(ITagRepository tagRepository, MetricsConfig metricsConfig)
        {
            this.tagRepository = tagRepository;
            this.metricsConfig = metricsConfig;
            this.topListSize = this.metricsConfig.Tags?.TopListSize ?? MetricsConfig.TagsConfig.DEFAULT_TOPLIST_SIZE;
        }

        public virtual List<TagDto> getTopNTags()
        {
            return this.tagRepository
                .getAllCountByTag()
                .OrderByDescending(kp => kp.Value.count)
                .Select(kp => kp.Value)
                .Take(10)
                .ToList();
        }

        public void incrementTagCount(string tag)
        {
            this.tagRepository.incrementTagCount(tag);
        }

        public void extractAndUpdateTagCount(string text)
        {
            ExtractTags(text).ForEach(this.tagRepository.incrementTagCount);
        }

        private List<string> ExtractTags(string text)
        {
            return tagPattern.Matches(text)
                .Select(m => m.Value)
                .Distinct()
                .ToList();
        }

    }
}
