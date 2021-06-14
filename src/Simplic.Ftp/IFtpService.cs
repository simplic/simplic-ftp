using System.Collections.Generic;

namespace Simplic.Ftp
{
    /// <summary>
    /// A service to interact with ftp server files
    /// </summary>
    public interface IFtpService
    {
        /// <summary>
        /// Gets the directory content of an ftp server.
        /// </summary>
        /// <param name="serverConfiguration">A ftp server configuration</param>
        /// <param name="directory">directory name</param>
        /// <returns>A list of file names</returns>
        IList<string> GetDirectoryContent(FtpServerConfiguration serverConfiguration, string directory);

        /// <summary>
        /// Downloads a file from an ftp server.
        /// </summary>
        /// <param name="serverConfiguration">A frp server configuration</param>
        /// <param name="filename">The filename</param>
        /// <returns></returns>
        byte[] DownloadFile(FtpServerConfiguration serverConfiguration, string filename);

        /// <summary>
        /// Uploads a file to an ftp server.
        /// </summary>
        /// <param name="serverConfiguration">The ftp server configuration</param>
        /// <param name="file">The file</param>
        /// <param name="path">The path</param>
        /// <param name="fileName">The filename</param>
        /// <returns></returns>
        bool UploadFile(FtpServerConfiguration serverConfiguration, byte[] file, string path, string fileName);

        /// <summary>
        /// Deletes a file from an ftp server.
        /// </summary>
        /// <param name="serverConfiguration">The ftp server configuration</param>
        /// <param name="filename">The filename</param>
        /// <returns></returns>
        bool DeleteFile(FtpServerConfiguration serverConfiguration, string filename);

        /// <summary>
        /// Renames a file on an ftp server.
        /// </summary>
        /// <param name="serverConfiguration">The ftp server configuration</param>
        /// <param name="filename">The filename</param>
        /// <param name="newFilename">The new filename</param>
        /// <returns></returns>
        bool RenameFile(FtpServerConfiguration serverConfiguration, string filename, string newFilename);

    }
}
