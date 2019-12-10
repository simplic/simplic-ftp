using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Ftp.Service
{
    public class FtpService : IFtpService
    {
        public bool DeleteFile(FtpServerConfiguration serverConfiguration, string filename)
        {
            var request = (FtpWebRequest)WebRequest.Create(serverConfiguration.URI + filename);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(serverConfiguration.Username, serverConfiguration.Password);
            request.GetResponse();
            return true;
        }

        public byte[] DownloadFile(FtpServerConfiguration serverConfiguration, string filename)
        {
            var request = new WebClient();
            request.Credentials = new NetworkCredential(serverConfiguration.Username, serverConfiguration.Password);
            return request.DownloadData(serverConfiguration.URI + filename);
        }

        public IList<string> GetDirectoryContent(FtpServerConfiguration serverConfiguration, string directory)
        {
            var request = (FtpWebRequest)WebRequest.Create(serverConfiguration.URI + directory);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;


            request.Credentials = new NetworkCredential(serverConfiguration.Username, serverConfiguration.Password);

            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);
            var dir = new List<string>();
            while (reader.Peek() >= 0)
                dir.Add(reader.ReadLine());
            
            return dir;
        }

        public bool RenameFile(FtpServerConfiguration serverConfiguration, string filename, string newFilename)
        {
            var request = (FtpWebRequest)WebRequest.Create(serverConfiguration.URI + filename);
            request.Method = WebRequestMethods.Ftp.Rename;

            request.Credentials = new NetworkCredential(serverConfiguration.Username, serverConfiguration.Password);

            request.RenameTo = newFilename;
            request.GetResponse();

            return true;
        }

        public bool UploadFile(FtpServerConfiguration serverConfiguration, byte[] file, string path, string fileName)
        {
            var request = (FtpWebRequest)WebRequest.Create(serverConfiguration.URI + path + fileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(serverConfiguration.Username, serverConfiguration.Password);
            request.ContentLength = file.Length;
            request.UseBinary = true;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(file, 0, file.Length);
            }

            return true;
        }
    }
}
