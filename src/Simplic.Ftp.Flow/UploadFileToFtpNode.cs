using Simplic.Flow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Ftp.Flow
{
    [ActionNodeDefinition(Name = nameof(UploadFileToFtpNode), DisplayName = "Uplad file to ftp server", Category = "FTP")]
    public class UploadFileToFtpNode : ActionNode
    {
        private IFtpServerService ftpServerService;

        public override bool Execute(IFlowRuntimeService runtime, DataPinScope scope)
        {
            if (ftpServerService == null)
                ftpServerService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFtpServerService>();
            var servername = scope.GetValue<string>(InPinServer);
            var server = ftpServerService.GetByName(servername);
            var request = (FtpWebRequest)WebRequest.Create(server.URI);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(server.Username, server.Password);
            var file = scope.GetValue<byte[]>(InPinFile);
            request.ContentLength = file.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(file, 0, file.Length);
            }

            runtime.EnqueueNode(OutNodeSuccess,scope);
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
    }
}
