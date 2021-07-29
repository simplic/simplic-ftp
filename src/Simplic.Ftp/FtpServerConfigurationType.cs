namespace Simplic.Ftp
{
    /// <summary>
    /// A configuration type for ftp server
    /// </summary>
    public enum FtpServerConfigurationType
    {
        /// <summary>
        /// Ftp server type
        /// </summary>
        Ftp = 0,

        /// <summary>
        /// Sftp server type
        /// </summary>
        Sftp = 1,

        /// <summary>
        /// Use fluent ftp connections for this type
        /// </summary>
        FluentFtp = 2
    }
}
