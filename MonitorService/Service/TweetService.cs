using MonitorService.Dto;
using MonitorService.Repository;

namespace MonitorService.Service
{
    public class TweetService : ITweetService
    {
        private readonly ITweetRepository tweetRepository;
        private readonly ITagService tagService;

        public TweetService(ITweetRepository tweetRepository, ITagService tagService)
        {
            this.tweetRepository = tweetRepository;
            this.tagService = tagService;
        }

        public long getTotalCount()
        {
            return tweetRepository.getTotalCount();
        }

        public void SaveTweet(TweetDto tweetDto)
        {
            if (tweetRepository.addTweet(tweetDto))
            {
                tagService.extractAndUpdateTagCount(tweetDto.Text);
            }
        }
    }
}
