using Simplic.Framework.UI;
using Simplic.Studio.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Ftp.UI
{
    /// <summary>
    /// Viemodel to represent ftp server configurations.
    /// </summary>
    public class FtpServerViewModel : ExtendableViewModel, IWindowViewModel<FtpServerConfiguration>
    {

        /// <summary>
        /// Initializes the viewmodel.
        /// </summary>
        /// <param name="model"></param>
        public void Initialize(FtpServerConfiguration model)
        {
            Model = model;
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public FtpServerConfiguration Model { get; set; }

        /// <summary>
        /// Gets or sets the internal name.
        /// </summary>
        public string InternalName { get => Model.InternalName; set => PropertySetter(value, newValue => { Model.InternalName = newValue; }); }

        /// <summary>
        /// Gets or sets the uri.
        /// </summary>
        public string URI { get => Model.URI; set => PropertySetter(value, newValue => { Model.URI = newValue; }); }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get => Model.Username; set => PropertySetter(value, newValue => { Model.Username = newValue; }); }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get => Model.Password; set => PropertySetter(value, newValue => { Model.Password = newValue; }); }

        /// <summary>
        /// Gets or sets wherther the configuration is active.
        /// </summary>
        public bool Active { get => Model.Active; set => PropertySetter(value, newValue => { Model.Active = newValue; }); }

        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        public string GroupName { get => Model.GroupName; set => PropertySetter(value, newValue => { Model.GroupName = newValue; }); }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public FtpServerConfigurationType Type { get => Model.Type; set => PropertySetter(value, newValue => { Model.Type = newValue; }); }

        /// <summary>
        /// Gets or sets the type source.
        /// </summary>
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
