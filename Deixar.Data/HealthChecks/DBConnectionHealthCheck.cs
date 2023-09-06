using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Deixar.Data.HealthChecks
{
    public class DBConnectionHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public DBConnectionHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Check health status for database connection 
        /// </summary>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var connection = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection conn = new SqlConnection(connection);
            try
            {
                await conn.OpenAsync();
                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, ex.Message);
            }
        }
    }
}
