using Simplic.Flow;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.Ftp.Flow
{
    /// <summary>
    /// Node to download a file from an ftp server.
    /// </summary>
    [ActionNodeDefinition(Name = nameof(DownloadFileNode), DisplayName = "Download file form ftp directory", Category = "FTP")]
    public class DownloadFileNode : ActionNode
    {
        private IFtpServerConfigurationService ftpServerService;
        private IFtpService ftpService;
        private Dictionary<string, IFtpService> serviceCache;

        /// <summary>
        /// Initializes a new instance of DownloadFileNode.
        /// </summary>
        public DownloadFileNode()
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
            var file = ftpService.DownloadFile(server, path + filename);
            scope.SetValue(OutPinFile, file);
            runtime.EnqueueNode(OutNodeSuccess, scope);

            return true;
        }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        public override string Name => nameof(DownloadFileNode);

        /// <summary>
        /// Gets the friendly name of the node.
        /// </summary>
        public override string FriendlyName => nameof(DownloadFileNode);

        /// <summary>
        /// Gets or sets the success out node.
        /// </summary>
        [FlowPinDefinition(DisplayName = "Success", Name = "OutNodeSuccess", PinDirection = PinDirection.Out)]
        public ActionNode OutNodeSuccess { get; set; }

        /// <summary>
        /// Gets or sets the in pin for the server name.
        /// </summary>
        [DataPinDefinition(
            Id = "B70916E1-7E06-4D75-9334-5652624313DC",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinServer",
            DisplayName = "Server",
            DataType = typeof(string))]
        public DataPin InPinServer { get; set; }

        /// <summary>
        /// Gets or sets the in pin for the path.
        /// </summary>
        [DataPinDefinition(
            Id = "55A3798F-100B-4F1D-AF7D-CB175C4AE9AB",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinPath",
            DisplayName = "Path",
            DataType = typeof(string))]
        public DataPin InPinPath { get; set; }

        /// <summary>
        /// Gets or sets the in pin for the file name.
        /// </summary>
        [DataPinDefinition(
            Id = "E611D837-F95C-4BFF-BFCE-743654BC2640",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinFileName",
            DisplayName = "Filename",
            DataType = typeof(string))]
        public DataPin InPinFileName { get; set; }

        /// <summary>
        /// Gets or sets the out pin for the file.
        /// </summary>
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
