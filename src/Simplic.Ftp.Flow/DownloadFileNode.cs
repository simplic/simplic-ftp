using Simplic.Flow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Simplic.Ftp.Flow
{
    [ActionNodeDefinition(Name = nameof(DownloadFileNode), DisplayName = "Download file form ftp directory", Category = "FTP")]
    public class DownloadFileNode : ActionNode
    {
        private IFtpServerConfigurationService ftpServerService;
        private IFtpService ftpService;
        private Dictionary<string, IFtpService> serviceCache;

        public DownloadFileNode()
        {
            serviceCache = new Dictionary<string, IFtpService>();
        }

        public override bool Execute(IFlowRuntimeService runtime, DataPinScope scope)
        {
            if (ftpServerService == null)
                ftpServerService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFtpServerConfigurationService>();

            var servername = scope.GetValue<string>(InPinServer);
            var server = ftpServerService.GetByName(servername);
            if (serviceCache.Keys.Contains(server.Type.ToString()))
                ftpService = serviceCache[server.Type.ToString()];
            else
            {
                ftpService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFtpService>(server.Type.ToString());
                serviceCache.Add(server.Type.ToString(), ftpService);
            }
            var filename = scope.GetValue<string>(InPinFileName);
            var path = scope.GetValue<string>(InPinPath);
            var file = ftpService.DownloadFile(server, path + filename );
            scope.SetValue(OutPinFile, file);
            runtime.EnqueueNode(OutNodeSuccess, scope);

            return true;
        }



        public override string Name => nameof(DownloadFileNode);

        public override string FriendlyName => nameof(DownloadFileNode);

        [FlowPinDefinition(DisplayName = "Success", Name = "OutNodeSuccess", PinDirection = PinDirection.Out)]
        public ActionNode OutNodeSuccess { get; set; }

        [DataPinDefinition(
            Id = "B70916E1-7E06-4D75-9334-5652624313DC",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinServer",
            DisplayName = "Server",
            DataType = typeof(string))]
        public DataPin InPinServer { get; set; }

        [DataPinDefinition(
            Id = "55A3798F-100B-4F1D-AF7D-CB175C4AE9AB",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinPath",
            DisplayName = "Path",
            DataType = typeof(string))]
        public DataPin InPinPath{ get; set; }

        [DataPinDefinition(
            Id = "E611D837-F95C-4BFF-BFCE-743654BC2640",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinFileName",
            DisplayName = "Filename",
            DataType = typeof(string))]
        public DataPin InPinFileName { get; set; }

        [DataPinDefinition(
            Id = "14248A85-9D95-41E3-A460-0C6E1B5D397B",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.Out,
            Name = "OutPinFile",
            DisplayName = "File",
            DataType = typeof(byte[]))]
        public DataPin OutPinFile { get; set; }
    }
}
