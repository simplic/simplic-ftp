using Simplic.Cache;
using Simplic.Data.Sql;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace Simplic.Ftp.Data.DB
{
    public class FtpServerConfigurationRepository : SqlRepositoryBase<Guid, FtpServerConfiguration>, IFtpServerConfigurationRepository
    {
        private readonly ISqlService sqlService;

        public FtpServerConfigurationRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService)
            : base(sqlService, sqlColumnService, cacheService)
        {
            this.sqlService = sqlService;
        }

        public override string TableName => "IT_Ftp_server_Configuration";

        public override string PrimaryKeyColumn => "Guid";

        public IEnumerable<FtpServerConfiguration> GetActiveByGroupName(string groupName)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<FtpServerConfiguration>($"Select * from {TableName} where Active = 1 and GroupName like :groupName", new { groupName });
            });
        }

        public IEnumerable<FtpServerConfiguration> GetAllActive()
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<FtpServerConfiguration>($"Select * from {TableName} where Active = 1");
            });
        }

        public FtpServerConfiguration GetByName(string name)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<FtpServerConfiguration>($"Select * from {TableName} where InternalName like :name", new { name }).FirstOrDefault();
            });
        }

        public override Guid GetId(FtpServerConfiguration obj)
        {
            return obj.Guid;
        }
    }
}
