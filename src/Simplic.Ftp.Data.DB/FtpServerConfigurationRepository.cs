using Simplic.Cache;
using Simplic.Data.Sql;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace Simplic.Ftp.Data.DB
{
    /// <summary>
    ///  Repository implementation to load, save and delete ftp server configurations.
    /// </summary>
    public class FtpServerConfigurationRepository : SqlRepositoryBase<Guid, FtpServerConfiguration>, IFtpServerConfigurationRepository
    {
        private readonly ISqlService sqlService;

        /// <summary>
        /// Initializes a new instance of FtpServerConfigurationRepository.
        /// </summary>
        /// <param name="sqlService"></param>
        /// <param name="sqlColumnService"></param>
        /// <param name="cacheService"></param>
        public FtpServerConfigurationRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService)
            : base(sqlService, sqlColumnService, cacheService)
        {
            this.sqlService = sqlService;
        }

        /// <summary>
        /// Gets the table name.
        /// </summary>
        public override string TableName => "IT_Ftp_server_Configuration";

        /// <summary>
        /// Gets the primary key column name.
        /// </summary>
        public override string PrimaryKeyColumn => "Guid";

        /// <summary>
        /// Gets all active ftp server configurations from a group by its group name. 
        /// </summary>
        /// <param name="groupName">The group name</param>
        /// <returns>An enumerable of ftp server confifgurations</returns>
        public IEnumerable<FtpServerConfiguration> GetActiveByGroupName(string groupName)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<FtpServerConfiguration>($"Select * from {TableName} where Active = 1 and GroupName like :groupName", new { groupName });
            });
        }

        /// <summary>
        /// Gets all active ftp server configurations.
        /// </summary>
        /// <returns>An enumerable of ftp server configurations</returns>
        public IEnumerable<FtpServerConfiguration> GetAllActive()
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<FtpServerConfiguration>($"Select * from {TableName} where Active = 1");
            });
        }

        /// <summary>
        /// Gets a ftp server configuration by its name.
        /// </summary>
        /// <param name="name">The name of an ftp server configuration</param>
        /// <returns></returns>
        public FtpServerConfiguration GetByName(string name)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<FtpServerConfiguration>($"Select * from {TableName} where InternalName like :name", new { name }).FirstOrDefault();
            });
        }

        /// <summary>
        /// Gets the id of the given ftp server configuration.
        /// </summary>
        /// <param name="obj">The ftp server configuration</param>
        /// <returns></returns>
        public override Guid GetId(FtpServerConfiguration obj)
        {
            return obj.Guid;
        }
    }
}
