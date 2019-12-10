using System.Collections.Generic;

namespace Simplic.Ftp
{
    public interface IFtpService
    {
        IList<string> GetDirectoryContent(FtpServerConfiguration serverConfiguration, string directory);

        byte[] DownloadFile(FtpServerConfiguration serverConfiguration, string filename);

        bool UploadFile(FtpServerConfiguration serverConfiguration, byte[] file, string path, string fileName);

        bool DeleteFile(FtpServerConfiguration serverConfiguration, string filename);

        bool RenameFile(FtpServerConfiguration serverConfiguration, string filename, string newFilename);

    }
}
