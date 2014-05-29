using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate;
using NHibernate.AdoNet;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Connection;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using Pipelines.Infrastructure.Records;
using ReleaseManager.DataAccess;
using ReleaseManager.DataAccess.ReadModels;
using ReleaseManager.Extensibility;
using Environment = ReleaseManager.DataAccess.ReadModels.Environment;

namespace ReleaseManager.Host
{
    public static class NHibernateSessionFactoryBuilder
    {
        public static ISessionFactory PrepareDatabase(IEnumerable<IMappingProvider> mappingProviders)
        {
            var cfg = BuildConfiguration(mappingProviders);
            var sessionFactory = cfg.BuildSessionFactory();
            var schemaExport = new SchemaExport(cfg);
            //schemaExport.Execute(false, true, false);
            return sessionFactory;
        }

        private static Configuration BuildConfiguration(IEnumerable<IMappingProvider> mappingProviders)
        {
            var configure = new Configuration();

            configure.DataBaseIntegration(dbi =>
            {
                ConfigureForSQLServer(dbi);
                dbi.Timeout = 255;
                dbi.Batcher<NonBatchingBatcherFactory>();
            });

            configure.AddDeserializedMapping(GenerateMappings(mappingProviders), null);
            return configure;
        }

        private static void ConfigureForSQLServer(IDbIntegrationConfigurationProperties dbi)
        {
            dbi.ConnectionString = "Data Source=(local);Integrated Security=SSPI;Initial Catalog=ReleaseManager";
            dbi.Dialect<NHibernate.Dialect.MsSql2012Dialect>();
            dbi.Driver<NHibernate.Driver.Sql2008ClientDriver>();
        }

        private static HbmMapping GenerateMappings(IEnumerable<IMappingProvider> mappingProviders)
        {
            var mapper = new ModelMapper();
            mapper.AddMapping<Environment.Mapping>();
            mapper.AddMapping<Project.Mapping>();
            mapper.AddMapping<ReleaseCandidate.Mapping>();
            mapper.AddMapping<TestSuite.Mapping>();
            mapper.AddMapping<EventRecord.Mapping>();
            mapper.AddMapping<CommandRecord.Mapping>();
            mapper.AddMapping<CommandProcessingResultRecord.Mapping>();

            mapper.AddMappings(mappingProviders.SelectMany(x => x.GetMappings()));

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