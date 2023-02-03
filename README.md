# data-stream-monitor
monitor data stream like tweets with a simple summary of total tweets and top 10 #tags.

StreamProducerService
  - Streams tweet samples from Twitter and update MonitorService
  - ASP.NET worker service
  - Update Twitter API Token in appsettings.json
      - Twitter:Api:Token
  
MonitorService
  - Tracks count of tweets received
  - Tracks count of tweets by #tag
  - Provides top 10 #tags by count
  - ASP.NET webApi - http://localhost:5204/swagger/v1/swagger.json
      - /api/Metrics/tweetcount - total tweets received
      - /api/Metrics/toptags - top 10 tags with respective count
  
