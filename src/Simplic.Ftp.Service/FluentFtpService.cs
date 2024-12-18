﻿using FluentFTP;
using Simplic.Log;
using System;
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
            if (serverConfiguration == null)
                throw new ArgumentNullException(nameof(serverConfiguration));

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentOutOfRangeException("Argument filename must not be null or whitespace");

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
            if (serverConfiguration == null)
                throw new ArgumentNullException(nameof(serverConfiguration));

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentOutOfRangeException("Argument filename must not be null or whitespace");

            using (var client = new FtpClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
                client.Connect();

                using (var res = new MemoryStream())
                {
                    if (!client.DownloadStream(res, filename))
                    {
                        LogManagerInstance.Instance.Error($"Could not download file `{filename}` for ftp server {serverConfiguration.InternalName}/{serverConfiguration.URI}");
                        return null;
                    }

                    res.Position = 0;

                    return res.ToArray();
                }
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
            if (serverConfiguration == null)
                throw new ArgumentNullException(nameof(serverConfiguration));

            using (var client = new FtpClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
                client.Connect();

                client.SetWorkingDirectory(directory ?? "");
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
            if (serverConfiguration == null)
                throw new ArgumentNullException(nameof(serverConfiguration));

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentOutOfRangeException("Argument filename must not be null or whitespace");

            if (string.IsNullOrWhiteSpace(newFilename))
                throw new ArgumentOutOfRangeException("Argument newFilename must not be null or whitespace");

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
            if (serverConfiguration == null)
                throw new ArgumentNullException(nameof(serverConfiguration));

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentOutOfRangeException("Argument filename must not be null or whitespace");

            if (file == null)
                throw new ArgumentNullException(nameof(file));

            using (var client = new FtpClient(serverConfiguration.URI, serverConfiguration.Username, serverConfiguration.Password))
            {
                client.Connect();
                if (!client.DirectoryExists(path))
                    client.CreateDirectory(path);

                client.SetWorkingDirectory(path);
                client.UploadBytes(file, fileName);
            }

            return true;
        }
    }
}