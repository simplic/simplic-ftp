using System;
using System.Collections.Generic;

namespace Simplic.Ftp.Service
{
    /// <summary>
    /// Service implementation to load, save and delete ftp server configurations.
    /// </summary>
    public class FtpServerConfigurationService : IFtpServerConfigurationService
    {

        private IFtpServerConfigurationRepository ftpServerRepository;

        /// <summary>
        /// Initializes a new instance of FtpServerConfigurationService.
        /// </summary>
        /// <param name="ftpServerRepository"></param>
        public FtpServerConfigurationService(IFtpServerConfigurationRepository ftpServerRepository)
        {
            this.ftpServerRepository = ftpServerRepository;
        }

        /// <summary>
        /// Deletes a ftp server configuration.
        /// </summary>
        /// <param name="obj">The ftp server configuration</param>
        /// <returns></returns>
        public bool Delete(FtpServerConfiguration obj)
        {
            return ftpServerRepository.Delete(obj);
        }

        /// <summary>
        /// Deletes a ftp server configuration.
        /// </summary>
        /// <param name="id">The id of an ftp server configuration</param>
        /// <returns></returns>
        public bool Delete(Guid id)
        {
            return ftpServerRepository.Delete(id);
        }

        /// <summary>
        /// Gets an ftp server configuration
        /// </summary>
        /// <param name="id">The id of an ftp server configuration</param>
        /// <returns>An instance of FtpServerConfiguration</returns>
        public FtpServerConfiguration Get(Guid id)
        {
            return ftpServerRepository.Get(id);
        }

        /// <summary>
        /// Gets all active server configurations from a group.
        /// </summary>
        /// <param name="groupName">The group name</param>
        /// <returns>An enumerable of ftp server configurations</returns>
        public IEnumerable<FtpServerConfiguration> GetActiveByGroupName(string groupName)
        {
            return ftpServerRepository.GetActiveByGroupName(groupName);
        }

        /// <summary>
        /// Gets all ftp server configurations
        /// </summary>
        /// <returns>An enumerable of ftp server configurations</returns>
        public IEnumerable<FtpServerConfiguration> GetAll()
        {
            return ftpServerRepository.GetAll();
        }

        /// <summary>
        /// Gets all active ftp server configurations
        /// </summary>
        /// <returns>An enumerable of ftp server configurations</returns>
        public IEnumerable<FtpServerConfiguration> GetAllActive()
        {
            return ftpServerRepository.GetAllActive();
        }

        /// <summary>
        /// Gets a ftp server configuration by its name
        /// </summary>
        /// <param name="name">The name of an ftp server configuration</param>
        /// <returns>An instance of FtpServerConfiguration</returns>
        public FtpServerConfiguration GetByName(string name)
        {
            return ftpServerRepository.GetByName(name);
        }

        /// <summary>
        /// Saves a ftp server configuration
        /// </summary>
        /// <param name="obj">A ftp server configuration</param>
        /// <returns></returns>
        public bool Save(FtpServerConfiguration obj)
        {
            return ftpServerRepository.Save(obj);
        }
    }
}
