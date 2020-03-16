using System;
using System.Collections.Generic;

namespace Simplic.Ftp.Service
{
    public class FtpServerConfigurationService : IFtpServerConfigurationService
    {

        private IFtpServerConfigurationRepository ftpServerRepository;
        public FtpServerConfigurationService(IFtpServerConfigurationRepository ftpServerRepository)
        {
            this.ftpServerRepository = ftpServerRepository;
        }

        public bool Delete(FtpServerConfiguration obj)
        {
            return ftpServerRepository.Delete(obj);
        }

        public bool Delete(Guid id)
        {
            return ftpServerRepository.Delete(id);
        }

        public FtpServerConfiguration Get(Guid id)
        {
            return ftpServerRepository.Get(id);
        }

        public IEnumerable<FtpServerConfiguration> GetActiveByGroupName(string groupName)
        {
            return ftpServerRepository.GetActiveByGroupName(groupName);
        }

        public IEnumerable<FtpServerConfiguration> GetAll()
        {
            return ftpServerRepository.GetAll();
        }

        public IEnumerable<FtpServerConfiguration> GetAllActive()
        {
            return ftpServerRepository.GetAllActive();
        }

        public FtpServerConfiguration GetByName(string name)
        {
            return ftpServerRepository.GetByName(name);
        }

        public bool Save(FtpServerConfiguration obj)
        {
            return ftpServerRepository.Save(obj);
        }
    }
}
