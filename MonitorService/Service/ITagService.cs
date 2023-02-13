using MonitorService.Dto;

namespace MonitorService.Service
{
    public interface ITagService
    {
        void incrementTagCount(string tag);
        List<TagDto> getTopNTags();
        void extractAndUpdateTagCount(string text);

    }
}
