using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamProducerService.Configuration
{
    public class TwitterConfig
    {
        public static readonly string SECTION_NAME = "Twitter";
        public Api? api { get; set; }
        public class Api
        {
            public string? Token { get; set; }
            public string? BaseUrl { get; set; }
            public Endpoint? EndPoint { get; set; }
            public class Endpoint
            {
                public string? TweetSampleStream { get; set; }
            }

        }

    }
}
