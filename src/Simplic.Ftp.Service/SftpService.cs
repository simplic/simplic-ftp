using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;

namespace Simplic.Ftp.Service
{
    /// <summary>
    /// Service implementation to interact with sftp servers.
    /// </summary>
    public class SftpService : IFtpService
    {
        /// <summary>
        /// Create SftpClient
        /// </summary>
        /// <param name="uri">Uri to parse</param>
        /// <param name="username">Sftp username</param>
        /// <param name="password">Sftp password</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private SftpClient GetClient(string uri, string username, string password)
        {
            if (uri.Contains(":"))
            { 
                var splitted = uri.Split(':');

                var url = splitted[0];

                // TODO: Add check
                if (!int.TryParse(splitted[1], out int port))
                    throw new Exception($"Could not parse port to int: {splitted[1]}");

                return new SftpClient(url, port, username, password);
            }

            return new SftpClient(uri, username, password);
        }

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="serverConfiguration">The ftp server configuration</param>
        /// <param name="filename">The filename</param>
        /// <returns></returns>
        public bool DeleteFile(FtpServerConfiguration serverConfiguration, string filename)
        {
            using (SftpClient sftp = GetClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
                if (serverConfiguration.Timeout != 0)
                    sftp.OperationTimeout = TimeSpan.FromMilliseconds(serverConfiguration.Timeout);
                sftp.Connect();
                sftp.DeleteFile(filename);
                sftp.Disconnect();
            }
            return true;
        }

        /// <summary>
        /// Downloads a file.
        /// </summary>
        /// <param name="serverConfiguration">The ftp server configuration</param>
        /// <param name="filename">The filename</param>
        /// <returns></returns>
        public byte[] DownloadFile(FtpServerConfiguration serverConfiguration, string filename)
        {
            using (SftpClient sftp = GetClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
                if (serverConfiguration.Timeout != 0)
                    sftp.OperationTimeout = TimeSpan.FromMilliseconds(serverConfiguration.Timeout);

                sftp.Connect();
                using (var fs = new MemoryStream())
                {
                    sftp.DownloadFile(filename, fs);
                    fs.Position = 0;
                    var file = fs.ToArray();
                    sftp.Disconnect();
                    return file;
                }
            }
        }

        /// <summary>
        /// Gets the directory content.
        /// </summary>
        /// <param name="serverConfiguration">The ftp server configuration</param>
        /// <param name="directory">The directory</param>
        /// <returns></returns>
        public IList<string> GetDirectoryContent(FtpServerConfiguration serverConfiguration, string directory)
        {
            var filenames = new List<string>();
            using (SftpClient sftp = GetClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
                if (serverConfiguration.Timeout != 0)
                    sftp.OperationTimeout = TimeSpan.FromMilliseconds(serverConfiguration.Timeout);

                sftp.Connect();
                var files = sftp.ListDirectory(directory);
                foreach (var file in files)
                {
                    if (file.IsRegularFile)
                        filenames.Add(file.Name);
                }
                sftp.Disconnect();
            }
            return filenames;
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
            using (SftpClient sftp = GetClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
                if (serverConfiguration.Timeout != 0)
                    sftp.OperationTimeout = TimeSpan.FromMilliseconds(serverConfiguration.Timeout);

                sftp.Connect();
                sftp.RenameFile(filename, newFilename);
                sftp.Disconnect();
            }
            return true;
        }

        /// <summary>
        /// Uploads a file.
        /// </summary>
        /// <param name="serverConfiguration">The ftp server configuration</param>
        /// <param name="file">The file</param>
        /// <param name="path">The path</param>
        /// <param name="fileName">The file name</param>
        /// <returns></returns>
        public bool UploadFile(FtpServerConfiguration serverConfiguration, byte[] file, string path, string fileName)
        {
            if (!path.EndsWith("/"))
                path = path + "/";
            if (!path.StartsWith("/"))
                path = "/" + path;
            if (fileName.StartsWith("/"))
                fileName = fileName.Substring(1, fileName.Length - 1);

            var filepath = path + fileName;

            Console.WriteLine($"Begin SSH.Net upload to {serverConfiguration.URI}{filepath}");
            using (SftpClient sftp = GetClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
                if (serverConfiguration.Timeout != 0)
                    sftp.OperationTimeout = TimeSpan.FromMilliseconds(serverConfiguration.Timeout);

                sftp.Connect();
                using (var stream = new MemoryStream(file))
                {
                    stream.Position = 0;
                    sftp.UploadFile(stream, filepath, true);
                }
                sftp.Disconnect();
            }
            return true;
        }
    }
}
