using Microsoft.Extensions.Options;

using System.Diagnostics;


namespace KafkaLagMonitor.Configuration.Validation;

/// <summary>
/// Validator for <see cref="LagApplicationConfiguration"/>.
/// </summary>
#pragma warning disable CA1515 // Consider making public types internal
public class LagApplicationConfigurationValidator : IValidateOptions<LagApplicationConfiguration>
#pragma warning restore CA1515 // Consider making public types internal
{
    /// <summary>
    /// Validates <see cref="LagApplicationConfiguration"/>.
    /// </summary>
    public ValidateOptionsResult Validate(string? name, LagApplicationConfiguration options)
    {
        Debug.Assert(name is not null);
        Debug.Assert(options is not null);

        if (options.Timeout == TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail("Timeout could not be zero.");
        }

        if (options.Groups is null)
        {
            return ValidateOptionsResult.Fail("Groups section is not set.");
        }

        if (options.Groups.Count == 0)
        {
            return ValidateOptionsResult.Fail("Groups section is empty.");
        }

        if (options.Groups.Any(string.IsNullOrEmpty))
        {
            return ValidateOptionsResult.Fail("Groups section contains empty string.");
        }

        if (options.Groups.Any(string.IsNullOrWhiteSpace))
        {
            return ValidateOptionsResult.Fail("Groups section contains empty string of whitespaces.");
        }

        return ValidateOptionsResult.Success;
    }
}

