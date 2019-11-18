using Simplic.Framework.UI;
using Simplic.Studio.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Ftp.UI
{
    public class FtpServerViewModel : ExtendableViewModel, IWindowViewModel<FtpServer>
    {
        public void Initialize(FtpServer model)
        {
            Model = model;
        }


        public FtpServer Model { get; set; }

        public string InternalName { get => Model.InternalName; set => PropertySetter(value, newValue => { Model.InternalName = newValue; }); }

        public string URI { get => Model.URI; set => PropertySetter(value, newValue => { Model.URI = newValue; }); }

        public string Username { get => Model.Username; set => PropertySetter(value, newValue => { Model.Username = newValue; }); }

        public string Password { get => Model.Password; set => PropertySetter(value, newValue => { Model.Password = newValue; }); }

        public bool Active { get => Model.Active; set => PropertySetter(value, newValue => { Model.Active = newValue; }); }
    }
}
