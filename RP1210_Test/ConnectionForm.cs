
// Type: RP1210_Test.ConnectionForm




using RP1210_Test.HelpClasses;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RP1210_Test
{
  public class ConnectionForm : Form
  {
    private RP1210_InformationReader m_Reader;
    private RP1210_ProtocolInfo m_Protocol;
    private RP1210_Baudrate m_Baudrate;
    private RP1210_DeviceInfo m_Device;
    private int m_Channel;
    private bool m_UsePCANRP64;
    private IContainer components;
    private ComboBox cbbImplementations;
    private GroupBox groupBox1;
    private Label label3;
    private Label label2;
    private Label laAddress;
    private Label laName;
    private Label laTelephone;
    private Label label4;
    private Label label5;
    private LinkLabel llaWebsite;
    private ComboBox cbbProtocols;
    private Label label7;
    private Label label6;
    private ComboBox cbbDevices;
    private GroupBox groupBox2;
    private GroupBox groupBox3;
    private Button btnOK;
    private Button btnCancel;
    private Label label1;
    private ComboBox cbbBaudrate;
    private CheckBox chbAppIsPacking;
    private CheckBox chbUseOldDllName;
    private Label laChannel;
    private ComboBox cbbChannel;

    public ConnectionForm()
      : this(new RP1210_ImplementationInformation())
    {
    }

    public ConnectionForm(RP1210_ImplementationInformation implementation)
    {
      this.InitializeComponent();
      this.m_UsePCANRP64 = false;
      this.m_Baudrate = RP1210_Baudrate.Standard;
      this.m_Reader = new RP1210_InformationReader(implementation.Name);
    }

    private void SetImplementation(RP1210_ImplementationInformation value)
    {
      this.m_Reader = new RP1210_InformationReader(value.ToString());
      this.ConnectionForm_Load((object) this, new EventArgs());
    }

    private void ConnectionForm_Load(object sender, EventArgs e)
    {
      this.cbbChannel.SelectedIndex = 0;
      this.cbbImplementations.Items.Clear();
      this.cbbImplementations.Items.AddRange((object[]) RP1210_ImplementationInformation.Implementations);
      if (this.m_Reader.Implementation.Name == null && this.cbbImplementations.Items.Count > 0)
        this.cbbImplementations.SelectedIndex = 0;
      else
        this.cbbImplementations.SelectedItem = (object) this.m_Reader.Implementation.Name;
    }

    private void cbbImplementations_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.m_Reader = new RP1210_InformationReader(this.cbbImplementations.Text);
      this.laName.Text = this.m_Reader.Name;
      this.laAddress.Text = string.Format("{0}, {1}, {2}, {3}", (object) this.m_Reader.Address1, (object) this.m_Reader.Postal, (object) this.m_Reader.City, (object) this.m_Reader.Country);
      this.laTelephone.Text = this.m_Reader.Telephone;
      this.llaWebsite.Text = this.m_Reader.VendorURL;
      this.cbbProtocols.Items.Clear();
      this.cbbProtocols.Items.AddRange(this.m_Reader.ProtocolsList.Cast<object>().ToArray<object>());
      this.cbbProtocols.Text = this.m_Protocol.ToString();
      if (this.cbbProtocols.Text == string.Empty && this.cbbProtocols.Items.Count > 0)
        this.cbbProtocols.SelectedIndex = 0;
      this.chbUseOldDllName.Visible = this.cbbImplementations.Text == "PCANRP32" && Environment.Is64BitProcess;
      this.cbbChannel.Enabled = this.cbbImplementations.Text == "PEAKRP32";
    }

    private void llaWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start(this.llaWebsite.Text);
      this.llaWebsite.LinkVisited = true;
    }

    private void cbbProtocols_SelectedIndexChanged(object sender, EventArgs e)
    {
      RP1210_ProtocolInfo selectedItem = (RP1210_ProtocolInfo) this.cbbProtocols.SelectedItem;
      this.cbbBaudrate.Items.Clear();
      this.cbbDevices.Items.Clear();
      if (selectedItem.DevicesList == null)
        return;
      this.cbbDevices.Items.AddRange(selectedItem.DevicesList.Cast<object>().ToArray<object>());
      if (this.cbbDevices.Items.Count > 0)
      {
        if ((this.m_Device.ToString() == null || this.m_Device.DeviceDescription == null) && this.cbbDevices.Items.Count > 0)
          this.cbbDevices.SelectedIndex = 0;
        else
          this.cbbDevices.Text = this.m_Device.ToString();
        this.cbbBaudrate.Items.Add((object) RP1210_Baudrate.Standard);
        if (selectedItem.ProtocolSpeed != string.Empty)
        {
          string protocolSpeed = selectedItem.ProtocolSpeed;
          char[] chArray = new char[2]{ ',', ' ' };
          foreach (string str in protocolSpeed.Split(chArray))
          {
            RP1210_Baudrate result;
            if (Enum.TryParse<RP1210_Baudrate>(str, out result))
              this.cbbBaudrate.Items.Add((object) result);
          }
        }
        if (this.m_Baudrate.ToString() == null && this.cbbBaudrate.Items.Count > 0)
          this.cbbBaudrate.SelectedIndex = 0;
        else
          this.cbbBaudrate.Text = this.m_Baudrate.ToString();
        if (this.cbbDevices.SelectedIndex == -1)
          this.cbbDevices.SelectedIndex = 0;
      }
      this.chbAppIsPacking.Enabled = selectedItem.ProtocolString == "J1939";
    }

    private void cbbDevices_SelectedIndexChanged(object sender, EventArgs e)
    {
      RP1210_DeviceInfo selectedItem = (RP1210_DeviceInfo) this.cbbDevices.SelectedItem;
      this.cbbChannel.Items.Clear();
      this.cbbChannel.Items.Add((object) 1);
      this.cbbChannel.SelectedIndex = 0;
      for (int index = 2; index <= selectedItem.MultiCANChannels; ++index)
        this.cbbChannel.Items.Add((object) index);
      this.cbbChannel.Text = this.m_Channel == 0 ? "1" : this.m_Channel.ToString();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      try
      {
        this.m_Device = (RP1210_DeviceInfo) this.cbbDevices.SelectedItem;
        this.m_Protocol = (RP1210_ProtocolInfo) this.cbbProtocols.SelectedItem;
        this.m_Baudrate = (RP1210_Baudrate) this.cbbBaudrate.SelectedItem;
        this.m_Channel = (int) this.cbbChannel.SelectedItem;
        this.m_UsePCANRP64 = this.chbUseOldDllName.Visible && this.chbUseOldDllName.Checked;
      }
      catch
      {
      }
    }

    public RP1210_ImplementationInformation Implementation
    {
      get => this.m_Reader.Implementation;
      set => this.m_Reader = new RP1210_InformationReader(value.Name);
    }

    public RP1210_DeviceInfo Device
    {
      get => this.m_Device;
      set => this.m_Device = value;
    }

    public RP1210_ProtocolInfo Protocol
    {
      get => this.m_Protocol;
      set => this.m_Protocol = value;
    }

    public RP1210_Baudrate Baudrate
    {
      get => this.m_Baudrate;
      set => this.m_Baudrate = value;
    }

    public int ChannelNumber
    {
      get => this.m_Channel;
      set => this.m_Channel = value;
    }

    public bool AppIsDoingPacketizing
    {
      get => this.chbAppIsPacking.Checked;
      set => this.chbAppIsPacking.Checked = value;
    }

    public bool UsingOldPCANRP64
    {
      get => this.chbUseOldDllName.Checked;
      set => this.chbUseOldDllName.Checked = value;
    }

    public bool UsePCANRP64File => this.m_UsePCANRP64;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cbbImplementations = new ComboBox();
      this.groupBox1 = new GroupBox();
      this.llaWebsite = new LinkLabel();
      this.label5 = new Label();
      this.laTelephone = new Label();
      this.label4 = new Label();
      this.laAddress = new Label();
      this.laName = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.cbbProtocols = new ComboBox();
      this.label7 = new Label();
      this.label6 = new Label();
      this.cbbDevices = new ComboBox();
      this.groupBox2 = new GroupBox();
      this.chbUseOldDllName = new CheckBox();
      this.groupBox3 = new GroupBox();
      this.laChannel = new Label();
      this.cbbChannel = new ComboBox();
      this.chbAppIsPacking = new CheckBox();
      this.label1 = new Label();
      this.cbbBaudrate = new ComboBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      this.cbbImplementations.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbbImplementations.FormattingEnabled = true;
      this.cbbImplementations.Location = new Point(6, 19);
      this.cbbImplementations.Name = "cbbImplementations";
      this.cbbImplementations.Size = new Size(177, 21);
      this.cbbImplementations.TabIndex = 0;
      this.cbbImplementations.SelectedIndexChanged += new EventHandler(this.cbbImplementations_SelectedIndexChanged);
      this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.llaWebsite);
      this.groupBox1.Controls.Add((Control) this.label5);
      this.groupBox1.Controls.Add((Control) this.laTelephone);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.laAddress);
      this.groupBox1.Controls.Add((Control) this.laName);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Location = new Point(12, 62);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(353, 101);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Vendor Information ";
      this.llaWebsite.AutoSize = true;
      this.llaWebsite.Location = new Point(88, 73);
      this.llaWebsite.Name = "llaWebsite";
      this.llaWebsite.Size = new Size(37, 13);
      this.llaWebsite.TabIndex = 7;
      this.llaWebsite.TabStop = true;
      this.llaWebsite.Text = "_____";
      this.llaWebsite.LinkClicked += new LinkLabelLinkClickedEventHandler(this.llaWebsite_LinkClicked);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(33, 73);
      this.label5.Name = "label5";
      this.label5.Size = new Size(49, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "Website:";
      this.laTelephone.AutoSize = true;
      this.laTelephone.Location = new Point(88, 54);
      this.laTelephone.Name = "laTelephone";
      this.laTelephone.Size = new Size(37, 13);
      this.laTelephone.TabIndex = 5;
      this.laTelephone.Text = "_____";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(21, 54);
      this.label4.Name = "label4";
      this.label4.Size = new Size(61, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Telephone:";
      this.laAddress.AutoSize = true;
      this.laAddress.Location = new Point(88, 35);
      this.laAddress.Name = "laAddress";
      this.laAddress.Size = new Size(37, 13);
      this.laAddress.TabIndex = 3;
      this.laAddress.Text = "_____";
      this.laName.AutoSize = true;
      this.laName.Location = new Point(88, 16);
      this.laName.Name = "laName";
      this.laName.Size = new Size(37, 13);
      this.laName.TabIndex = 1;
      this.laName.Text = "_____";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(34, 35);
      this.label3.Name = "label3";
      this.label3.Size = new Size(48, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Address:";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(44, 16);
      this.label2.Name = "label2";
      this.label2.Size = new Size(38, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Name:";
      this.cbbProtocols.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbbProtocols.FormattingEnabled = true;
      this.cbbProtocols.Location = new Point(91, 20);
      this.cbbProtocols.Name = "cbbProtocols";
      this.cbbProtocols.Size = new Size(247, 21);
      this.cbbProtocols.TabIndex = 1;
      this.cbbProtocols.SelectedIndexChanged += new EventHandler(this.cbbProtocols_SelectedIndexChanged);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(27, 23);
      this.label7.Name = "label7";
      this.label7.Size = new Size(54, 13);
      this.label7.TabIndex = 0;
      this.label7.Text = "Protocols:";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(33, 50);
      this.label6.Name = "label6";
      this.label6.Size = new Size(49, 13);
      this.label6.TabIndex = 2;
      this.label6.Text = "Devices:";
      this.cbbDevices.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbbDevices.FormattingEnabled = true;
      this.cbbDevices.Location = new Point(91, 47);
      this.cbbDevices.BackColor = Color.Red;
      this.cbbDevices.Name = "cbbDevices";
      this.cbbDevices.Size = new Size(247, 21);
      this.cbbDevices.TabIndex = 3;
      this.cbbDevices.SelectedIndexChanged += new EventHandler(this.cbbDevices_SelectedIndexChanged);
      this.groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox2.Controls.Add((Control) this.chbUseOldDllName);
      this.groupBox2.Controls.Add((Control) this.cbbImplementations);
      this.groupBox2.Location = new Point(12, 3);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(353, 53);
      this.groupBox2.TabIndex = 0;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "RP1210 Implementation ";
      this.chbUseOldDllName.AutoSize = true;
      this.chbUseOldDllName.Location = new Point(196, 21);
      this.chbUseOldDllName.Name = "chbUseOldDllName";
      this.chbUseOldDllName.Size = new Size(136, 17);
      this.chbUseOldDllName.TabIndex = 1;
      this.chbUseOldDllName.Text = "Load as PCANRP64.dll";
      this.chbUseOldDllName.UseVisualStyleBackColor = true;
      this.groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox3.Controls.Add((Control) this.laChannel);
      this.groupBox3.Controls.Add((Control) this.cbbChannel);
      this.groupBox3.Controls.Add((Control) this.chbAppIsPacking);
      this.groupBox3.Controls.Add((Control) this.label1);
      this.groupBox3.Controls.Add((Control) this.cbbBaudrate);
      this.groupBox3.Controls.Add((Control) this.cbbDevices);
      this.groupBox3.Controls.Add((Control) this.label6);
      this.groupBox3.Controls.Add((Control) this.cbbProtocols);
      this.groupBox3.Controls.Add((Control) this.label7);
      this.groupBox3.Location = new Point(12, 169);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(353, 135);
      this.groupBox3.TabIndex = 2;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Connection ";
      this.laChannel.AutoSize = true;
      this.laChannel.Location = new Point(32, 104);
      this.laChannel.Name = "laChannel";
      this.laChannel.Size = new Size(49, 13);
      this.laChannel.TabIndex = 7;
      this.laChannel.Text = "Channel:";
      this.cbbChannel.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbbChannel.FormattingEnabled = true;
      this.cbbChannel.Items.AddRange(new object[6]
      {
        (object) "1",
        (object) "2",
        (object) "3",
        (object) "4",
        (object) "5",
        (object) "6"
      });
      this.cbbChannel.Location = new Point(91, 101);
      this.cbbChannel.Name = "cbbChannel";
      this.cbbChannel.Size = new Size(87, 21);
      this.cbbChannel.TabIndex = 8;
      this.chbAppIsPacking.AutoSize = true;
      this.chbAppIsPacking.Location = new Point(196, 78);
      this.chbAppIsPacking.Name = "chbAppIsPacking";
      this.chbAppIsPacking.Size = new Size(141, 17);
      this.chbAppIsPacking.TabIndex = 6;
      this.chbAppIsPacking.Text = "App is doing packetizing";
      this.chbAppIsPacking.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(25, 77);
      this.label1.Name = "label1";
      this.label1.Size = new Size(56, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Baud rate:";
      this.cbbBaudrate.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbbBaudrate.FormattingEnabled = true;
      this.cbbBaudrate.Items.AddRange(new object[6]
      {
        (object) "Default",
        (object) "125 kBits",
        (object) "250 kBits",
        (object) "500 kBits",
        (object) "1000 kBits",
        (object) "Auto"
      });
      this.cbbBaudrate.Location = new Point(91, 74);
      this.cbbBaudrate.Name = "cbbBaudrate";
      this.cbbBaudrate.Size = new Size(87, 21);
      this.cbbBaudrate.TabIndex = 5;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(208, 314);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(290, 314);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(375, 349);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.groupBox3);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.groupBox1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConnectionForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Gateway Selection";
      this.Load += new EventHandler(this.ConnectionForm_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
