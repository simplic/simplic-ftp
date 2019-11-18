using System;
using System.Collections.Generic;

namespace Simplic.Ftp.Service
{
    public class FtpServerService : IFtpServerService
    {

        private IFtpServerRepository ftpServerRepository;
        public FtpServerService(IFtpServerRepository ftpServerRepository)
        {
            this.ftpServerRepository = ftpServerRepository;
        }

        public bool Delete(FtpServer obj)
        {
            return ftpServerRepository.Delete(obj);
        }

        public bool Delete(Guid id)
        {
            return ftpServerRepository.Delete(id);
        }

        public FtpServer Get(Guid id)
        {
            return ftpServerRepository.Get(id);
        }

        public IEnumerable<FtpServer> GetAll()
        {
            return ftpServerRepository.GetAll();
        }

        public FtpServer GetByName(string name)
        {
            return ftpServerRepository.GetByName(name);
        }

        public bool Save(FtpServer obj)
        {
            return ftpServerRepository.Save(obj);
        }
    }
}
