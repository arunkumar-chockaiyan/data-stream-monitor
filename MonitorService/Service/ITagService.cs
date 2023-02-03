using MonitorService.Dto;

namespace MonitorService.Service
{
    public interface ITagService
    {
        void incrementTagCount(string tag);
        List<TagDto> getTop10Tags();
        void extractAndUpdateTagCount(string text);

        //Possible extension
        //List<TagDto> getTopNTags(long topCount);
    }
}
