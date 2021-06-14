using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Simplic.Ftp.Service
{
    /// <summary>
    /// Service implementation to interact with ftp servers
    /// </summary>
    public class FtpService : IFtpService
    {
        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="serverConfiguration">The server configuration</param>
        /// <param name="filename">The file name</param>
        /// <returns></returns>
        public bool DeleteFile(FtpServerConfiguration serverConfiguration, string filename)
        {
            if (!serverConfiguration.URI.EndsWith("/") && !filename.StartsWith("/"))
                filename = "/" + filename;

            var request = (FtpWebRequest)WebRequest.Create(serverConfiguration.URI + filename);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(serverConfiguration.Username, serverConfiguration.Password);
            request.UsePassive = serverConfiguration.UsePassive;
            request.GetResponse();
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
            if (!serverConfiguration.URI.EndsWith("/") && !filename.StartsWith("/"))
                filename = "/" + filename;

            var request = (FtpWebRequest)WebRequest.Create(serverConfiguration.URI + filename);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(serverConfiguration.Username, serverConfiguration.Password);
            request.UsePassive = serverConfiguration.UsePassive;

            var stream = request.GetResponse().GetResponseStream();
            var streamReader = new StreamReader(stream);

            return Encoding.UTF8.GetBytes(streamReader.ReadToEnd());
        }

        /// <summary>
        /// Gets the directory content.
        /// </summary>
        /// <param name="serverConfiguration">The ftp server configuration.</param>
        /// <param name="directory">The directory</param>
        /// <returns></returns>
        public IList<string> GetDirectoryContent(FtpServerConfiguration serverConfiguration, string directory)
        {

            if (!serverConfiguration.URI.EndsWith("/") && !directory.StartsWith("/"))
                directory = "/" + directory;

            var request = (FtpWebRequest)WebRequest.Create(serverConfiguration.URI + directory);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.UsePassive = serverConfiguration.UsePassive;
            request.Credentials = new NetworkCredential(serverConfiguration.Username, serverConfiguration.Password);

            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);
            var dir = new List<string>();
            while (reader.Peek() >= 0)
                dir.Add(reader.ReadLine());

            return dir;
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
            var request = (FtpWebRequest)WebRequest.Create(serverConfiguration.URI + filename);
            request.Method = WebRequestMethods.Ftp.Rename;
            request.UsePassive = serverConfiguration.UsePassive;
            request.Credentials = new NetworkCredential(serverConfiguration.Username, serverConfiguration.Password);

            request.RenameTo = newFilename;
            request.GetResponse();

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
            request.UsePassive = serverConfiguration.UsePassive;
            request.UseBinary = true;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(file, 0, file.Length);
            }

            return true;
        }
    }
}
