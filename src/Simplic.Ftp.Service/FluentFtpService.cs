using FluentFTP;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Simplic.Ftp.Service
{
    /// <summary>
    /// Ftp service for the fluent ftp impllementation
    /// </summary>
    public class FluentFtpService : IFtpService
    {
        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="serverConfiguration">The server configuration</param>
        /// <param name="filename">The file name</param>
        /// <returns></returns>
        public bool DeleteFile(FtpServerConfiguration serverConfiguration, string filename)
        {
            using (var client = new FtpClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
                client.Connect();

                client.DeleteFile(filename);
            }
            return true;
        }

        /// <summary>
        /// Downlaods a file.
        /// </summary>
        /// <param name="serverConfiguration">The ftp server configuration</param>
        /// <param name="filename">The filename</param>
        /// <returns></returns>
        public byte[] DownloadFile(FtpServerConfiguration serverConfiguration, string filename)
        {
            using (var client = new FtpClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
                client.Connect();

                Stream res = new MemoryStream();
                client.Download(res, filename);
                res.Position = 0;

                var streamReader = new BinaryReader(res);

                var text = streamReader.ReadBytes((int)res.Length);

                return text;
            }
        }

        /// <summary>
        /// Gets the directory content.
        /// </summary>
        /// <param name="serverConfiguration">The ftp server configuration.</param>
        /// <param name="directory">The directory</param>
        /// <returns></returns>
        public IList<string> GetDirectoryContent(FtpServerConfiguration serverConfiguration, string directory)
        {
            using (var client = new FtpClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
                client.Connect();

                client.SetWorkingDirectory(directory);
                return client.GetListing().Select(x => x.FullName).ToList();
            }
        }

        /// <summary>
        /// Renames a file.
        /// </summary>
        /// <param name="serverConfiguration">The ftp server configuration</param>
        /// <param name="filename">The filename</param>
        /// <param name="newFilename">The new filename</param>
        /// <returns></returns>
        public bool RenameFile(FtpServerConfiguration serverConfiguration, string filename, string newFilename)
        {
            using (var client = new FtpClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
                client.Connect();

                client.Rename(filename, newFilename);
            }
            return true;
        }

        /// <summary>
        /// Uploads a file.
        /// </summary>
        /// <param name="serverConfiguration">The ftp server configuation</param>
        /// <param name="file">The file</param>
        /// <param name="path">The path</param>
        /// <param name="fileName">The filename</param>
        /// <returns></returns>
        public bool UploadFile(FtpServerConfiguration serverConfiguration, byte[] file, string path, string fileName)
        {
            using (var client = new FtpClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
                client.Connect();
                if (!client.DirectoryExists(path))
                    client.CreateDirectory(path);

                client.SetWorkingDirectory(path);
                client.Upload(file, fileName);
            }
            return true;
        }
    }
}