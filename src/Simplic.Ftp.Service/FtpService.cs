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
            request.GetResponse();
            return true;
        }

        public byte[] DownloadFile(FtpServerConfiguration serverConfiguration, string filename)
        {
            var request = (FtpWebRequest)WebRequest.Create(serverConfiguration.URI + filename);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.UseBinary = true;

            request.Credentials = new NetworkCredential(serverConfiguration.Username, serverConfiguration.Password);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream reader = request.GetResponse().GetResponseStream();
            
            using (BinaryReader binaryReader = new BinaryReader(reader))
            {
                return binaryReader.ReadBytes((int)reader.Length);
            }
        }

        public IList<string> GetDirectoryContent(FtpServerConfiguration serverConfiguration)
        {
            var request = (FtpWebRequest)WebRequest.Create(serverConfiguration.URI);
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

        public bool UploadFile(FtpServerConfiguration serverConfiguration, byte[] file)
        {
            var request = (FtpWebRequest)WebRequest.Create(serverConfiguration.URI);
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
