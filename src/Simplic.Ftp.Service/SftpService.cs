using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Ftp.Service
{
    public class SftpService : IFtpService
    {
        public bool DeleteFile(FtpServerConfiguration serverConfiguration, string filename)
        {
            using (SftpClient sftp = new SftpClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
                sftp.Connect();
                sftp.DeleteFile(filename);
                sftp.Disconnect();
            }
            return true;
        }

        public byte[] DownloadFile(FtpServerConfiguration serverConfiguration, string filename)
        {
            using (SftpClient sftp = new SftpClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
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

        public IList<string> GetDirectoryContent(FtpServerConfiguration serverConfiguration, string directory)
        {
            var filenames = new List<string>();
            using (SftpClient sftp = new SftpClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
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

        public bool RenameFile(FtpServerConfiguration serverConfiguration, string filename, string newFilename)
        {
            using (SftpClient sftp = new SftpClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
                sftp.Connect();
                sftp.RenameFile(filename, newFilename);
                sftp.Disconnect();
            }
            return true;
        }

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
            using (SftpClient sftp = new SftpClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
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
