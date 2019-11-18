using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Ftp
{
    public class FtpServer
    {
        public Guid Guid { get; set; }

        public string InternalName { get; set; }

        public string URI { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool Active { get; set; }
    }
}
