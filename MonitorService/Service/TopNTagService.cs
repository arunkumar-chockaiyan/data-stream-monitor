using MonitorService.Dto;
using MonitorService.Repository;

namespace MonitorService.Service
{
    public class TopNTagService : TagService
    {
        private readonly ITopNTagRepository topNTagRepository;

        public TopNTagService(ITopNTagRepository topNTagRepository) : base(topNTagRepository) 
        {
            this.topNTagRepository = topNTagRepository;
        }


        public override List<TagDto> getTop10Tags()
        {
            return this.topNTagRepository.getTopNCountByTag()
                .OrderByDescending(kv => kv.Value.count)
                .Select(kv => kv.Value)
                .ToList();
        }

    }
}
