
// Type: RP1210_Test.FormTxMsgISO15765




using Peak.RP1210C;
using RP1210_Test.HelpClasses;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace RP1210_Test
{
  public class FormTxMsgISO15765 : Form
  {
    private byte[] m_Data;
    private IContainer components;
    private Button btnCancel;
    private Button btnOK;
    private TextBox txtData;
    private Label label7;
    private NumericUpDown nudDLC;
    private Label label6;
    private ComboBox cbbMode;
    private Label label4;
    private Label label1;
    private TextBox txtID;
    private Label label2;
    private TextBox txtAddr;
    private GroupBox groupBox1;
    private GroupBox grbInterval;
    private Label label13;
    private NumericUpDown nudInterval;

    public FormTxMsgISO15765()
      : this(false, new RP1210_MsgIso15765(RP1210CISO15765MsgType.STANDARD_CAN, 2047U, (byte) 0))
    {
    }

    public FormTxMsgISO15765(bool isBroadcast)
      : this(isBroadcast, new RP1210_MsgIso15765(RP1210CISO15765MsgType.STANDARD_CAN, 2047U, (byte) 0))
    {
    }

    public FormTxMsgISO15765(bool isBroadcast, RP1210_MsgIso15765 msg)
    {
      this.InitializeComponent();
      this.Message = msg;
      this.ConfigureUI(isBroadcast);
    }

    private void ConfigureUI(bool isBroadcast)
    {
      this.grbInterval.Visible = isBroadcast;
      this.Height = isBroadcast ? 410 : 335;
      this.Text = isBroadcast ? "New ISO15765 Broadcast Message" : "New Transmit ISO15765 Message";
    }

    private RP1210_MsgIso15765 CompileMessage()
    {
      RP1210_MsgIso15765 rp1210MsgIso15765 = new RP1210_MsgIso15765();
      rp1210MsgIso15765.MessageType = (RP1210CISO15765MsgType) this.cbbMode.SelectedIndex;
      rp1210MsgIso15765.ID = uint.Parse(this.txtID.Text, NumberStyles.HexNumber);
      rp1210MsgIso15765.ExtendedAddress = byte.Parse(this.txtAddr.Text);
      rp1210MsgIso15765.Data = (byte[]) this.m_Data.Clone();
      return rp1210MsgIso15765;
    }

    private void LoadMessage(RP1210_MsgIso15765 value)
    {
      string str = "";
      this.m_Data = (byte[]) value.Data.Clone();
      this.cbbMode.SelectedIndex = (int) value.MessageType;
      if (value.MessageType == RP1210CISO15765MsgType.EXTENDED_CAN_ISO15765_EXTENDED || value.MessageType == RP1210CISO15765MsgType.STANDARD_CAN_ISO15765_EXTENDED)
        this.txtID.Text = value.ID.ToString("X8");
      else
        this.txtID.Text = value.ID.ToString("X4");
      this.txtAddr.Text = value.ExtendedAddress.ToString();
      this.txtAddr.Tag = (object) value.ExtendedAddress;
      this.nudDLC.Value = (Decimal) this.m_Data.Length;
      for (int index = 0; index < this.m_Data.Length; ++index)
        str += string.Format("{0:X2} ", (object) this.m_Data[index]);
      this.txtData.Text = str;
      this.cbbMode_SelectedIndexChanged((object) this, new EventArgs());
    }

    private void nudDLC_ValueChanged(object sender, EventArgs e)
    {
      string str = "";
      Random random = new Random();
      this.m_Data = new byte[(int) this.nudDLC.Value];
      byte[] data = this.m_Data;
      random.NextBytes(data);
      for (int index = 0; index < this.m_Data.Length; ++index)
        str += string.Format("{0:X2} ", (object) this.m_Data[index]);
      this.txtData.Text = str;
    }

    private void cbbMode_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.txtID.MaxLength = this.cbbMode.SelectedIndex == 1 || this.cbbMode.SelectedIndex == 3 ? 8 : 4;
      this.txtAddr.Enabled = this.cbbMode.SelectedIndex >= 2;
      this.txtID_Leave((object) this.txtID, new EventArgs());
    }

    private void txtID_Leave(object sender, EventArgs e)
    {
      bool flag = this.cbbMode.SelectedIndex == 1 || this.cbbMode.SelectedIndex == 3;
      if (this.txtID.Text == "")
        this.txtID.Text = "0";
      uint num = flag ? 536870911U : 2047U;
      uint uint32 = Convert.ToUInt32(this.txtID.Text, 16);
      if (uint32 > num)
        this.txtID.Text = num.ToString("X" + (flag ? "8" : "4"));
      else
        this.txtID.Text = uint32.ToString("X" + (flag ? "8" : "4"));
    }

    private void txtID_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!(sender is TextBox textBox) || e.KeyChar == '\b')
        return;
      e.Handled = !ulong.TryParse(textBox.Text + e.KeyChar.ToString(), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out ulong _);
    }

    private void txtAddr_Leave(object sender, EventArgs e)
    {
      if (!byte.TryParse(this.txtAddr.Text, out byte _))
        this.txtAddr.Text = ((byte) this.txtAddr.Tag).ToString();
      else
        this.txtAddr.Tag = (object) byte.Parse(this.txtAddr.Text);
    }

    public RP1210_MsgIso15765 Message
    {
      get => this.CompileMessage();
      set => this.LoadMessage(value);
    }

    public ushort Interval
    {
      get => Convert.ToUInt16(this.nudInterval.Value);
      set => this.nudInterval.Value = (Decimal) value;
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
      this.txtData = new TextBox();
      this.label7 = new Label();
      this.nudDLC = new NumericUpDown();
      this.label6 = new Label();
      this.cbbMode = new ComboBox();
      this.label4 = new Label();
      this.label1 = new Label();
      this.txtID = new TextBox();
      this.label2 = new Label();
      this.txtAddr = new TextBox();
      this.groupBox1 = new GroupBox();
      this.grbInterval = new GroupBox();
      this.label13 = new Label();
      this.nudInterval = new NumericUpDown();
      this.nudDLC.BeginInit();
      this.groupBox1.SuspendLayout();
      this.grbInterval.SuspendLayout();
      this.nudInterval.BeginInit();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(395, 337);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(312, 337);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.txtData.Location = new Point(9, 85);
      this.txtData.Multiline = true;
      this.txtData.Name = "txtData";
      this.txtData.ReadOnly = true;
      this.txtData.ScrollBars = ScrollBars.Both;
      this.txtData.Size = new Size(436, 143);
      this.txtData.TabIndex = 8;
      this.txtData.TabStop = false;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(6, 69);
      this.label7.Name = "label7";
      this.label7.Size = new Size(33, 13);
      this.label7.TabIndex = 9;
      this.label7.Text = "Data:";
      this.nudDLC.Location = new Point(374, 33);
      this.nudDLC.Maximum = new Decimal(new int[4]
      {
        4095,
        0,
        0,
        0
      });
      this.nudDLC.Name = "nudDLC";
      this.nudDLC.Size = new Size(71, 20);
      this.nudDLC.TabIndex = 7;
      this.nudDLC.ValueChanged += new EventHandler(this.nudDLC_ValueChanged);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(371, 16);
      this.label6.Name = "label6";
      this.label6.Size = new Size(69, 13);
      this.label6.TabIndex = 6;
      this.label6.Text = "Data Length:";
      this.cbbMode.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbbMode.FormattingEnabled = true;
      this.cbbMode.Items.AddRange(new object[5]
      {
        (object) "Standard (11-bit)",
        (object) "Extended (29-bit)",
        (object) "Standard (11-bit) - Extended Address",
        (object) "Extended (29-bit) - Extended Address",
        (object) "Standard (11-bit) - Mixed Addressing"
      });
      this.cbbMode.Location = new Point(9, 33);
      this.cbbMode.Name = "cbbMode";
      this.cbbMode.Size = new Size(200, 21);
      this.cbbMode.TabIndex = 1;
      this.cbbMode.SelectedIndexChanged += new EventHandler(this.cbbMode_SelectedIndexChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(315, 16);
      this.label4.Name = "label4";
      this.label4.Size = new Size(50, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Ext.Addr:";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(80, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Message Type:";
      this.txtID.CharacterCasing = CharacterCasing.Upper;
      this.txtID.Location = new Point(221, 34);
      this.txtID.Name = "txtID";
      this.txtID.Size = new Size(85, 20);
      this.txtID.TabIndex = 3;
      this.txtID.Text = "0000";
      this.txtID.KeyPress += new KeyPressEventHandler(this.txtID_KeyPress);
      this.txtID.Leave += new EventHandler(this.txtID_Leave);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(218, 16);
      this.label2.Name = "label2";
      this.label2.Size = new Size(49, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "ID (Hex):";
      this.txtAddr.CharacterCasing = CharacterCasing.Upper;
      this.txtAddr.Location = new Point(318, 33);
      this.txtAddr.MaxLength = 3;
      this.txtAddr.Name = "txtAddr";
      this.txtAddr.Size = new Size(44, 20);
      this.txtAddr.TabIndex = 5;
      this.txtAddr.Text = "00";
      this.txtAddr.Leave += new EventHandler(this.txtAddr_Leave);
      this.groupBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.txtAddr);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.txtID);
      this.groupBox1.Controls.Add((Control) this.cbbMode);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.label6);
      this.groupBox1.Controls.Add((Control) this.nudDLC);
      this.groupBox1.Controls.Add((Control) this.label7);
      this.groupBox1.Controls.Add((Control) this.txtData);
      this.groupBox1.Location = new Point(12, 86);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(454, 241);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = " Message ";
      this.grbInterval.Controls.Add((Control) this.label13);
      this.grbInterval.Controls.Add((Control) this.nudInterval);
      this.grbInterval.Location = new Point(12, 12);
      this.grbInterval.Name = "grbInterval";
      this.grbInterval.Size = new Size(454, 66);
      this.grbInterval.TabIndex = 0;
      this.grbInterval.TabStop = false;
      this.grbInterval.Text = "Interval";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(143, 32);
      this.label13.Name = "label13";
      this.label13.Size = new Size(64, 13);
      this.label13.TabIndex = 1;
      this.label13.Text = "Milliseconds";
      this.nudInterval.Increment = new Decimal(new int[4]
      {
        10,
        0,
        0,
        0
      });
      this.nudInterval.Location = new Point(10, 30);
      this.nudInterval.Maximum = new Decimal(new int[4]
      {
        60000,
        0,
        0,
        0
      });
      this.nudInterval.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.nudInterval.Name = "nudInterval";
      this.nudInterval.Size = new Size(120, 20);
      this.nudInterval.TabIndex = 0;
      this.nudInterval.Value = new Decimal(new int[4]
      {
        1000,
        0,
        0,
        0
      });
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(478, 371);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.grbInterval);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FormTxMsgISO15765);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "New Transmit ISO15765 Message";
      this.nudDLC.EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.grbInterval.ResumeLayout(false);
      this.grbInterval.PerformLayout();
      this.nudInterval.EndInit();
      this.ResumeLayout(false);
    }
  }
}
