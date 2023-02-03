using MonitorService.Dto;

namespace MonitorService.Repository
{
    public interface ITweetRepository
    {
        bool addTweet(TweetDto tweetDto);
        long getTotalCount();
    }
}
