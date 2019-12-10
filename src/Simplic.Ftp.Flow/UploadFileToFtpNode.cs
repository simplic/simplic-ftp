﻿using Simplic.Flow;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.Ftp.Flow
{
    [ActionNodeDefinition(Name = nameof(UploadFileToFtpNode), DisplayName = "Upload file to ftp server", Category = "FTP")]
    public class UploadFileToFtpNode : ActionNode
    {
        private IFtpServerConfigurationService ftpServerService;
        private IFtpService ftpService;
        private Dictionary<string, IFtpService> serviceCache;

        public UploadFileToFtpNode()
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

            var file = scope.GetValue<byte[]>(InPinFile);
            var path = scope.GetValue<string>(InPinPath);
            var fileName = scope.GetValue<string>(InPinFileName);

            ftpService.UploadFile(server, file, path, fileName);

            runtime.EnqueueNode(OutNodeSuccess, scope);
            return true;
        }

        public override string Name => nameof(UploadFileToFtpNode);

        public override string FriendlyName => nameof(UploadFileToFtpNode);

        [FlowPinDefinition(DisplayName = "Success", Name = "OutNodeSuccess", PinDirection = PinDirection.Out)]
        public ActionNode OutNodeSuccess { get; set; }


        [DataPinDefinition(
            Id = "972CE8E3-7FC8-47B4-B3B1-37071A3B35A8",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinServer",
            DisplayName = "Server",
            DataType = typeof(string))]
        public DataPin InPinServer { get; set; }


        [DataPinDefinition(
            Id = "A0B7A136-BDB4-4FCB-9BC2-E7E30AC599CF",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinFile",
            DisplayName = "File",
            DataType = typeof(byte[]))]
        public DataPin InPinFile { get; set; }

        [DataPinDefinition(
            Id = "B0F691AE-E483-411B-ADD2-18E7938F3176",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinFileName",
            DisplayName = "Filename",
            DataType = typeof(string))]
        public DataPin InPinFileName { get; set; }

        [DataPinDefinition(
            Id = "0A750D38-446A-4E0C-970A-915D93DF6AD8",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinPath",
            DisplayName = "Path",
            DataType = typeof(byte[]))]
        public DataPin InPinPath { get; set; }
    }
}
