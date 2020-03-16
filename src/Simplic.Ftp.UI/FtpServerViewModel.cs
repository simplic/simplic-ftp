using Simplic.Framework.UI;
using Simplic.Studio.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Ftp.UI
{
    public class FtpServerViewModel : ExtendableViewModel, IWindowViewModel<FtpServerConfiguration>
    {
        public void Initialize(FtpServerConfiguration model)
        {
            Model = model;
        }


        public FtpServerConfiguration Model { get; set; }

        public string InternalName { get => Model.InternalName; set => PropertySetter(value, newValue => { Model.InternalName = newValue; }); }

        public string URI { get => Model.URI; set => PropertySetter(value, newValue => { Model.URI = newValue; }); }

        public string Username { get => Model.Username; set => PropertySetter(value, newValue => { Model.Username = newValue; }); }

        public string Password { get => Model.Password; set => PropertySetter(value, newValue => { Model.Password = newValue; }); }

        public bool Active { get => Model.Active; set => PropertySetter(value, newValue => { Model.Active = newValue; }); }

        public string GroupName { get => Model.GroupName; set => PropertySetter(value, newValue => { Model.GroupName = newValue; }); }

        public FtpServerConfigurationType Type { get => Model.Type; set => PropertySetter(value, newValue => { Model.Type = newValue; }); }

        public Dictionary<FtpServerConfigurationType, string> TypeSource
        {
            get => new Dictionary<FtpServerConfigurationType, string>
            {
                { FtpServerConfigurationType.Ftp, "FTP"},
                {FtpServerConfigurationType.Sftp, "SFTP" }
            };
        }
    }
}
