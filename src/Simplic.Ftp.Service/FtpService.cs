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
            var request = (FtpWebRequest)WebRequest.Create(serverConfiguration.URI + filename);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(serverConfiguration.Username, serverConfiguration.Password);
            request.UsePassive = false;

            var stream = request.GetResponse().GetResponseStream();
            var streamReader = new StreamReader(stream);

            return Encoding.UTF8.GetBytes(streamReader.ReadToEnd());
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
            if (file == null)
                throw new Exception("");

            var uri = serverConfiguration.URI.Trim();

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                path = path ?? "/";
                fileName = fileName ?? "";
                path = path.Trim();
                fileName = fileName.Trim();

                if (!path.EndsWith("/"))
                    path = path + "/";
                if (!path.StartsWith("/"))
                    path = "/" + path;

                if (fileName.StartsWith("/"))
                    fileName = fileName.Substring(1, fileName.Length - 1);
                
                if (uri.EndsWith("/"))
                    uri = uri.Substring(0, uri.Length - 1);
            }

            var request = (FtpWebRequest)WebRequest.Create(uri + path + fileName);
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
