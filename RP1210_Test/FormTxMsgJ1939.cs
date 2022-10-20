
// Type: RP1210_Test.FormTxMsgJ1939




using RP1210_Test.HelpClasses;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RP1210_Test
{
  public class FormTxMsgJ1939 : Form
  {
    private byte[] m_Data;
    private IContainer components;
    private NumericUpDown nudSource;
    private Label label2;
    private Label label1;
    private NumericUpDown nudDestination;
    private Label label3;
    private NumericUpDown nudPriority;
    private Label label4;
    private Label label5;
    private ComboBox cbbMode;
    private Label label6;
    private NumericUpDown nudDLC;
    private Label label7;
    private TextBox txtData;
    private Button btnCancel;
    private Button btnOK;
    private TextBox txtPGN;
    private GroupBox groupBox1;
    private GroupBox grbInterval;
    private Label label13;
    private NumericUpDown nudInterval;

    public FormTxMsgJ1939()
      : this(false, new RP1210_MsgJ1939(0U, RP1210_MsgJ1939.TransportMode.BAM, (byte) 7, (byte) 248, byte.MaxValue, new byte[0]))
    {
    }

    public FormTxMsgJ1939(bool isBroadcast)
      : this(isBroadcast, new RP1210_MsgJ1939(0U, RP1210_MsgJ1939.TransportMode.BAM, (byte) 7, (byte) 248, byte.MaxValue, new byte[0]))
    {
    }

    public FormTxMsgJ1939(bool isBroadcast, RP1210_MsgJ1939 msg)
    {
      this.InitializeComponent();
      this.Message = msg;
      this.ConfigureUI(isBroadcast);
    }

    private void ConfigureUI(bool isBroadcast)
    {
      this.grbInterval.Visible = isBroadcast;
      this.Height = isBroadcast ? 405 : 330;
      this.Text = isBroadcast ? "New J1939 Broadcast Message" : "New Transmit J1939 Message";
    }

    private RP1210_MsgJ1939 CompileMessage()
    {
      RP1210_MsgJ1939 rp1210MsgJ1939 = new RP1210_MsgJ1939();
      rp1210MsgJ1939.Length = (int) this.nudDLC.Value;
      rp1210MsgJ1939.PGN = Convert.ToUInt32(this.txtPGN.Text);
      rp1210MsgJ1939.Priority = Convert.ToByte(this.nudPriority.Value);
      rp1210MsgJ1939.SourceAddress = Convert.ToByte(this.nudSource.Value);
      rp1210MsgJ1939.DestinationAddress = Convert.ToByte(this.nudDestination.Value);
      rp1210MsgJ1939.SendMode = this.cbbMode.SelectedIndex == 0 ? RP1210_MsgJ1939.TransportMode.RTS_CTS : RP1210_MsgJ1939.TransportMode.BAM;
      rp1210MsgJ1939.Data = (byte[]) this.m_Data?.Clone() ?? new byte[0];
      return rp1210MsgJ1939;
    }

    private void LoadMessage(RP1210_MsgJ1939 msg)
    {
      string str = "";
      this.txtPGN.Text = msg.PGN.ToString();
      this.nudSource.Value = (Decimal) msg.SourceAddress;
      this.nudDestination.Value = (Decimal) msg.DestinationAddress;
      this.nudPriority.Value = (Decimal) msg.Priority;
      this.cbbMode.SelectedIndex = msg.SendMode == RP1210_MsgJ1939.TransportMode.RTS_CTS ? 0 : 1;
      this.nudDLC.Value = (Decimal) msg.Length;
      for (int index = 0; index < msg.Length; ++index)
        str += string.Format("{0:X2} ", (object) msg.Data[index]);
      this.txtData.Text = str;
    }

    private void txtPGN_Leave(object sender, EventArgs e)
    {
      if (this.txtPGN.Text == "")
        this.txtPGN.Text = "0";
      if (uint.Parse(this.txtPGN.Text) <= 16777215U)
        return;
      this.txtPGN.Text = 16777215U.ToString();
    }

    private void txtPGN_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!(sender is TextBox textBox) || e.KeyChar == '\b')
        return;
      e.Handled = !uint.TryParse(textBox.Text + e.KeyChar.ToString(), out uint _);
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

    public RP1210_MsgJ1939 Message
    {
      get => this.CompileMessage();
      set => this.LoadMessage(value);
    }

    public ushort Interval
    {
      get => Convert.ToUInt16(this.nudInterval.Value);
      set => this.nudInterval.Value = (Decimal) value;
    }

    public byte SA
    {
      get => (byte) this.nudSource.Value;
      set => this.nudSource.Value = (Decimal) value;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.nudSource = new NumericUpDown();
      this.label2 = new Label();
      this.label1 = new Label();
      this.nudDestination = new NumericUpDown();
      this.label3 = new Label();
      this.nudPriority = new NumericUpDown();
      this.label4 = new Label();
      this.label5 = new Label();
      this.cbbMode = new ComboBox();
      this.label6 = new Label();
      this.nudDLC = new NumericUpDown();
      this.label7 = new Label();
      this.txtData = new TextBox();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.txtPGN = new TextBox();
      this.groupBox1 = new GroupBox();
      this.grbInterval = new GroupBox();
      this.label13 = new Label();
      this.nudInterval = new NumericUpDown();
      this.nudSource.BeginInit();
      this.nudDestination.BeginInit();
      this.nudPriority.BeginInit();
      this.nudDLC.BeginInit();
      this.groupBox1.SuspendLayout();
      this.grbInterval.SuspendLayout();
      this.nudInterval.BeginInit();
      this.SuspendLayout();
      this.nudSource.Location = new Point(93, 32);
      this.nudSource.Maximum = new Decimal(new int[4]
      {
        253,
        0,
        0,
        0
      });
      this.nudSource.Name = "nudSource";
      this.nudSource.Size = new Size(44, 20);
      this.nudSource.TabIndex = 3;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(93, 16);
      this.label2.Name = "label2";
      this.label2.Size = new Size(44, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Source:";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(33, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "PGN:";
      this.nudDestination.Location = new Point(155, 33);
      this.nudDestination.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.nudDestination.Name = "nudDestination";
      this.nudDestination.Size = new Size(44, 20);
      this.nudDestination.TabIndex = 5;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(152, 16);
      this.label3.Name = "label3";
      this.label3.Size = new Size(63, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Destination:";
      this.nudPriority.Location = new Point(220, 33);
      this.nudPriority.Maximum = new Decimal(new int[4]
      {
        7,
        0,
        0,
        0
      });
      this.nudPriority.Name = "nudPriority";
      this.nudPriority.Size = new Size(44, 20);
      this.nudPriority.TabIndex = 7;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(217, 16);
      this.label4.Name = "label4";
      this.label4.Size = new Size(41, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Priority:";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(276, 16);
      this.label5.Name = "label5";
      this.label5.Size = new Size(37, 13);
      this.label5.TabIndex = 8;
      this.label5.Text = "Mode:";
      this.cbbMode.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbbMode.FormattingEnabled = true;
      this.cbbMode.Items.AddRange(new object[2]
      {
        (object) "RTS/CTS",
        (object) "BAM"
      });
      this.cbbMode.Location = new Point(279, 33);
      this.cbbMode.Name = "cbbMode";
      this.cbbMode.Size = new Size(77, 21);
      this.cbbMode.TabIndex = 9;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(371, 16);
      this.label6.Name = "label6";
      this.label6.Size = new Size(69, 13);
      this.label6.TabIndex = 10;
      this.label6.Text = "Data Length:";
      this.nudDLC.Location = new Point(374, 33);
      this.nudDLC.Maximum = new Decimal(new int[4]
      {
        1785,
        0,
        0,
        0
      });
      this.nudDLC.Name = "nudDLC";
      this.nudDLC.Size = new Size(71, 20);
      this.nudDLC.TabIndex = 11;
      this.nudDLC.ValueChanged += new EventHandler(this.nudDLC_ValueChanged);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(6, 69);
      this.label7.Name = "label7";
      this.label7.Size = new Size(33, 13);
      this.label7.TabIndex = 12;
      this.label7.Text = "Data:";
      this.txtData.Location = new Point(9, 85);
      this.txtData.Multiline = true;
      this.txtData.Name = "txtData";
      this.txtData.ReadOnly = true;
      this.txtData.ScrollBars = ScrollBars.Both;
      this.txtData.Size = new Size(436, 143);
      this.txtData.TabIndex = 13;
      this.txtData.TabStop = false;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(394, 332);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(311, 332);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.txtPGN.CharacterCasing = CharacterCasing.Upper;
      this.txtPGN.Location = new Point(9, 33);
      this.txtPGN.MaxLength = 8;
      this.txtPGN.Name = "txtPGN";
      this.txtPGN.Size = new Size(71, 20);
      this.txtPGN.TabIndex = 1;
      this.txtPGN.KeyPress += new KeyPressEventHandler(this.txtPGN_KeyPress);
      this.txtPGN.Leave += new EventHandler(this.txtPGN_Leave);
      this.groupBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.txtPGN);
      this.groupBox1.Controls.Add((Control) this.txtData);
      this.groupBox1.Controls.Add((Control) this.nudSource);
      this.groupBox1.Controls.Add((Control) this.label7);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.nudDLC);
      this.groupBox1.Controls.Add((Control) this.nudDestination);
      this.groupBox1.Controls.Add((Control) this.label6);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.cbbMode);
      this.groupBox1.Controls.Add((Control) this.nudPriority);
      this.groupBox1.Controls.Add((Control) this.label5);
      this.groupBox1.Location = new Point(12, 85);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(456, 241);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = " Message ";
      this.grbInterval.Controls.Add((Control) this.label13);
      this.grbInterval.Controls.Add((Control) this.nudInterval);
      this.grbInterval.Location = new Point(12, 12);
      this.grbInterval.Name = "grbInterval";
      this.grbInterval.Size = new Size(456, 66);
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
      this.ClientSize = new Size(477, 366);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.grbInterval);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FormTxMsgJ1939);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "New Transmit J1939 Message";
      this.nudSource.EndInit();
      this.nudDestination.EndInit();
      this.nudPriority.EndInit();
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
