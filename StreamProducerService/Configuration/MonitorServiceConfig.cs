using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamProducerService.Configuration
{
    public class MonitorServiceConfig
    {
        public static readonly string SECTION_NAME = "MonitorService";

        public string? BaseUrl { get; set; }
    }
}
