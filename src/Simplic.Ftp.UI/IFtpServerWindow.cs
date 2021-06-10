using Simplic.Studio.UI;
using System;

namespace Simplic.Ftp.UI
{
    /// <summary>
    /// Window interface for ftp server windows.
    /// </summary>
    public interface  IFtpServerWindow : IWindow<Guid, FtpServerConfiguration, FtpServerViewModel>
    {
    }
}
