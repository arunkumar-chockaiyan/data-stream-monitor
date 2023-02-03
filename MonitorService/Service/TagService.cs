using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;
using MonitorService.Dto;
using MonitorService.Repository;

namespace MonitorService.Service
{
    public class TagService : ITagService
    {
        private static Regex tagPattern = new Regex(@"#[a-zA-Z/_0-9]+");

        private readonly ITagRepository tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        public List<TagDto> getTop10Tags()
        {
            //TODO: update to handle very large set. Do not want to sort every time
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
