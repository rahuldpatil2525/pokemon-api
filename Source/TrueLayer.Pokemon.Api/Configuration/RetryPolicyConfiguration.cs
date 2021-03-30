namespace TrueLayer.Pokemon.Api.Configuration
{
    public interface IRetryPolicyConfiguration
    {
         int RetryAttempts { get; set; }
         int RetryInterval { get; set; }
    }
    public class RetryPolicyConfiguration : IRetryPolicyConfiguration
    {
        public int RetryAttempts { get; set; }
        public int RetryInterval { get; set; }
    }
}
