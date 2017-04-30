﻿using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.MigrationTarget;
using MigAz.UserControls.Migrators;

namespace MigAz.UserControls
{
    public partial class NetworkSecurityGroupProperties : UserControl
    {
        private TreeNode _NetworkSecurityGroupNode;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public NetworkSecurityGroupProperties()
        {
            InitializeComponent();
        }

        internal void Bind(TreeNode networkSecurityGroupNode, AsmToArm asmToArmForm)
        {
            _NetworkSecurityGroupNode = networkSecurityGroupNode;

            NetworkSecurityGroup targetNetworkSecurityGroup = (NetworkSecurityGroup)_NetworkSecurityGroupNode.Tag;
            if (targetNetworkSecurityGroup.Source.GetType() == typeof(Azure.Asm.NetworkSecurityGroup))
            {
                Azure.Asm.NetworkSecurityGroup asmNetworkSecurityGroup = (Azure.Asm.NetworkSecurityGroup)targetNetworkSecurityGroup.Source;
                lblSourceName.Text = asmNetworkSecurityGroup.Name;
            }
            else if (targetNetworkSecurityGroup.Source.GetType() == typeof(Azure.Arm.NetworkSecurityGroup))
            {
                Azure.Arm.NetworkSecurityGroup armNetworkSecurityGroup = (Azure.Arm.NetworkSecurityGroup)targetNetworkSecurityGroup.Source;
                lblSourceName.Text = armNetworkSecurityGroup.Name;
            }

            txtTargetName.Text = targetNetworkSecurityGroup.TargetName;
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;
            NetworkSecurityGroup asmNetworkSecurityGroup = (NetworkSecurityGroup)_NetworkSecurityGroupNode.Tag;

            asmNetworkSecurityGroup.TargetName = txtSender.Text;
            _NetworkSecurityGroupNode.Text = asmNetworkSecurityGroup.ToString();

            PropertyChanged();
        }
    }
}
