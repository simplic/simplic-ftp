using Simplic.Cache;
using Simplic.Data.Sql;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Simplic.Ftp.Data.DB
{
    public class FtpServerRepository : SqlRepositoryBase<Guid, FtpServer>, IFtpServerRepository
    {
        private readonly ISqlService sqlService;

        public FtpServerRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService)
            :base(sqlService, sqlColumnService, cacheService)
        {
            this.sqlService = sqlService;
        }

        public override string TableName => "";

        public override string PrimaryKeyColumn => "Guid";

        public FtpServer GetByName(string name)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<FtpServer>($"Select * from {TableName} where InternalName like :name", new { name }).FirstOrDefault();
            });
        }

        public override Guid GetId(FtpServer obj)
        {
            return obj.Guid;
        }
    }
}
