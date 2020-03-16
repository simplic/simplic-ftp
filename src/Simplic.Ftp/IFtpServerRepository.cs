using Simplic.Data;
using System;
using System.Collections.Generic;

namespace Simplic.Ftp
{
    public interface IFtpServerConfigurationRepository : IRepositoryBase<Guid,FtpServerConfiguration>
    {
        FtpServerConfiguration GetByName(string name);

        IEnumerable<FtpServerConfiguration> GetActiveByGroupName(string groupName);

        IEnumerable<FtpServerConfiguration> GetAllActive();
    }
}
