using Simplic.Data;
using System;
using System.Collections.Generic;

namespace Simplic.Ftp
{
    /// <summary>
    /// Repository to save, load and delete fttp server configurations
    /// </summary>
    public interface IFtpServerConfigurationRepository : IRepositoryBase<Guid, FtpServerConfiguration>
    {
        /// <summary>
        /// Gets an ftp server configuration by its name
        /// </summary>
        /// <param name="name">The name of a configuration</param>
        /// <returns>A a ftp server configuration</returns>
        FtpServerConfiguration GetByName(string name);

        /// <summary>
        /// Gets all active ftp server configurations from a group by the group name
        /// </summary>
        /// <param name="groupName">The name of the group</param>
        /// <returns>An enumerable of ftp server configurations</returns>
        IEnumerable<FtpServerConfiguration> GetActiveByGroupName(string groupName);

        /// <summary>
        /// Gets all active ftp server configurations
        /// </summary>
        /// <returns>An enumerable of ftp server configurations</returns>
        IEnumerable<FtpServerConfiguration> GetAllActive();
    }
}
