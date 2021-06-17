using System;

namespace Simplic.Ftp
{
    /// <summary>
    /// Class to represent a ftp server configuration
    /// </summary>
    public class FtpServerConfiguration
    {
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the internal name.
        /// </summary>
        public string InternalName { get; set; }

        /// <summary>
        /// Gets or sets the uri.
        /// </summary>
        public string URI { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets wherther the configuration is active. 
        /// This is not the property for the active/passive flag on the server.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the group name
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets wherther the connection uses the passive ftp feature
        /// </summary>
        public bool UsePassive { get; set; } = true;

        /// <summary>
        /// Gets or sets the ftp server configuration type (Ftp/Sftp)
        /// </summary>
        public FtpServerConfigurationType Type { get; set; }

        /// <summary>
        /// Gets or sets the timeoft seconds
        /// </summary>
        public int Timeout { get; set; }
    }
}
