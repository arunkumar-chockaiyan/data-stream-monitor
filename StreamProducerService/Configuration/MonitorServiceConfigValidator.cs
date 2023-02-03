using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamProducerService.Configuration
{
    internal class MonitorServiceConfigValidator : IValidateOptions<MonitorServiceConfig>
    {
        public ValidateOptionsResult Validate(string? name, MonitorServiceConfig options)
        {
            if (options == null)
            {
                return ValidateOptionsResult.Fail("MonitorService Configuration is missing. Update appSettings.json");
            }

            if (ValidatorHelper.isConfigMissing(options.BaseUrl))
            {
                return ValidatorHelper.MissingConfigResult(GetSettingsName(nameof(options.BaseUrl)));
            }

            return ValidateOptionsResult.Success;
        }

        private static string GetSettingsName(string configName)
        {
            return $"TwitterConfig: {configName}";
        }

    }
}
