using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamProducerService.Service
{
    public interface ITwitterStreamService
    {
        void processTweetSamples(CancellationToken stoppingToken);
    }
}
