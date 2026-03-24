using Neo4j.Driver;

namespace FlexFit.Infrastructure.Data
{
    public class Neo4jContext : IDisposable
    {
        private readonly IDriver _driver;

        public Neo4jContext(IConfiguration configuration)
        {
            var uri = configuration["Neo4jSettings:Uri"];
            var username = configuration["Neo4jSettings:Username"];
            var password = configuration["Neo4jSettings:Password"];

            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password));
        }

        public IAsyncSession GetSession()
        {
            return _driver.AsyncSession();
        }

        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}
