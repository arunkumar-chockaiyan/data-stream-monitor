using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonitorService.Service;

namespace MonitorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        private readonly ILogger<TweetController> logger;
        private readonly ITweetService tweetService;

        public TweetController(ILogger<TweetController> logger, ITweetService tweetService)
        {
            this.logger = logger;
            this.tweetService = tweetService;
        }

        [HttpPut("{tweetId}")]
        public IActionResult ReceiveTweet(string tweetId, [FromBody] string tweet)
        {
            try
            {
                tweetService.SaveTweet(new Dto.TweetDto(tweetId, tweet));
                logger.LogInformation($"Id-{tweetId}: '{tweet}'");
                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error on saving tweet");
                return Problem(e.Message, statusCode: 500); //TODO: get traceId and log
            }

        }
    }
}
