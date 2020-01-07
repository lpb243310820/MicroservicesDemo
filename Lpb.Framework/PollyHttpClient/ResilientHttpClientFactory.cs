using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Net.Http;

namespace PollyHttpClient
{
    public class ResilientHttpClientFactory
    {
        private readonly ILogger<ResilientHttpClient> _logger;
        private readonly int _retryCount;
        private readonly int _exceptionsAllowedBeforeBreaking;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResilientHttpClientFactory(ILogger<ResilientHttpClient> logger, IHttpContextAccessor httpContextAccessor, int exceptionsAllowedBeforeBreaking = 5, int retryCount = 6)
        {
            _logger = logger;
            _exceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
            _retryCount = retryCount;
            _httpContextAccessor = httpContextAccessor;
        }


        public ResilientHttpClient CreateResilientHttpClient() => new ResilientHttpClient((origin) => CreatePolicies(), _logger, _httpContextAccessor);

        private AsyncPolicy[] CreatePolicies()
        {
            return new AsyncPolicy[]
               {
                    Policy.Handle<HttpRequestException>()
                    .WaitAndRetryAsync(
                        // number of retries
                        _retryCount,
                        // exponential backofff
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        // on retry
                        (exception, timeSpan, retryCount, context) =>
                        {
                            var msg = $"在重试 {retryCount} 次后 " +
                                $"of {context.PolicyKey} " +
                                $"at {context.OperationKey}, " +
                                $"due to: {exception}.";
                            _logger.LogWarning(msg);
                            _logger.LogDebug(msg);
                        }),
                    Policy.Handle<HttpRequestException>()
                    .CircuitBreakerAsync( 
                           // number of exceptions before breaking circuit
                           _exceptionsAllowedBeforeBreaking,
                           // time circuit opened before retry
                           TimeSpan.FromMinutes(1),
                           (exception, duration) =>
                           {
                                // on circuit opened
                                _logger.LogTrace("Circuit breaker opened");
                           },
                           () =>
                           {
                                // on circuit closed
                                _logger.LogTrace("Circuit breaker reset");
                           })
               };
        }
    }
}