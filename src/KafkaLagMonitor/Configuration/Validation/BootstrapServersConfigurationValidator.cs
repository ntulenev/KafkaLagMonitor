using Microsoft.Extensions.Options;

using System.Diagnostics;

namespace KafkaLagMonitor.Configuration.Validation;

/// <summary>
/// Validator for <see cref="BootstrapServersConfiguration"/>.
/// </summary>
#pragma warning disable CA1515 // Consider making public types internal
public class BootstrapServersConfigurationValidator : IValidateOptions<BootstrapServersConfiguration>
#pragma warning restore CA1515 // Consider making public types internal
{
    /// <summary>
    /// Validates <see cref="BootstrapServersConfiguration"/>.
    /// </summary>
    public ValidateOptionsResult Validate(string? name, BootstrapServersConfiguration options)
    {
        Debug.Assert(name is not null);
        Debug.Assert(options is not null);

        if (options.BootstrapServers is null)
        {
            return ValidateOptionsResult.Fail("BootstrapServers section is not set.");
        }

        if (options.BootstrapServers.Count == 0)
        {
            return ValidateOptionsResult.Fail("BootstrapServers section is empty.");
        }

        if (options.BootstrapServers.Any(string.IsNullOrEmpty))
        {
            return ValidateOptionsResult.Fail("BootstrapServers section contains empty string.");
        }

        if (options.BootstrapServers.Any(string.IsNullOrWhiteSpace))
        {
            return ValidateOptionsResult.Fail("BootstrapServers section contains empty string of whitespaces.");
        }

        return ValidateOptionsResult.Success;
    }
}
