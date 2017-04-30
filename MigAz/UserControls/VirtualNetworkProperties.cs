﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.Asm;

namespace MigAz.UserControls
{
    public partial class VirtualNetworkProperties : UserControl
    {
        TreeNode _ARMVirtualNetowrkNode;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public VirtualNetworkProperties()
        {
            InitializeComponent();
        }

        public void Bind(TreeNode armVirtualNetworkNode)
        {
            _ARMVirtualNetowrkNode = armVirtualNetworkNode;

            Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)_ARMVirtualNetowrkNode.Tag;

            if (targetVirtualNetwork.Source.GetType() == typeof(Azure.Asm.VirtualNetwork))
            {
                Azure.Asm.VirtualNetwork asmVirtualNetwork = (Azure.Asm.VirtualNetwork)targetVirtualNetwork.Source;
                lblVNetName.Text = asmVirtualNetwork.Name;
            }
            else if (targetVirtualNetwork.Source.GetType() == typeof(Azure.Arm.VirtualNetwork))
            {
                Azure.Arm.VirtualNetwork armVirtualNetwork = (Azure.Arm.VirtualNetwork)targetVirtualNetwork.Source;
                lblVNetName.Text = armVirtualNetwork.Name;
            }

            txtVirtualNetworkName.Text = targetVirtualNetwork.TargetName;
            dgvAddressSpaces.DataSource = targetVirtualNetwork.AddressPrefixes.Select(x => new { AddressPrefix = x }).ToList();
        }

        private void txtVirtualNetworkName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)_ARMVirtualNetowrkNode.Tag;

            targetVirtualNetwork.TargetName = txtSender.Text.Trim();
            _ARMVirtualNetowrkNode.Text = targetVirtualNetwork.ToString();

            PropertyChanged();
        }

        private void txtVirtualNetworkName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
