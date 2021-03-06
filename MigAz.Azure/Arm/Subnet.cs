// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure.Core.ArmTemplate;
using MigAz.Azure.Core.Interface;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class Subnet : ArmResource, ISubnet, IMigrationSubnet
    {
        private JToken _Subnet;
        private VirtualNetwork _VirtualNetwork;
        private NetworkSecurityGroup _NetworkSecurityGroup;
        private RouteTable _RouteTable;

        private Subnet() : base(null, null) { }

        public Subnet(VirtualNetwork virtualNetwork, JToken subnet) : base(virtualNetwork.AzureSubscription, subnet)
        {
            _VirtualNetwork = virtualNetwork;
            _Subnet = subnet;
        }

        public new async Task InitializeChildrenAsync()
        {
            if (this.NetworkSecurityGroupId != string.Empty)
            {
                _NetworkSecurityGroup = await this.AzureSubscription.GetAzureARMNetworkSecurityGroup(this.NetworkSecurityGroupId);
            }
            if (this.RouteTableId != string.Empty)
            {
                _RouteTable = await this.AzureSubscription.GetAzureARMRouteTable(this.RouteTableId);
            }
        }

        public string TargetId
        {
            get { return this.Id; }
        }

        public string AddressPrefix
        {
            get { return (string)_Subnet["properties"]["addressPrefix"]; }
        }
        private string NetworkSecurityGroupId
        {
            get
            {
                if (_Subnet["properties"]["networkSecurityGroup"] == null)
                    return string.Empty;

                return (string)_Subnet["properties"]["networkSecurityGroup"]["id"];
            }
        }

        private string RouteTableId
        {
            get
            {
                if (_Subnet["properties"]["routeTable"] == null)
                    return string.Empty;

                return (string)_Subnet["properties"]["routeTable"]["id"];
            }
        }

        public VirtualNetwork VirtualNetwork
        {
            get { return _VirtualNetwork; }
        }

        public NetworkSecurityGroup NetworkSecurityGroup
        {
            get
            {
                return _NetworkSecurityGroup;
            }
        }
        public RouteTable RouteTable
        {
            get
            {
                return _RouteTable;
            }
        }

        public bool IsGatewaySubnet
        {
            get { return this.Name == ArmConst.GatewaySubnetName; }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public static bool operator ==(Subnet lhs, Subnet rhs)
        {
            bool status = false;
            if (((object)lhs == null && (object)rhs == null) ||
                    ((object)lhs != null && (object)rhs != null && lhs.Id == rhs.Id))
            {
                status = true;
            }
            return status;
        }

        public static bool operator !=(Subnet lhs, Subnet rhs)
        {
            return !(lhs == rhs);
        }
    }
}

