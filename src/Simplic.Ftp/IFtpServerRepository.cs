using Simplic.Data;
using System;

namespace Simplic.Ftp
{
    public interface IFtpServerRepository : IRepositoryBase<Guid,FtpServer>
    {
        FtpServer GetByName(string name);
    }
}
