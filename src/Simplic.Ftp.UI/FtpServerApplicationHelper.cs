using Simplic.Framework.DBUI;
using System;

namespace Simplic.Ftp.UI
{
    /// <summary>
    /// Application helper for ftp server configuration related grid functions.
    /// </summary>
    public class FtpServerApplicationHelper : GridWindowApplicationHelper<Guid, FtpServerConfiguration, FtpServerViewModel>
    {
        /// <summary>
        /// Gets the name of the primary key column.
        /// </summary>
        public override string PrimaryKeyColumn => "Guid";

        /// <summary>
        /// Gets the window interface type.
        /// </summary>
        public override Type WindowInterface => typeof(IFtpServerWindow);
    }
}
