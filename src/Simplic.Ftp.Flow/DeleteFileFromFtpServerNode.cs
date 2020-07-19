using Simplic.Flow;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.Ftp.Flow
{
    [ActionNodeDefinition(Name = nameof(DeleteFileFromFtpServerNode), DisplayName = "Delete file form ftp directory", Category = "FTP")]
    public class DeleteFileFromFtpServerNode : ActionNode
    {
        private IFtpServerConfigurationService ftpServerService;
        private IFtpService ftpService;
        private Dictionary<string, IFtpService> serviceCache;

        public DeleteFileFromFtpServerNode()
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

            ftpService.DeleteFile(server, path + filename);
            runtime.EnqueueNode(OutNodeSuccess, scope);

            return true;
        }

        public override string Name => nameof(DeleteFileFromFtpServerNode);

        public override string FriendlyName => nameof(DeleteFileFromFtpServerNode);

        [FlowPinDefinition(DisplayName = "Success", Name = "OutNodeSuccess", PinDirection = PinDirection.Out)]
        public ActionNode OutNodeSuccess { get; set; }

        [DataPinDefinition(
            Id = "95DE2D7B-E472-4616-B631-52A2FA4C9400",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = nameof(InPinServer),
            DisplayName = "Server",
            DataType = typeof(string))]
        public DataPin InPinServer { get; set; }

        [DataPinDefinition(
            Id = "7DA5F9D0-3875-4856-9B47-6B283DA73333",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = nameof(InPinFileName),
            DisplayName = "Filename",
            DataType = typeof(string))]
        public DataPin InPinFileName { get; set; }

        [DataPinDefinition(
           Id = "6CC16F0F-163A-42F1-B58D-1170160B5ACB",
           ContainerType = DataPinContainerType.Single,
           Direction = PinDirection.In,
           Name = nameof(InPinPath),
           DisplayName = "Path",
           DataType = typeof(string))]
        public DataPin InPinPath { get; set; }

    }
}
