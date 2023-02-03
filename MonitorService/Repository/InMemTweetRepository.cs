using System.Text.RegularExpressions;
using MonitorService.Dto;

namespace MonitorService.Repository
{
    /**
     * Naive implementation of in-memory store. 
     */
    public class InMemTweetRepository : ITweetRepository
    {
        //Track list of ids to avoid counting duplicates. Not saving rest of the details for simplicity
        private readonly HashSet<string> tweetIds;

        public InMemTweetRepository()
        {
            tweetIds = new HashSet<string>();
        }

        public long getTotalCount()
        {
            return tweetIds.LongCount();
        }

        public bool addTweet(TweetDto tweetDto)
        {
            return tweetIds.Add(tweetDto.Id);
        }

    }
}
