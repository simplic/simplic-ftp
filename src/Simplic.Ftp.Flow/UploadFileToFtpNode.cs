using Simplic.Flow;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.Ftp.Flow
{
    /// <summary>
    /// Node to upload a file to a ftp server.
    /// </summary>
    [ActionNodeDefinition(Name = nameof(UploadFileToFtpNode), DisplayName = "Upload file to ftp server", Category = "FTP")]
    public class UploadFileToFtpNode : ActionNode
    {
        private IFtpServerConfigurationService ftpServerService;
        private IFtpService ftpService;
        private Dictionary<string, IFtpService> serviceCache;

        /// <summary>
        /// Initializes a new instance of UploadFileToFtpNode.
        /// </summary>
        public UploadFileToFtpNode()
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

            var file = scope.GetValue<byte[]>(InPinFile);
            var path = scope.GetValue<string>(InPinPath);
            var fileName = scope.GetValue<string>(InPinFileName);
            try
            {
                ftpService.UploadFile(server, file, path, fileName);
                runtime.EnqueueNode(OutNodeSuccess, scope);
            }
            catch (Exception ex)
            {
                runtime.EnqueueNode(OutNodeFaield, scope);
                Console.WriteLine(ex);
            }
            return true;
        }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        public override string Name => nameof(UploadFileToFtpNode);

        /// <summary>
        /// Gets the friendly name of the node.
        /// </summary>
        public override string FriendlyName => nameof(UploadFileToFtpNode);

        /// <summary>
        /// Gets or sets the success out node.
        /// </summary>
        [FlowPinDefinition(DisplayName = "Success", Name = nameof(OutNodeSuccess), PinDirection = PinDirection.Out)]
        public ActionNode OutNodeSuccess { get; set; }

        /// <summary>
        /// Gets or sets the failed out node.
        /// </summary>
        [FlowPinDefinition(DisplayName = "Failed", Name = nameof(OutNodeFaield), PinDirection = PinDirection.Out)]
        public ActionNode OutNodeFaield { get; set; }

        /// <summary>
        /// Gets oe sets the in pin for the server name.
        /// </summary>
        [DataPinDefinition(
            Id = "972CE8E3-7FC8-47B4-B3B1-37071A3B35A8",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinServer",
            DisplayName = "Server",
            DataType = typeof(string))]
        public DataPin InPinServer { get; set; }

        /// <summary>
        /// Gets or sets the in pin for the file.
        /// </summary>
        [DataPinDefinition(
            Id = "A0B7A136-BDB4-4FCB-9BC2-E7E30AC599CF",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinFile",
            DisplayName = "File",
            DataType = typeof(byte[]))]
        public DataPin InPinFile { get; set; }

        /// <summary>
        /// Gets or sets the in pin for the file name.
        /// </summary>
        [DataPinDefinition(
            Id = "B0F691AE-E483-411B-ADD2-18E7938F3176",
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
            Id = "0A750D38-446A-4E0C-970A-915D93DF6AD8",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinPath",
            DisplayName = "Path",
            DataType = typeof(string))]
        public DataPin InPinPath { get; set; }
    }
}
