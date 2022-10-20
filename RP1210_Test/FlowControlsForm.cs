
// Type: RP1210_Test.FlowControlsForm




using Peak.RP1210C;
using RP1210_Test.HelpClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RP1210_Test
{
  public class FlowControlsForm : Form
  {
    private IContainer components;
    private Button btnCancel;
    private Button btnOK;
    private GroupBox groupBox3;
    private ListView lsvFlowControls;
    private ColumnHeader clhType;
    private ColumnHeader clhIncomingID;
    private ColumnHeader clhExtendedIncomingID;
    private ColumnHeader clhOutgoingID;
    private ColumnHeader clhExtendedOutgoingID;
    private ColumnHeader clhBlockSize;
    private ColumnHeader clhSeparationTime;
    private ColumnHeader clhSeparationTimeTX;
    private Button btnAdd;
    private Button btnDelete;
    private Button btnClear;

    public FlowControlsForm(RP1210ISO15765FlowControl[] configuredFlows = null)
    {
      this.InitializeComponent();
      this.FlowControls = configuredFlows;
    }

    private void SetButtonsStatus()
    {
      bool flag = this.lsvFlowControls.Items.Count > 0;
      this.btnOK.Enabled = true;
      this.btnAdd.Enabled = true;
      this.btnClear.Enabled = flag;
      this.btnDelete.Enabled = flag;
    }

    private void AddFlowControlItem(RP1210ISO15765FlowControl flow)
    {
      ListViewItem listViewItem = new ListViewItem(flow.MsgType.ToString());
      listViewItem.Tag = (object) flow;
      int num1 = flow.MsgType == RP1210CISO15765MsgType.EXTENDED_CAN || flow.MsgType == RP1210CISO15765MsgType.EXTENDED_CAN_ISO15765_EXTENDED ? 8 : 4;
      ListViewItem.ListViewSubItemCollection subItems1 = listViewItem.SubItems;
      uint num2 = flow.IncomingCanID;
      string text1 = "0x" + num2.ToString(string.Format("X{0}", (object) num1));
      subItems1.Add(text1);
      ListViewItem.ListViewSubItemCollection subItems2 = listViewItem.SubItems;
      byte num3 = flow.IncomingExtendedCanID;
      string text2 = "0x" + num3.ToString("X");
      subItems2.Add(text2);
      ListViewItem.ListViewSubItemCollection subItems3 = listViewItem.SubItems;
      num2 = flow.OutgoingCanID;
      string text3 = "0x" + num2.ToString(string.Format("X{0}", (object) num1));
      subItems3.Add(text3);
      ListViewItem.ListViewSubItemCollection subItems4 = listViewItem.SubItems;
      num3 = flow.OutgoingExtendedCanID;
      string text4 = "0x" + num3.ToString("X");
      subItems4.Add(text4);
      ListViewItem.ListViewSubItemCollection subItems5 = listViewItem.SubItems;
      num3 = flow.BlockSize;
      string text5 = num3.ToString();
      subItems5.Add(text5);
      if (flow.SeparationTime <= (byte) 127)
        listViewItem.SubItems.Add(string.Format("{0} ms", (object) flow.SeparationTime));
      else if (flow.SeparationTime >= (byte) 128 && flow.SeparationTime <= (byte) 249)
      {
        listViewItem.SubItems.Add(string.Format("{0} µs", (object) (((int) flow.SeparationTime & 15) * 100)));
      }
      else
      {
        ListViewItem.ListViewSubItemCollection subItems6 = listViewItem.SubItems;
        num3 = flow.SeparationTime;
        string text6 = "Wrong value -> 0x" + num3.ToString("X");
        subItems6.Add(text6);
      }
      if (flow.SeparationTimeTx <= (ushort) sbyte.MaxValue)
        listViewItem.SubItems.Add(string.Format("{0} ms", (object) flow.SeparationTimeTx));
      else if (flow.SeparationTimeTx >= (ushort) 128 && flow.SeparationTimeTx <= (ushort) 249)
        listViewItem.SubItems.Add(string.Format("{0} µs", (object) (((int) flow.SeparationTimeTx & 15) * 100)));
      else if (flow.SeparationTimeTx == ushort.MaxValue)
      {
        listViewItem.SubItems.Add("Use vehicle value");
      }
      else
      {
        ListViewItem.ListViewSubItemCollection subItems6 = listViewItem.SubItems;
        num3 = flow.SeparationTime;
        string text6 = "Wrong value -> 0x" + num3.ToString("X");
        subItems6.Add(text6);
      }
      this.lsvFlowControls.Items.Add(listViewItem);
      this.lsvFlowControls.SelectedIndices.Clear();
      listViewItem.Selected = true;
    }

    private bool FlowControlAlreadyExists(RP1210ISO15765FlowControl current)
    {
      foreach (ListViewItem listViewItem in this.lsvFlowControls.Items)
      {
        if (listViewItem.Tag as RP1210ISO15765FlowControl == current)
        {
          this.lsvFlowControls.SelectedItems.Clear();
          listViewItem.Selected = true;
          return true;
        }
      }
      return false;
    }

    private void ConfigureFlowControls(RP1210ISO15765FlowControl[] flowControls)
    {
      this.lsvFlowControls.Items.Clear();
      if (flowControls != null)
      {
        foreach (RP1210ISO15765FlowControl flowControl in flowControls)
          this.AddFlowControlItem(flowControl);
      }
      if (this.lsvFlowControls.Items.Count <= 0)
        return;
      this.lsvFlowControls.Items[0].Selected = true;
    }

    private RP1210ISO15765FlowControl[] CompileFlowControls()
    {
      List<RP1210ISO15765FlowControl> o15765FlowControlList = new List<RP1210ISO15765FlowControl>();
      foreach (ListViewItem listViewItem in this.lsvFlowControls.Items)
        o15765FlowControlList.Add(listViewItem.Tag as RP1210ISO15765FlowControl);
      return o15765FlowControlList.ToArray();
    }

    private void FlowControlsForm_Load(object sender, EventArgs e)
    {
      this.SetButtonsStatus();
      this.btnOK.Enabled = false;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      AddFlowControlForm addFlowControlForm = new AddFlowControlForm();
      if (addFlowControlForm.ShowDialog() != DialogResult.OK)
        return;
      RP1210ISO15765FlowControl o15765FlowControl = new RP1210ISO15765FlowControl(addFlowControlForm.MessageType, addFlowControlForm.IncomingID, addFlowControlForm.IncomingExtendedAddress, addFlowControlForm.OutgoingID, addFlowControlForm.OutgoingExtendedAddress, addFlowControlForm.BlockSize, addFlowControlForm.SeparationTime, addFlowControlForm.SeparationTimeTx);
      if (this.FlowControlAlreadyExists(o15765FlowControl))
      {
        int num = (int) MessageBox.Show("The defined flow control already exists in the list. The entry was marked");
      }
      else
        this.AddFlowControlItem(o15765FlowControl);
      this.SetButtonsStatus();
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.lsvFlowControls.Clear();
      this.SetButtonsStatus();
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      int num = -1;
      while (this.lsvFlowControls.SelectedItems.Count > 0)
      {
        num = this.lsvFlowControls.SelectedItems[0].Index;
        this.lsvFlowControls.Items.Remove(this.lsvFlowControls.SelectedItems[0]);
      }
      this.SetButtonsStatus();
      if (this.lsvFlowControls.Items.Count <= 0 || this.lsvFlowControls.SelectedItems.Count != 0 || num < 0)
        return;
      if (num > 0)
        this.lsvFlowControls.Items[num - 1].Selected = true;
      else
        this.lsvFlowControls.Items[0].Selected = true;
    }

    public RP1210ISO15765FlowControl[] FlowControls
    {
      get => this.CompileFlowControls();
      set => this.ConfigureFlowControls(value);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.groupBox3 = new GroupBox();
      this.lsvFlowControls = new ListView();
      this.clhType = new ColumnHeader();
      this.clhIncomingID = new ColumnHeader();
      this.clhExtendedIncomingID = new ColumnHeader();
      this.clhOutgoingID = new ColumnHeader();
      this.clhExtendedOutgoingID = new ColumnHeader();
      this.clhBlockSize = new ColumnHeader();
      this.clhSeparationTime = new ColumnHeader();
      this.clhSeparationTimeTX = new ColumnHeader();
      this.btnAdd = new Button();
      this.btnDelete = new Button();
      this.btnClear = new Button();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(684, 215);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Enabled = false;
      this.btnOK.Location = new Point(603, 215);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox3.Controls.Add((Control) this.lsvFlowControls);
      this.groupBox3.Location = new Point(12, 12);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(747, 192);
      this.groupBox3.TabIndex = 0;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = " Defined flow controls ";
      this.lsvFlowControls.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lsvFlowControls.Columns.AddRange(new ColumnHeader[8]
      {
        this.clhType,
        this.clhIncomingID,
        this.clhExtendedIncomingID,
        this.clhOutgoingID,
        this.clhExtendedOutgoingID,
        this.clhBlockSize,
        this.clhSeparationTime,
        this.clhSeparationTimeTX
      });
      this.lsvFlowControls.FullRowSelect = true;
      this.lsvFlowControls.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.lsvFlowControls.HideSelection = false;
      this.lsvFlowControls.Location = new Point(8, 19);
      this.lsvFlowControls.Name = "lsvFlowControls";
      this.lsvFlowControls.Size = new Size(732, 163);
      this.lsvFlowControls.TabIndex = 0;
      this.lsvFlowControls.UseCompatibleStateImageBehavior = false;
      this.lsvFlowControls.View = View.Details;
      this.clhType.Text = "Type";
      this.clhType.Width = 100;
      this.clhIncomingID.Text = "Device ID";
      this.clhIncomingID.Width = 80;
      this.clhExtendedIncomingID.Text = "Ext. Addr";
      this.clhOutgoingID.Text = "Tester ID";
      this.clhOutgoingID.Width = 80;
      this.clhExtendedOutgoingID.Text = "Ext. Addr.";
      this.clhBlockSize.Text = "Block Size";
      this.clhBlockSize.Width = 80;
      this.clhSeparationTime.Text = "Separation Time (Rx)";
      this.clhSeparationTime.Width = 120;
      this.clhSeparationTimeTX.Text = "Separation Time (Tx)";
      this.clhSeparationTimeTX.Width = 120;
      this.btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnAdd.Location = new Point(12, 215);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 23);
      this.btnAdd.TabIndex = 1;
      this.btnAdd.Text = "Add ...";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnDelete.Location = new Point(93, 215);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(75, 23);
      this.btnDelete.TabIndex = 2;
      this.btnDelete.Text = "Delete Selected";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnClear.Location = new Point(174, 215);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(75, 23);
      this.btnClear.TabIndex = 3;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(771, 250);
      this.Controls.Add((Control) this.btnClear);
      this.Controls.Add((Control) this.btnDelete);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.groupBox3);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FlowControlsForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ISO15765 Flow Controls";
      this.Load += new EventHandler(this.FlowControlsForm_Load);
      this.groupBox3.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
