using MonitorService.Dto;

namespace MonitorService.Service
{
    public interface ITweetService
    {
        void SaveTweet(TweetDto tweetDto);
        long getTotalCount();

    }
}
