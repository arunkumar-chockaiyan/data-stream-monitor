using StreamProducerService.Configuration;
using Newtonsoft.Json;
using WebApi;

namespace StreamProducerService.Service
{
    public class TwitterStreamService : ITwitterStreamService
    {
        private readonly TwitterConfig twitterConfig;
        private readonly ILogger<TwitterStreamService> logger;
        private readonly HttpClient httpClient;
        private readonly MonitorServiceConfig monitorServiceConfig;

        public TwitterStreamService(TwitterConfig twitterConfig, ILogger<TwitterStreamService> logger, HttpClient httpClient, MonitorServiceConfig monitorServiceConfig)
        {
            this.twitterConfig = twitterConfig;
            this.logger = logger;
            this.httpClient = httpClient;
            this.monitorServiceConfig = monitorServiceConfig;
        }

        private HttpRequestMessage GetRequestMessageForSamples()
        {
            string url = $"{twitterConfig.api?.BaseUrl}{twitterConfig.api?.EndPoint?.TweetSampleStream}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", $"Bearer {twitterConfig.api?.Token}");
            return request;
        }

        public void processTweetSamples(CancellationToken stoppingToken)
        {
            var response = httpClient.SendAsync(GetRequestMessageForSamples(),
                    HttpCompletionOption.ResponseHeadersRead)
                .Result;

            var stream = response.Content.ReadAsStreamAsync()
                .Result;

            using (var streamReader = new StreamReader(stream))
            {
                while (!streamReader.EndOfStream && !stoppingToken.IsCancellationRequested)
                {
                    string tweetJson = streamReader.ReadLine() ?? "";
                    if (!String.IsNullOrWhiteSpace(tweetJson))
                    {
                        dynamic tweet = JsonConvert.DeserializeObject<dynamic>(tweetJson);
                        ProcessTweet(tweet?.data.id.Value, tweet?.data.text.ToString()); //TODO: handle invalid ids
                    }
                }
            }
        }

        private void ProcessTweet(string id, string tweet)
        {
            var monitorServiceClient = new MonitorServiceClient(monitorServiceConfig.BaseUrl, httpClient);
            monitorServiceClient.TweetAsync(id, tweet).Wait();
        }
    }
}
