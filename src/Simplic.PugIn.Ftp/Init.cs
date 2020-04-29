using Simplic.Flow;
using Simplic.Framework.Base;
using Simplic.Framework.PlugIn;
using Simplic.Ftp;
using Simplic.Ftp.Data.DB;
using Simplic.Ftp.Flow;
using Simplic.Ftp.Service;
using Simplic.Ftp.UI;
using System;
using Unity;

namespace Simplic.PlugIn.Ftp
{
    public class InitFramework : IFrameworkEntryPoint
    {
        public Type[] DependingEntryPoints()
        {
            return null;
        }

        public void Initilize()
        {
            var container = CommonServiceLocator.ServiceLocator.Current.GetInstance<IUnityContainer>();

            container.RegisterType<IFtpServerConfigurationRepository, FtpServerConfigurationRepository>();
            container.RegisterType<IFtpServerConfigurationService, FtpServerConfigurationService>();

            container.RegisterType<IFtpService, FtpService>("Ftp");
            container.RegisterType<IFtpService, SftpService>("Sftp");
            container.RegisterType<IFtpServerWindow, FtpServerWindow>();

            container.RegisterType<INodeResolver, GenericNodeResolver<DeleteFileFromFtpServerNode>>(nameof(DeleteFileFromFtpServerNode));
            container.RegisterType<INodeResolver, GenericNodeResolver<DownloadFileNode>>(nameof(DownloadFileNode));
            container.RegisterType<INodeResolver, GenericNodeResolver<GetActiveServerNode>>(nameof(GetActiveServerNode));
            container.RegisterType<INodeResolver, GenericNodeResolver<GetAllFilesFromFtpDirectoryNode>>(nameof(GetAllFilesFromFtpDirectoryNode));
            container.RegisterType<INodeResolver, GenericNodeResolver<RenameFileNode>>(nameof(RenameFileNode));
            container.RegisterType<INodeResolver, GenericNodeResolver<UploadFileToFtpNode>>(nameof(UploadFileToFtpNode));

        }
    }


    /// <summary>
    /// Root PlugIn class
    /// </summary>
    [PlugInDesc("Simplic Ftp", "1.0.20.429", "D02206F4-26CD-4026-8B4A-81BFF978A0C2")]
    public class Init
    {
        /// <summary>
        /// Initialize the current plugin
        /// </summary>
        public static void Initialize()
        {
            CheckLicense();
        }

        /// <summary>
        /// Check if an valid license exists
        /// </summary>
        private static void CheckLicense()
        {

        }
    }
}
