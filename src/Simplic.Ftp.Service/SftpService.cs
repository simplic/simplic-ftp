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
                using (var fs = File.Create(Path.GetTempFileName(), 4096, FileOptions.DeleteOnClose))
                {
                    sftp.DownloadFile(filename, fs);
                    using (var br = new BinaryReader(fs))
                    {
                        var bytes = br.ReadBytes((int)fs.Length);
                        sftp.Disconnect();
                        return bytes;
                    }
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
            using (SftpClient sftp = new SftpClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
                sftp.Connect();
                using (var fs = File.Create(Path.GetTempFileName(), 4096, FileOptions.DeleteOnClose))
                {
                    using (var bw = new BinaryWriter(fs))
                    {
                        bw.Write(file);
                    }
                    sftp.UploadFile(fs, path);
                }
                sftp.Disconnect();
            }
            return true;
        }
    }
}
