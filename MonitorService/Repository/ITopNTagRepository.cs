using MonitorService.Dto;
using System.Collections.Concurrent;

namespace MonitorService.Repository
{
    public interface ITopNTagRepository : ITagRepository
    {
        IReadOnlyDictionary<string, TagDto> getTopNCountByTag();

    }
}
