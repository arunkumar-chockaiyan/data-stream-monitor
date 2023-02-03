using System.Collections.Concurrent;
using MonitorService.Dto;

namespace MonitorService.Repository
{
    public interface ITagRepository
    {
        void incrementTagCount(string tag);
        ConcurrentDictionary<string, TagDto> getAllCountByTag();
    }
}
