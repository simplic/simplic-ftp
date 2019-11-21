using Simplic.Flow;
using System.Collections.Generic;

namespace Simplic.Ftp.Flow
{
    [ActionNodeDefinition(Name = nameof(GetActiveServerNode), DisplayName = "Download file form ftp directory", Category = "FTP")]
    public class GetActiveServerNode : ActionNode
    {
        private IFtpServerConfigurationService ftpServerConfigurationService;

        public override bool Execute(IFlowRuntimeService runtime, DataPinScope scope)
        {
            if (ftpServerConfigurationService == null)
                ftpServerConfigurationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFtpServerConfigurationService>();

            var groupName = scope.GetValue<string>(InPinGroupName);
            var serverNames = new List<string>();
            if(groupName == null || groupName.Trim() == "")
            {
                var servers = ftpServerConfigurationService.GetActiveByGroupName(groupName);
                foreach (var server in servers)
                    serverNames.Add(server.InternalName);
            }
            else
            {
                var servers = ftpServerConfigurationService.GetAllActive();
                foreach (var server in servers)
                    serverNames.Add(server.InternalName);
            }
            scope.SetValue(OutPinServerNames, serverNames);
            runtime.EnqueueNode(OutNodeSuccess, scope);

            return true;
        }

        public override string Name => nameof(GetActiveServerNode);

        public override string FriendlyName => nameof(GetActiveServerNode);

        [FlowPinDefinition(DisplayName = "Success", Name = "OutNodeSuccess", PinDirection = PinDirection.Out)]
        public ActionNode OutNodeSuccess { get; set; }

        [DataPinDefinition(
            Id = "9BEDEFFC-5B54-4EC0-ADE2-4601A64E1D14",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.In,
            Name = "InPinGroupName",
            DisplayName = "GroupName",
            DataType = typeof(string))]
        public DataPin InPinGroupName{ get; set; }

        [DataPinDefinition(
            Id = "122CE224-B7ED-4E3B-9F8F-11038047E545",
            ContainerType = DataPinContainerType.Single,
            Direction = PinDirection.Out,
            Name = "OutPinServerNames",
            DisplayName = "Server names",
            DataType = typeof(List<string>))]
        public DataPin OutPinServerNames { get; set; }
    }
}
