using Simplic.Framework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simplic.Ftp.UI
{
    public abstract class BaseFtpServerWindow : ApplicationWindow<Guid, FtpServerConfiguration, FtpServerViewModel, IFtpServerConfigurationService>
    {
        public BaseFtpServerWindow(IFtpServerConfigurationService ftpServerService)
            : base(ftpServerService)
        {

        }
    }


    /// <summary>
    /// Interaction logic for FtpServerWindow.xaml
    /// </summary>
    public partial class FtpServerWindow : BaseFtpServerWindow, IFtpServerWindow
    {
        public FtpServerWindow(IFtpServerConfigurationService ftpServerService)
            : base(ftpServerService)
        {
            InitializeComponent();
        }
    }
}
