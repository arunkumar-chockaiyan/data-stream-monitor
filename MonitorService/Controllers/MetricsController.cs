using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonitorService.Dto;
using MonitorService.Service;

namespace MonitorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetricsController : ControllerBase
    {
        private readonly ILogger<MetricsController> logger;
        private readonly ITagService tagService;
        private readonly ITweetService tweetService;

        public MetricsController(ILogger<MetricsController> logger, ITagService tagService, ITweetService tweetService)
        {
            this.logger = logger;
            this.tagService = tagService;
            this.tweetService = tweetService;
        }


        [HttpGet("tweetcount")]
        public ActionResult<long> GetTweetCount()
        {
            return tweetService.getTotalCount();
        }

        [HttpGet("toptags")]
        public ActionResult<List<TagDto>> getTopTags()
        {
            return tagService.getTopNTags();
        }
    }
}
