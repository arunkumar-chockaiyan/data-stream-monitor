using MonitorService.Configuration;
using MonitorService.Dto;
using MonitorService.Repository;

namespace MonitorService.Service
{
    public class TopNTagService : TagService
    {
        private readonly ITopNTagRepository topNTagRepository;
        private readonly MetricsConfig metricsConfig;

        public TopNTagService(ITopNTagRepository topNTagRepository, MetricsConfig metricsConfig) : base(topNTagRepository, metricsConfig)
        {
            this.topNTagRepository = topNTagRepository;
            this.metricsConfig = metricsConfig;
        }


        public override List<TagDto> getTopNTags()
        {
            return this.topNTagRepository.getTopNCountByTag()
                .OrderByDescending(kv => kv.Value.count)
                .Select(kv => kv.Value)
                .ToList();
        }

    }
}
