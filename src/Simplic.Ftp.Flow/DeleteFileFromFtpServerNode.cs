using Simplic.Flow;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.Ftp.Flow
{
    /// <summary>
    /// Node to delete a file from an ftp server.
    /// </summary>
    [ActionNodeDefinition(Name = nameof(DeleteFileFromFtpServerNode), DisplayName = "Delete file form ftp directory", Category = "FTP")]
    public class DeleteFileFromFtpServerNode : ActionNode
    {
        private IFtpServerConfigurationService ftpServerService;
        private IFtpService ftpService;
        private Dictionary<string, IFtpService> serviceCache;

        /// <summary>
        /// Initializes a new instance fo DeleteFileFromFtpServerNode.
        /// </summary>
        public DeleteFileFromFtpServerNode()
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
            var path = scope.GetValue<string>(InPinPath);

            ftpService.DeleteFile(server, path + filename);
            runtime.EnqueueNode(OutNodeSuccess, scope);

            return true;
        }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        public override string Name => nameof(DeleteFileFromFtpServerNode);

        /// <summary>
        /// Gets the friendly name of the node.
        /// </summary>
        public override string FriendlyName => nameof(DeleteFileFromFtpServerNode);

        /// <summary>
        /// Gets or sets the success out node.
        /// </summary>
        [FlowPinDefinition(DisplayName = "Success", Name = "OutNodeSuccess", PinDirection = PinDirection.Out)]
        public ActionNode OutNodeSuccess { get; set; }

        /// <summary>
        /// Gets or sets the in oin for the server name.
        /// </summary>
        [DataPinDefinition(
            Id = "95DE2D7B-E472-4616-B631-52A2FA4C9400",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = nameof(InPinServer),
            DisplayName = "Server",
            DataType = typeof(string))]
        public DataPin InPinServer { get; set; }

        /// <summary>
        /// Gets or sets the in pin for the file name.
        /// </summary>
        [DataPinDefinition(
            Id = "7DA5F9D0-3875-4856-9B47-6B283DA73333",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = nameof(InPinFileName),
            DisplayName = "Filename",
            DataType = typeof(string))]
        public DataPin InPinFileName { get; set; }

        /// <summary>
        /// Gets or sets the in pin for the path.
        /// </summary>
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
