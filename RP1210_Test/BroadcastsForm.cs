
// Type: RP1210_Test.BroadcastsForm




using Peak.RP1210C;
using RP1210_Test.HelpClasses;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RP1210_Test
{
  public class BroadcastsForm : Form
  {
    public RP1210_Configuration m_Config;
    private IContainer components;
    private Button btnApply;
    private Button btnCancel;
    private Button btnDel;
    private Button btnAdd;
    private GroupBox grbGroup;
    private ListView lsvBroadcast;
    private ColumnHeader clhIndex;
    private ColumnHeader clhInterval;
    private ColumnHeader clhMsg;

    public BroadcastsForm()
      : this(RP1210CProtocol.CAN, new RP1210_Configuration())
    {
    }

    public BroadcastsForm(RP1210CProtocol protocol)
      : this(protocol, new RP1210_Configuration())
    {
    }

    public BroadcastsForm(RP1210CProtocol protocol, RP1210_Configuration configuration)
    {
      this.InitializeComponent();
      this.m_Config = new RP1210_Configuration(configuration);
      this.Protocol = protocol;
    }

    private void BroadcastsForm_Load(object sender, EventArgs e)
    {
      foreach (RP1210_BroadcastMsg broadcastMessage in this.m_Config.BroadcastMessages)
        this.AddNewItem(broadcastMessage);
      this.lsvBroadcast.Sort();
      this.UpdateUI();
    }

    private void AddNewItem(RP1210_BroadcastMsg broadcast)
    {
      ListViewItem listViewItem = this.lsvBroadcast.Items.Add(broadcast.EntryNumber != byte.MaxValue ? broadcast.EntryNumber.ToString() : " ");
      listViewItem.SubItems.Add(broadcast.Interval.ToString());
      listViewItem.SubItems.Add(broadcast.Msg.ToString());
      listViewItem.Tag = (object) broadcast;
    }

    private void UpdateUI()
    {
      this.grbGroup.Text = string.Format("{0} ({1})", (object) this.Protocol, (object) this.m_Config.BroadcastMessagesCount);
      this.btnAdd.Enabled = this.m_Config.BroadcastMessagesCount < RP1210_Configuration.MaximumCountBroadcastMessages;
      this.btnDel.Enabled = (uint) this.m_Config.BroadcastMessagesCount > 0U;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      switch (this.Protocol)
      {
        case RP1210CProtocol.CAN:
          this.AddBroadcastCAN();
          break;
        case RP1210CProtocol.J1939:
          this.AddBroadcastJ1939();
          break;
        case RP1210CProtocol.ISO15765:
          this.AddBroadcastISO15765();
          break;
        default:
          int num = (int) MessageBox.Show("Unknown protocol used!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
      }
    }

    private void AddBroadcastCAN()
    {
      FormTxMsgCan formTxMsgCan = new FormTxMsgCan(true);
      if (formTxMsgCan.ShowDialog() != DialogResult.OK)
        return;
      RP1210_BroadcastMsg rp1210BroadcastMsg = new RP1210_BroadcastMsg((RP1210_SerialisableMsg) formTxMsgCan.Message, formTxMsgCan.Interval, byte.MaxValue);
      if (this.m_Config.AddBroadcastMessage(rp1210BroadcastMsg))
      {
        this.AddNewItem(rp1210BroadcastMsg);
        this.UpdateUI();
      }
      else
      {
        int num = (int) MessageBox.Show("CAN Broadcast message already exists");
      }
    }

    private void AddBroadcastJ1939()
    {
      FormTxMsgJ1939 formTxMsgJ1939 = new FormTxMsgJ1939(true);
      if (formTxMsgJ1939.ShowDialog() != DialogResult.OK)
        return;
      RP1210_BroadcastMsg rp1210BroadcastMsg = new RP1210_BroadcastMsg((RP1210_SerialisableMsg) formTxMsgJ1939.Message, formTxMsgJ1939.Interval, byte.MaxValue);
      if (this.m_Config.AddBroadcastMessage(rp1210BroadcastMsg))
      {
        this.AddNewItem(rp1210BroadcastMsg);
        this.UpdateUI();
      }
      else
      {
        int num = (int) MessageBox.Show("J1939 Broadcast message already exists");
      }
    }

    private void AddBroadcastISO15765()
    {
      FormTxMsgISO15765 formTxMsgIsO15765 = new FormTxMsgISO15765(true);
      if (formTxMsgIsO15765.ShowDialog() != DialogResult.OK)
        return;
      RP1210_BroadcastMsg rp1210BroadcastMsg = new RP1210_BroadcastMsg((RP1210_SerialisableMsg) formTxMsgIsO15765.Message, formTxMsgIsO15765.Interval, byte.MaxValue);
      if (this.m_Config.AddBroadcastMessage(rp1210BroadcastMsg))
      {
        this.AddNewItem(rp1210BroadcastMsg);
        this.UpdateUI();
      }
      else
      {
        int num = (int) MessageBox.Show("ISO15765 Broadcast message already exists");
      }
    }

    private void btnDel_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem selectedItem in this.lsvBroadcast.SelectedItems)
      {
        if (!this.m_Config.RemoveBroadcastMessage(selectedItem.Tag as RP1210_BroadcastMsg))
        {
          int num = (int) MessageBox.Show(string.Format("Error while removing Broadcast-Entry. Selected index {0}", (object) this.lsvBroadcast.SelectedItems[0].Index));
        }
        else
          this.lsvBroadcast.Items.Remove(selectedItem);
      }
      this.UpdateUI();
    }

    public RP1210_Configuration Configuration
    {
      get => this.m_Config;
      set => this.m_Config = value;
    }

    public RP1210CProtocol Protocol { get; set; }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnApply = new Button();
      this.btnCancel = new Button();
      this.btnDel = new Button();
      this.btnAdd = new Button();
      this.grbGroup = new GroupBox();
      this.lsvBroadcast = new ListView();
      this.clhIndex = new ColumnHeader();
      this.clhInterval = new ColumnHeader();
      this.clhMsg = new ColumnHeader();
      this.grbGroup.SuspendLayout();
      this.SuspendLayout();
      this.btnApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnApply.DialogResult = DialogResult.OK;
      this.btnApply.Location = new Point(207, 352);
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = new Size(75, 23);
      this.btnApply.TabIndex = 1;
      this.btnApply.Text = "Apply";
      this.btnApply.UseVisualStyleBackColor = true;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(288, 352);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnDel.Enabled = false;
      this.btnDel.Location = new Point(319, 48);
      this.btnDel.Name = "btnDel";
      this.btnDel.Size = new Size(26, 23);
      this.btnDel.TabIndex = 2;
      this.btnDel.Text = "-";
      this.btnDel.UseVisualStyleBackColor = true;
      this.btnDel.Click += new EventHandler(this.btnDel_Click);
      this.btnAdd.Enabled = false;
      this.btnAdd.Location = new Point(319, 19);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(26, 23);
      this.btnAdd.TabIndex = 1;
      this.btnAdd.Text = "+";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.grbGroup.Controls.Add((Control) this.lsvBroadcast);
      this.grbGroup.Controls.Add((Control) this.btnDel);
      this.grbGroup.Controls.Add((Control) this.btnAdd);
      this.grbGroup.Location = new Point(12, 12);
      this.grbGroup.Name = "grbGroup";
      this.grbGroup.Size = new Size(351, 334);
      this.grbGroup.TabIndex = 0;
      this.grbGroup.TabStop = false;
      this.grbGroup.Text = " Protocol (Count) ";
      this.lsvBroadcast.Columns.AddRange(new ColumnHeader[3]
      {
        this.clhIndex,
        this.clhInterval,
        this.clhMsg
      });
      this.lsvBroadcast.FullRowSelect = true;
      this.lsvBroadcast.HideSelection = false;
      this.lsvBroadcast.Location = new Point(6, 19);
      this.lsvBroadcast.Name = "lsvBroadcast";
      this.lsvBroadcast.Size = new Size(307, 301);
      this.lsvBroadcast.Sorting = SortOrder.Ascending;
      this.lsvBroadcast.TabIndex = 0;
      this.lsvBroadcast.UseCompatibleStateImageBehavior = false;
      this.lsvBroadcast.View = View.Details;
      this.clhIndex.Text = "Index";
      this.clhIndex.Width = 50;
      this.clhInterval.Text = "Interval (ms)";
      this.clhInterval.TextAlign = HorizontalAlignment.Right;
      this.clhInterval.Width = 70;
      this.clhMsg.Text = "Message";
      this.clhMsg.Width = 180;
      this.AcceptButton = (IButtonControl) this.btnApply;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(375, 383);
      this.Controls.Add((Control) this.grbGroup);
      this.Controls.Add((Control) this.btnApply);
      this.Controls.Add((Control) this.btnCancel);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (BroadcastsForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Broadcast Messages Configuration";
      this.Load += new EventHandler(this.BroadcastsForm_Load);
      this.grbGroup.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
