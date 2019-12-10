using System.Collections.Generic;
using System.Linq;
using Simplic.Flow;

namespace Simplic.Ftp.Flow
{
    /// <summary>
    /// Node to get the directory content from a ftp server
    /// </summary>
    [ActionNodeDefinition(Name = "GetAllFilesFromFtpDirectoryNode", DisplayName = "Get all files form ftp directory", Category = "FTP")]
    public class GetAllFilesFromFtpDirectoryNode : ActionNode
    {
        private IFtpServerConfigurationService ftpServerService;
        private IFtpService ftpService;
        private Dictionary<string, IFtpService> serviceCache;

        public GetAllFilesFromFtpDirectoryNode()
        {
            serviceCache = new Dictionary<string, IFtpService>();
        }

        public override string Name => nameof(GetAllFilesFromFtpDirectoryNode);

        public override string FriendlyName => nameof(GetAllFilesFromFtpDirectoryNode);

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
            var directory = scope.GetValue<string>(InPinDirectory);
            var dir = ftpService.GetDirectoryContent(server, directory);

            scope.SetValue(OutPinDirectory, dir);
            runtime.EnqueueNode(OutNodeSuccess, scope);

            return true;

        }

        [FlowPinDefinition(DisplayName = "Success", Name = "OutNodeSuccess", PinDirection = PinDirection.Out)]
        public ActionNode OutNodeSuccess { get; set; }

        [DataPinDefinition(
            Id = "45FA0352-8F9D-42EC-9CE1-8FF27CBE5565",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinServer",
            DisplayName = "Server",
            DataType = typeof(string))]
        public DataPin InPinServer { get; set; }

        [DataPinDefinition(
            Id = "202B5F61-4989-4720-A8AF-78BA746750F9",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinDirectory",
            DisplayName = "Directory",
            DataType = typeof(string))]
        public DataPin InPinDirectory { get; set; }

        [DataPinDefinition(
            Id = "B099730A-5A72-47AD-B7F2-A43A4283A9BA",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.Out,
            Name = "OutPinDirectory",
            DisplayName = "Directory",
            DataType = typeof(List<string>))]
        public DataPin OutPinDirectory { get; set; }
    }
}
