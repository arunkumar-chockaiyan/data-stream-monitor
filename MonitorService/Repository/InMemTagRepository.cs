using System.Collections.Concurrent;
using MonitorService.Dto;

namespace MonitorService.Repository
{
    /**
     * Naive implementation of in-memory store. 
     */
    public class InMemTagRepository : ITagRepository
    {
        private readonly ConcurrentDictionary<string, TagDto> countByTag;

        public InMemTagRepository()
        {
            countByTag = new ConcurrentDictionary<string, TagDto>();
        }

        public ConcurrentDictionary<string, TagDto> getAllCountByTag()
        {
            return countByTag;
        }

        public void incrementTagCount(string tag)
        {
            TagDto tagDto = countByTag.GetOrAdd(tag, new TagDto(tag, 0));
            tagDto.count++;
        }
    }
}
