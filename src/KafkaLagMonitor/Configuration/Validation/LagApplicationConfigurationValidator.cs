using Microsoft.Extensions.Options;

using System.Diagnostics;


namespace KafkaLagMonitor.Configuration.Validation;

/// <summary>
/// Validator for <see cref="LagApplicationConfiguration"/>.
/// </summary>
internal class LagApplicationConfigurationValidator : IValidateOptions<LagApplicationConfiguration>
{
    /// <summary>
    /// Validates <see cref="LagApplicationConfiguration"/>.
    /// </summary>
    public ValidateOptionsResult Validate(string name, LagApplicationConfiguration options)
    {
        Debug.Assert(name is not null);
        Debug.Assert(options is not null);

        if (options.Timeout == TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail("Timeout could not be zero.");
        }

        if (options.Groups == null)
        {
            return ValidateOptionsResult.Fail("Groups must be set.");
        }

        if (options.Groups is null)
        {
            return ValidateOptionsResult.Fail("Groups section is not set.");
        }

        if (!options.Groups.Any())
        {
            return ValidateOptionsResult.Fail("Groups section is empty.");
        }

        if (options.Groups.Any(x => string.IsNullOrEmpty(x)))
        {
            return ValidateOptionsResult.Fail("Groups section contains empty string.");
        }

        if (options.Groups.Any(x => string.IsNullOrWhiteSpace(x)))
        {
            return ValidateOptionsResult.Fail("Groups section contains empty string of whitespaces.");
        }

        return ValidateOptionsResult.Success;
    }
}

