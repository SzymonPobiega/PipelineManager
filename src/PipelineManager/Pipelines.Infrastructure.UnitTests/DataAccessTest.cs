using System.Data;
using NHibernate;
using NHibernate.AdoNet;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Connection;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Pipelines.Infrastructure.Records;

namespace Pipelines.Infrastructure.UnitTests
{
    public abstract class DataAccessTest
    {
        protected ISessionFactory SessionFactory;

        [SetUp]
        public void CreateCleanDatabase()
        {
            var cfg = BuildInMemoryConfiguration();
            SessionFactory = cfg.BuildSessionFactory();
            var schemaExport = new SchemaExport(cfg);
            schemaExport.Execute(false, true, false);
        }

        protected Configuration BuildInMemoryConfiguration()
        {
            var configure = new Configuration();

            configure.DataBaseIntegration(dbi =>
            {
                dbi.ConnectionString = "Data Source=:memory:;Version=3;New=True";
                dbi.Dialect<NHibernate.Dialect.SQLiteDialect>();
                dbi.Driver<NHibernate.Driver.SQLite20Driver>();
                dbi.ConnectionProvider<SQLiteConnectionProvider>();
                dbi.Timeout = 255;
                dbi.Batcher<NonBatchingBatcherFactory>();
            });

            configure.AddDeserializedMapping(GenerateMappings(), null);
            return configure;
        }

        public HbmMapping GenerateMappings()
        {
            var mapper = new ModelMapper();
            mapper.AddMapping<EventRecord.Mapping>();
            mapper.AddMapping<CommandRecord.Mapping>();
            mapper.AddMapping<CommandProcessingResultRecord.Mapping>();

            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            return mapping;
        }

        // ReSharper disable once InconsistentNaming

        public class SQLiteConnectionProvider : ConnectionProvider
        {
            private static IDbConnection _connection;

            public override void CloseConnection(IDbConnection conn)
            {
                //NOOP
            }

            public override IDbConnection GetConnection()
            {
                if (_connection == null)
                {
                    _connection = Driver.CreateConnection();
                    _connection.ConnectionString = ConnectionString;
                    _connection.Open();
                }
                return _connection;
            }
        }
    }
}