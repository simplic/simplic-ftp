using System.Collections.Generic;

namespace Simplic.Ftp
{
    public interface IFtpService
    {
        IList<string> GetDirectoryContent(FtpServerConfiguration serverConfiguration);

        byte[] DownloadFile(FtpServerConfiguration serverConfiguration, string filename);

        bool UploadFile(FtpServerConfiguration serverConfiguration, byte[] file);

        bool DeleteFile(FtpServerConfiguration serverConfiguration, string filename);

        bool RenameFile(FtpServerConfiguration serverConfiguration, string filename, string newFilename);

    }
}
