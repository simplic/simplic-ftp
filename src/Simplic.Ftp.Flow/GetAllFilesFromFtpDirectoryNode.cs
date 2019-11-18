using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Simplic.Flow;
using Simplic.Ftp;

namespace Simplic.Ftp.Flow
{
    /// <summary>
    /// Node to get the directory content from a ftp server
    /// </summary>
    [ActionNodeDefinition(Name = "GetAllFilesFromFtpDirectory", DisplayName = "Get all files form ftp directory", Category = "FTP")]
    public class GetAllFilesFromFtpDirectoryNode : ActionNode
    {
        private IFtpServerService ftpServerService;

        public override string Name => nameof(GetAllFilesFromFtpDirectoryNode);

        public override string FriendlyName => nameof(GetAllFilesFromFtpDirectoryNode);

        public override bool Execute(IFlowRuntimeService runtime, DataPinScope scope)
        {
            if (ftpServerService == null)
                ftpServerService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFtpServerService>();
            var servername = scope.GetValue<string>(InPinServer);
            var server = ftpServerService.GetByName(servername);

            var request = (FtpWebRequest)WebRequest.Create(server.URI);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;


            request.Credentials = new NetworkCredential(server.Username, server.Password);

            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);
            var dir = new List<string>();
            while (reader.Peek() >= 0)
                dir.Add(reader.ReadLine());

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
            Id = "B099730A-5A72-47AD-B7F2-A43A4283A9BA",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.Out,
            Name = "OutPinDirectory",
            DisplayName = "Directory",
            DataType = typeof(List<string>))]
        public DataPin OutPinDirectory { get; set; }
    }
}
