using Simplic.Flow;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.Ftp.Flow
{
    /// <summary>
    /// Node to rename a file on an ftp server.
    /// </summary>
    [ActionNodeDefinition(Name = nameof(RenameFileNode), DisplayName = "Rename file form ftp directory", Category = "FTP")]
    public class RenameFileNode : ActionNode
    {
        private IFtpServerConfigurationService ftpServerService;
        private IFtpService ftpService;
        private Dictionary<string, IFtpService> serviceCache;

        /// <summary>
        /// Initializes a new instance of RenameFileNode.
        /// </summary>
        public RenameFileNode()
        {
            serviceCache = new Dictionary<string, IFtpService>();
        }

        /// <summary>
        /// Executes the node.
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
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
            var newFilename = scope.GetValue<string>(InPinNewFileName);
            var path = scope.GetValue<string>(InPinPath);

            ftpService.RenameFile(server, path + filename, newFilename);
            runtime.EnqueueNode(OutNodeSuccess, scope);

            return true;
        }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        public override string Name => nameof(RenameFileNode);

        /// <summary>
        /// Gets the friendly name of the node
        /// </summary>
        public override string FriendlyName => nameof(RenameFileNode);

        /// <summary>
        /// Gets or sets the success out node.
        /// </summary>
        [FlowPinDefinition(DisplayName = "Success", Name = "OutNodeSuccess", PinDirection = PinDirection.Out)]
        public ActionNode OutNodeSuccess { get; set; }

        /// <summary>
        /// Gets or sets the in pin for the server name
        /// </summary>
        [DataPinDefinition(
            Id = "3A2C50A4-3488-43EB-A047-7D4305E97480",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinServer",
            DisplayName = "Server",
            DataType = typeof(string))]
        public DataPin InPinServer { get; set; }

        /// <summary>
        /// Gets or sets the in pin for the file name.
        /// </summary>
        [DataPinDefinition(
            Id = "15EBF648-30E8-4B8F-92AA-C0CF72EDE140",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinFileName",
            DisplayName = "Filename",
            DataType = typeof(string))]
        public DataPin InPinFileName { get; set; }

        /// <summary>
        /// Gets or sets the in pin for the path.
        /// </summary>
        [DataPinDefinition(
            Id = "B0AA62F0-71EB-4FE0-B957-3118D1F17650",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinPath",
            DisplayName = "Path",
            DataType = typeof(string))]
        public DataPin InPinPath { get; set; }

        /// <summary>
        /// Gets or sets the in pin for the new file name
        /// </summary>
        [DataPinDefinition(
            Id = "78021FBB-D7F4-4A96-B558-7689C246398B",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinNewFileName",
            DisplayName = "New Filename",
            DataType = typeof(string))]
        public DataPin InPinNewFileName { get; set; }




    }
}
