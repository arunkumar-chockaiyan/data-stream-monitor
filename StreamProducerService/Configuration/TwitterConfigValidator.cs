using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamProducerService.Configuration
{
    internal class TwitterConfigValidator : IValidateOptions<TwitterConfig>
    {
        public ValidateOptionsResult Validate(string? name, TwitterConfig options)
        {
            if (options == null) 
            {
                return ValidateOptionsResult.Fail("Twitter Configuration is missing. Update appSettings.json");
            }

            if (ValidatorHelper.isConfigMissing(options.api?.Token))
            {
                return ValidatorHelper.MissingConfigResult(GetSettingsName(nameof(options.api.Token)));
            }

            if (ValidatorHelper.isConfigMissing(options.api?.BaseUrl))
            {
                return ValidatorHelper.MissingConfigResult(GetSettingsName(nameof(options.api.BaseUrl)));
            }

            if (ValidatorHelper.isConfigMissing(options.api?.EndPoint?.TweetSampleStream))
            {
                return ValidatorHelper.MissingConfigResult(GetSettingsName(nameof(options.api.EndPoint.TweetSampleStream)));
            }
            return ValidateOptionsResult.Success;
        }

        private static string GetSettingsName(string configName)
        {
            return $"TwitterConfig: {configName}";
        }
    }
}
