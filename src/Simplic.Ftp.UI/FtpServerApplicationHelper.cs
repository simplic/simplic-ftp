using Simplic.Framework.DBUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Ftp.UI
{
    public class FtpServerApplicationHelper : GridWindowApplicationHelper<Guid, FtpServerConfiguration, FtpServerViewModel>
    {
        public override string PrimaryKeyColumn => "Guid";

        public override Type WindowInterface => typeof(IFtpServerWindow);
    }
}
