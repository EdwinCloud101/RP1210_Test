
// Type: RP1210_Test.ConfigurationForm




using Peak.RP1210C;
using RP1210_Test.HelpClasses;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RP1210_Test
{
  public class ConfigurationForm : Form
  {
    private RP1210_Configuration m_Config;
    private PEAKRP32Plus.ImplementationLoadLevel m_ImplementationLoadLevel;
    private IContainer components;
    private GroupBox groupBox2;
    private Button btnCancel;
    private GroupBox groupBox1;
    private CheckBox chbEnqueueEchoes;
    private CheckBox chbReceiveMsgs;
    private ComboBox cbbFilterStatus;
    private Label label2;
    private RadioButton rdbExclude;
    private RadioButton rdbInclude;
    private Label label1;
    private Button btnDel;
    private Button btnAdd;
    private Label label3;
    private ListBox lsbFilters;
    private Button btnApply;
    private Label laBAM;
    private NumericUpDown nudInterpacketTime;

    public ConfigurationForm()
      : this(RP1210CProtocol.CAN, new RP1210_Configuration(), PEAKRP32Plus.ImplementationLoadLevel.A)
    {
    }

    public ConfigurationForm(RP1210CProtocol protocol)
      : this(protocol, new RP1210_Configuration(), PEAKRP32Plus.ImplementationLoadLevel.A)
    {
    }

    public ConfigurationForm(RP1210CProtocol protocol, RP1210_Configuration configuration)
      : this(protocol, configuration, PEAKRP32Plus.ImplementationLoadLevel.A)
    {
    }

    public ConfigurationForm(
      RP1210CProtocol protocol,
      RP1210_Configuration configuration,
      PEAKRP32Plus.ImplementationLoadLevel implementationLoadLevel)
    {
      this.InitializeComponent();
      this.m_Config = new RP1210_Configuration(configuration);
      this.Protocol = protocol;
      this.m_ImplementationLoadLevel = implementationLoadLevel;
      this.laBAM.Visible = this.nudInterpacketTime.Visible = this.Protocol == RP1210CProtocol.J1939;
    }

    private void ConfigurationForm_Load(object sender, EventArgs e)
    {
      this.chbEnqueueEchoes.Checked = this.m_Config.ReceivingEcho;
      this.chbReceiveMsgs.Checked = this.m_Config.ReceivingMessages;
      this.rdbInclude.Checked = this.m_Config.FilterType == RP1210C_FilterType.Inclusive;
      this.rdbExclude.Checked = !this.rdbInclude.Checked;
      this.cbbFilterStatus.SelectedIndex = (int) this.m_Config.FilterStatus;
      this.nudInterpacketTime.Value = (Decimal) this.m_Config.InterpacketTime;
      switch (this.Protocol)
      {
        case RP1210CProtocol.CAN:
          foreach (RP1210_FilterCan canFilter in this.m_Config.CanFilters)
            this.lsbFilters.Items.Add((object) canFilter);
          break;
        case RP1210CProtocol.J1939:
          foreach (RP1210_FilterJ1939 j1939Filter in this.m_Config.J1939Filters)
            this.lsbFilters.Items.Add((object) j1939Filter);
          break;
        case RP1210CProtocol.ISO15765:
          foreach (RP1210_FilterIso15765 iso15765Filter in this.m_Config.Iso15765Filters)
            this.lsbFilters.Items.Add((object) iso15765Filter);
          break;
      }
      if (this.lsbFilters.Items.Count > 0 && this.lsbFilters.SelectedIndices.Count == 0)
        this.lsbFilters.SelectedIndex = 0;
      this.rdbExclude.Enabled = this.m_ImplementationLoadLevel > PEAKRP32Plus.ImplementationLoadLevel.A;
      this.nudInterpacketTime.Enabled = this.m_ImplementationLoadLevel > PEAKRP32Plus.ImplementationLoadLevel.B;
    }

    private void cbbFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnAdd.Enabled = this.cbbFilterStatus.SelectedIndex == 2;
      this.btnDel.Enabled = this.btnAdd.Enabled && this.lsbFilters.Items.Count > 0;
      this.m_Config.FilterStatus = (RP1210C_FilterStatus) this.cbbFilterStatus.SelectedIndex;
    }

    private void btnDel_Click(object sender, EventArgs e)
    {
      int index1 = -1;
      for (int index2 = this.lsbFilters.SelectedIndices.Count - 1; index2 > -1; --index2)
      {
        index1 = this.lsbFilters.SelectedIndices[index2];
        switch (this.Protocol)
        {
          case RP1210CProtocol.CAN:
            this.m_Config.RemoveFilter((RP1210_FilterCan) this.lsbFilters.Items[index1]);
            break;
          case RP1210CProtocol.J1939:
            this.m_Config.RemoveFilter((RP1210_FilterJ1939) this.lsbFilters.Items[index1]);
            break;
          case RP1210CProtocol.ISO15765:
            this.m_Config.RemoveFilter((RP1210_FilterIso15765) this.lsbFilters.Items[index1]);
            break;
          default:
            continue;
        }
        this.lsbFilters.Items.RemoveAt(index1);
      }
      this.lsbFilters.SelectedItems.Clear();
      if (!this.btnDel.Enabled)
        return;
      this.lsbFilters.SelectedIndex = index1 < this.lsbFilters.Items.Count ? index1 : this.lsbFilters.Items.Count - 1;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      switch (this.Protocol)
      {
        case RP1210CProtocol.CAN:
          this.AddCustomFilterCan();
          break;
        case RP1210CProtocol.J1939:
          this.AddCustomFilterJ1939();
          break;
        case RP1210CProtocol.ISO15765:
          this.AddCustomFilterISO15765();
          break;
      }
      if (this.lsbFilters.SelectedItem == null && this.lsbFilters.Items.Count > 0)
        this.lsbFilters.SelectedItem = this.lsbFilters.Items[0];
      this.btnDel.Enabled = true;
    }

    private void AddCustomFilterJ1939()
    {
      FormCustomFilterJ1939 customFilterJ1939 = new FormCustomFilterJ1939();
      if (customFilterJ1939.ShowDialog() != DialogResult.OK)
        return;
      RP1210_FilterJ1939 filter = new RP1210_FilterJ1939();
      filter.Flags = customFilterJ1939.Falgs;
      filter.PGN = customFilterJ1939.PGN;
      filter.Priority = customFilterJ1939.Priority;
      filter.SourceAddress = customFilterJ1939.SourceAddress;
      filter.DestinationAddress = customFilterJ1939.DestinationAddress;
      if (!this.m_Config.AddFilter(filter))
      {
        int num = (int) MessageBox.Show("Filter already exists");
      }
      else
        this.lsbFilters.Items.Add((object) filter);
    }

    private void AddCustomFilterISO15765()
    {
      FormCustomFilterISO15765 customFilterIsO15765 = new FormCustomFilterISO15765();
      if (customFilterIsO15765.ShowDialog() != DialogResult.OK)
        return;
      RP1210_FilterIso15765 filter = new RP1210_FilterIso15765()
      {
        FilterType = customFilterIsO15765.MessageType,
        Mask = customFilterIsO15765.Mask,
        Header = customFilterIsO15765.Header
      };
      filter.ExtendedAddressMask = filter.FilterType > RP1210CISO15765MsgType.EXTENDED_CAN ? customFilterIsO15765.ExtendedAddressMask : (byte) 0;
      filter.ExtendedAddressHeader = filter.FilterType > RP1210CISO15765MsgType.EXTENDED_CAN ? customFilterIsO15765.ExtendedAddressHeader : (byte) 0;
      if (!this.m_Config.AddFilter(filter))
      {
        int num = (int) MessageBox.Show("Filter already exists");
      }
      else
        this.lsbFilters.Items.Add((object) filter);
    }

    private void AddCustomFilterCan()
    {
      FormCustomFilterCan formCustomFilterCan = new FormCustomFilterCan();
      if (formCustomFilterCan.ShowDialog() != DialogResult.OK)
        return;
      RP1210_FilterCan filter = new RP1210_FilterCan();
      filter.Extended = formCustomFilterCan.Extended;
      filter.Mask = formCustomFilterCan.Mask;
      filter.Header = formCustomFilterCan.Header;
      if (!this.m_Config.AddFilter(filter))
      {
        int num = (int) MessageBox.Show("Filter already exists");
      }
      else
        this.lsbFilters.Items.Add((object) filter);
    }

    private void lsbFilters_SelectedIndexChanged(object sender, EventArgs e) => this.btnDel.Enabled = this.lsbFilters.Items.Count > 0 && this.cbbFilterStatus.SelectedIndex == 2;

    private void chbReceiveMsgs_CheckedChanged(object sender, EventArgs e) => this.m_Config.ReceivingMessages = this.chbReceiveMsgs.Checked;

    private void chbEnqueueEchoes_CheckedChanged(object sender, EventArgs e) => this.m_Config.ReceivingEcho = this.chbEnqueueEchoes.Checked;

    private void rdbInclude_CheckedChanged(object sender, EventArgs e) => this.m_Config.FilterType = this.rdbInclude.Checked ? RP1210C_FilterType.Inclusive : RP1210C_FilterType.Exclusive;

    private void nudInterpacketTime_ValueChanged(object sender, EventArgs e) => this.m_Config.InterpacketTime = (uint) this.nudInterpacketTime.Value;

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
      this.groupBox2 = new GroupBox();
      this.btnDel = new Button();
      this.btnAdd = new Button();
      this.label3 = new Label();
      this.lsbFilters = new ListBox();
      this.cbbFilterStatus = new ComboBox();
      this.label2 = new Label();
      this.rdbExclude = new RadioButton();
      this.rdbInclude = new RadioButton();
      this.label1 = new Label();
      this.btnCancel = new Button();
      this.groupBox1 = new GroupBox();
      this.laBAM = new Label();
      this.nudInterpacketTime = new NumericUpDown();
      this.chbEnqueueEchoes = new CheckBox();
      this.chbReceiveMsgs = new CheckBox();
      this.btnApply = new Button();
      this.groupBox2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.nudInterpacketTime.BeginInit();
      this.SuspendLayout();
      this.groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox2.Controls.Add((Control) this.btnDel);
      this.groupBox2.Controls.Add((Control) this.btnAdd);
      this.groupBox2.Controls.Add((Control) this.label3);
      this.groupBox2.Controls.Add((Control) this.lsbFilters);
      this.groupBox2.Controls.Add((Control) this.cbbFilterStatus);
      this.groupBox2.Controls.Add((Control) this.label2);
      this.groupBox2.Controls.Add((Control) this.rdbExclude);
      this.groupBox2.Controls.Add((Control) this.rdbInclude);
      this.groupBox2.Controls.Add((Control) this.label1);
      this.groupBox2.Location = new Point(12, 100);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(353, 238);
      this.groupBox2.TabIndex = 1;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Message Filtering";
      this.btnDel.Enabled = false;
      this.btnDel.Location = new Point(321, 133);
      this.btnDel.Name = "btnDel";
      this.btnDel.Size = new Size(26, 23);
      this.btnDel.TabIndex = 8;
      this.btnDel.Text = "-";
      this.btnDel.UseVisualStyleBackColor = true;
      this.btnDel.Click += new EventHandler(this.btnDel_Click);
      this.btnAdd.Enabled = false;
      this.btnAdd.Location = new Point(321, 104);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(26, 23);
      this.btnAdd.TabIndex = 7;
      this.btnAdd.Text = "+";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 88);
      this.label3.Name = "label3";
      this.label3.Size = new Size(75, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Custom Filters:";
      this.lsbFilters.FormattingEnabled = true;
      this.lsbFilters.Location = new Point(9, 104);
      this.lsbFilters.Name = "lsbFilters";
      this.lsbFilters.ScrollAlwaysVisible = true;
      this.lsbFilters.SelectionMode = SelectionMode.MultiExtended;
      this.lsbFilters.Size = new Size(306, 121);
      this.lsbFilters.TabIndex = 6;
      this.lsbFilters.SelectedIndexChanged += new EventHandler(this.lsbFilters_SelectedIndexChanged);
      this.cbbFilterStatus.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbbFilterStatus.FormattingEnabled = true;
      this.cbbFilterStatus.Items.AddRange(new object[3]
      {
        (object) "Discard all messages (close)",
        (object) "Accept all messages (open)",
        (object) "Custom Filtering..."
      });
      this.cbbFilterStatus.Location = new Point(74, 52);
      this.cbbFilterStatus.Name = "cbbFilterStatus";
      this.cbbFilterStatus.Size = new Size(158, 21);
      this.cbbFilterStatus.TabIndex = 4;
      this.cbbFilterStatus.SelectedIndexChanged += new EventHandler(this.cbbFilterStatus_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 55);
      this.label2.Name = "label2";
      this.label2.Size = new Size(62, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Filter Staus:";
      this.rdbExclude.AutoSize = true;
      this.rdbExclude.Location = new Point(169, 28);
      this.rdbExclude.Name = "rdbExclude";
      this.rdbExclude.Size = new Size(63, 17);
      this.rdbExclude.TabIndex = 2;
      this.rdbExclude.TabStop = true;
      this.rdbExclude.Text = "Exclude";
      this.rdbExclude.UseVisualStyleBackColor = true;
      this.rdbExclude.CheckedChanged += new EventHandler(this.rdbInclude_CheckedChanged);
      this.rdbInclude.AutoSize = true;
      this.rdbInclude.Location = new Point(74, 28);
      this.rdbInclude.Name = "rdbInclude";
      this.rdbInclude.Size = new Size(60, 17);
      this.rdbInclude.TabIndex = 1;
      this.rdbInclude.TabStop = true;
      this.rdbInclude.Text = "Include";
      this.rdbInclude.UseVisualStyleBackColor = true;
      this.rdbInclude.CheckedChanged += new EventHandler(this.rdbInclude_CheckedChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 30);
      this.label1.Name = "label1";
      this.label1.Size = new Size(59, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Filter Type:";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(288, 344);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.laBAM);
      this.groupBox1.Controls.Add((Control) this.nudInterpacketTime);
      this.groupBox1.Controls.Add((Control) this.chbEnqueueEchoes);
      this.groupBox1.Controls.Add((Control) this.chbReceiveMsgs);
      this.groupBox1.Location = new Point(12, 3);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(353, 91);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Options";
      this.laBAM.AutoSize = true;
      this.laBAM.Location = new Point(215, 34);
      this.laBAM.Name = "laBAM";
      this.laBAM.Size = new Size(96, 13);
      this.laBAM.TabIndex = 1;
      this.laBAM.Text = "BAM Packet-Time:";
      this.nudInterpacketTime.Increment = new Decimal(new int[4]
      {
        5,
        0,
        0,
        0
      });
      this.nudInterpacketTime.Location = new Point(217, 56);
      this.nudInterpacketTime.Maximum = new Decimal(new int[4]
      {
        500,
        0,
        0,
        0
      });
      this.nudInterpacketTime.Name = "nudInterpacketTime";
      this.nudInterpacketTime.Size = new Size(73, 20);
      this.nudInterpacketTime.TabIndex = 3;
      this.nudInterpacketTime.Value = new Decimal(new int[4]
      {
        50,
        0,
        0,
        0
      });
      this.nudInterpacketTime.ValueChanged += new EventHandler(this.nudInterpacketTime_ValueChanged);
      this.chbEnqueueEchoes.AutoSize = true;
      this.chbEnqueueEchoes.Location = new Point(9, 59);
      this.chbEnqueueEchoes.Name = "chbEnqueueEchoes";
      this.chbEnqueueEchoes.Size = new Size(148, 17);
      this.chbEnqueueEchoes.TabIndex = 2;
      this.chbEnqueueEchoes.Text = "Enqueue Echo Messages";
      this.chbEnqueueEchoes.UseVisualStyleBackColor = true;
      this.chbEnqueueEchoes.CheckedChanged += new EventHandler(this.chbEnqueueEchoes_CheckedChanged);
      this.chbReceiveMsgs.AutoSize = true;
      this.chbReceiveMsgs.Location = new Point(9, 33);
      this.chbReceiveMsgs.Name = "chbReceiveMsgs";
      this.chbReceiveMsgs.Size = new Size(125, 17);
      this.chbReceiveMsgs.TabIndex = 0;
      this.chbReceiveMsgs.Text = "Receiving Messages";
      this.chbReceiveMsgs.UseVisualStyleBackColor = true;
      this.chbReceiveMsgs.CheckedChanged += new EventHandler(this.chbReceiveMsgs_CheckedChanged);
      this.btnApply.DialogResult = DialogResult.OK;
      this.btnApply.Location = new Point(207, 344);
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = new Size(75, 23);
      this.btnApply.TabIndex = 2;
      this.btnApply.Text = "Apply";
      this.btnApply.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnApply;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(375, 379);
      this.Controls.Add((Control) this.btnApply);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConfigurationForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Session Configuration";
      this.Load += new EventHandler(this.ConfigurationForm_Load);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.nudInterpacketTime.EndInit();
      this.ResumeLayout(false);
    }
  }
}
