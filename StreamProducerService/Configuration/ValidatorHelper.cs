using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamProducerService.Configuration
{
    public class ValidatorHelper
    {

        public static bool isConfigMissing(String? configValue)
        {
            return String.IsNullOrWhiteSpace(configValue);
        }

        public static ValidateOptionsResult MissingConfigResult(string configName)
        {
            return ValidateOptionsResult.Fail($"Missing configuration - {configName}");
        }
    }
}
