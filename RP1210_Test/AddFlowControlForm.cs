
// Type: RP1210_Test.AddFlowControlForm




using Peak.RP1210C;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace RP1210_Test
{
  public class AddFlowControlForm : Form
  {
    private IContainer components;
    private Button btnCancel;
    private Button btnOK;
    private ComboBox cbbType;
    private Label label1;
    private Label label2;
    private NumericUpDown nudBlockSize;
    private GroupBox groupBox1;
    private TextBox txtIncomingEA;
    private Label label4;
    private TextBox txtIncomingID;
    private Label label3;
    private GroupBox groupBox2;
    private TextBox txtOutgoingEA;
    private Label label5;
    private TextBox txtOutgoingID;
    private Label label6;
    private GroupBox groupBox3;
    private TextBox txtSeparationTimeTx;
    private Label label7;
    private TextBox txtSeparationTime;
    private Label label8;

    public AddFlowControlForm()
    {
      this.InitializeComponent();
      this.cbbType.SelectedIndex = 0;
      this.txtIncomingID.Text = "0010";
      this.txtOutgoingID.Text = "0020";
      this.txtIncomingEA.Text = "02";
      this.txtOutgoingEA.Text = "01";
      this.txtSeparationTime.Text = "A";
      this.txtSeparationTimeTx.Text = "A";
    }

    private void cbbType_SelectedIndexChanged(object sender, EventArgs e)
    {
      int selectedIndex = this.cbbType.SelectedIndex;
      this.txtIncomingID.MaxLength = this.txtOutgoingID.MaxLength = selectedIndex == 1 || selectedIndex == 3 ? 8 : 4;
      this.txtIncomingEA.Enabled = this.txtOutgoingEA.Enabled = selectedIndex > 1;
      this.txtIncomingID_Leave((object) this, new EventArgs());
      this.txtOutgoingID_Leave((object) this, new EventArgs());
    }

    private void txtIncomingID_Leave(object sender, EventArgs e) => this.IncomingID = uint.Parse(this.txtIncomingID.Text, NumberStyles.HexNumber);

    private void txtOutgoingID_Leave(object sender, EventArgs e) => this.OutgoingID = uint.Parse(this.txtOutgoingID.Text, NumberStyles.HexNumber);

    private void txtIncomingEA_Leave(object sender, EventArgs e) => this.IncomingExtendedAddress = byte.Parse(this.txtIncomingEA.Text, NumberStyles.HexNumber);

    private void txtOutgoingEA_Leave(object sender, EventArgs e) => this.OutgoingExtendedAddress = byte.Parse(this.txtOutgoingEA.Text, NumberStyles.HexNumber);

    private void txtSeparationTime_Leave(object sender, EventArgs e)
    {
      this.SeparationTime = byte.Parse(this.txtSeparationTime.Text, NumberStyles.HexNumber);
      if ((this.SeparationTime < (byte) 128 || this.SeparationTime > (byte) 240) && (this.SeparationTime < (byte) 250 || this.SeparationTime > byte.MaxValue))
        return;
      int num = (int) MessageBox.Show("The value entered ist reserved. Possible values are in range [0x00,0x7F] and [0xF1,0xF9]", "Out of range", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      this.SeparationTime = (byte) 127;
    }

    private void txtSeparationTimeTx_Leave(object sender, EventArgs e)
    {
      this.SeparationTimeTx = ushort.Parse(this.txtSeparationTimeTx.Text, NumberStyles.HexNumber);
      if ((this.SeparationTimeTx <= (ushort) byte.MaxValue || this.SeparationTimeTx == ushort.MaxValue) && (this.SeparationTimeTx < (ushort) 128 || this.SeparationTimeTx > (ushort) 240) && (this.SeparationTimeTx < (ushort) 250 || this.SeparationTimeTx > (ushort) byte.MaxValue))
        return;
      int num = (int) MessageBox.Show("The value entered ist reserved. Possible values are in range [0x00,0x7F] and [0xF1,0xF9], or 0xFFFF to use the device default value", "Out of range", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      this.SeparationTimeTx = (ushort) sbyte.MaxValue;
    }

    private void EditBoxes_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!(sender is TextBox textBox) || e.KeyChar == '\b')
        return;
      e.Handled = !uint.TryParse(textBox.Text + e.KeyChar.ToString(), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out uint _);
    }

    public RP1210CISO15765MsgType MessageType
    {
      get => (RP1210CISO15765MsgType) this.cbbType.SelectedIndex;
      set => this.cbbType.SelectedIndex = (int) value;
    }

    public byte BlockSize
    {
      get => (byte) this.nudBlockSize.Value;
      set => this.nudBlockSize.Value = (Decimal) value;
    }

    public uint IncomingID
    {
      get => uint.Parse(this.txtIncomingID.Text, NumberStyles.HexNumber);
      set => this.txtIncomingID.Text = value.ToString(string.Format("X{0}", (object) this.txtIncomingID.MaxLength));
    }

    public uint OutgoingID
    {
      get => uint.Parse(this.txtOutgoingID.Text, NumberStyles.HexNumber);
      set => this.txtOutgoingID.Text = value.ToString(string.Format("X{0}", (object) this.txtOutgoingID.MaxLength));
    }

    public byte IncomingExtendedAddress
    {
      get => byte.Parse(this.txtIncomingEA.Text, NumberStyles.HexNumber);
      set => this.txtIncomingEA.Text = value.ToString("X2");
    }

    public byte OutgoingExtendedAddress
    {
      get => byte.Parse(this.txtOutgoingEA.Text, NumberStyles.HexNumber);
      set => this.txtOutgoingEA.Text = value.ToString("X2");
    }

    public byte SeparationTime
    {
      get => byte.Parse(this.txtSeparationTime.Text, NumberStyles.HexNumber);
      set => this.txtSeparationTime.Text = value.ToString("X2");
    }

    public ushort SeparationTimeTx
    {
      get => ushort.Parse(this.txtSeparationTimeTx.Text, NumberStyles.HexNumber);
      set => this.txtSeparationTimeTx.Text = value.ToString("X4");
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
      this.cbbType = new ComboBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.nudBlockSize = new NumericUpDown();
      this.groupBox1 = new GroupBox();
      this.txtIncomingEA = new TextBox();
      this.label4 = new Label();
      this.txtIncomingID = new TextBox();
      this.label3 = new Label();
      this.groupBox2 = new GroupBox();
      this.txtOutgoingEA = new TextBox();
      this.label5 = new Label();
      this.txtOutgoingID = new TextBox();
      this.label6 = new Label();
      this.groupBox3 = new GroupBox();
      this.txtSeparationTimeTx = new TextBox();
      this.label7 = new Label();
      this.txtSeparationTime = new TextBox();
      this.label8 = new Label();
      this.nudBlockSize.BeginInit();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(236, 295);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(155, 295);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.cbbType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbbType.FormattingEnabled = true;
      this.cbbType.Items.AddRange(new object[5]
      {
        (object) "Standard CAN",
        (object) "Extended CAN",
        (object) "Standard CAN ISO15765 Extended",
        (object) "Extended CAN ISO15765 Extended",
        (object) "Standard Mixed CAN ISO15765"
      });
      this.cbbType.Location = new Point(15, 34);
      this.cbbType.Name = "cbbType";
      this.cbbType.Size = new Size(194, 21);
      this.cbbType.TabIndex = 5;
      this.cbbType.SelectedIndexChanged += new EventHandler(this.cbbType_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(34, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Type:";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(234, 9);
      this.label2.Name = "label2";
      this.label2.Size = new Size(58, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "Block size:";
      this.nudBlockSize.Location = new Point(237, 35);
      this.nudBlockSize.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.nudBlockSize.Name = "nudBlockSize";
      this.nudBlockSize.Size = new Size(75, 20);
      this.nudBlockSize.TabIndex = 7;
      this.nudBlockSize.Value = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.groupBox1.Controls.Add((Control) this.txtIncomingEA);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.txtIncomingID);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Location = new Point(12, 71);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(300, 63);
      this.groupBox1.TabIndex = 8;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = " Device side (Incoming) ";
      this.txtIncomingEA.Location = new Point(254, 25);
      this.txtIncomingEA.Name = "txtIncomingEA";
      this.txtIncomingEA.Size = new Size(40, 20);
      this.txtIncomingEA.TabIndex = 3;
      this.txtIncomingEA.Text = "02";
      this.txtIncomingEA.KeyPress += new KeyPressEventHandler(this.EditBoxes_KeyPress);
      this.txtIncomingEA.Leave += new EventHandler(this.txtIncomingEA_Leave);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(152, 28);
      this.label4.Name = "label4";
      this.label4.Size = new Size(96, 13);
      this.label4.TabIndex = 2;
      this.label4.Text = "Extended Address:";
      this.txtIncomingID.Location = new Point(58, 25);
      this.txtIncomingID.Name = "txtIncomingID";
      this.txtIncomingID.Size = new Size(80, 20);
      this.txtIncomingID.TabIndex = 1;
      this.txtIncomingID.Text = "0010";
      this.txtIncomingID.KeyPress += new KeyPressEventHandler(this.EditBoxes_KeyPress);
      this.txtIncomingID.Leave += new EventHandler(this.txtIncomingID_Leave);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 28);
      this.label3.Name = "label3";
      this.label3.Size = new Size(46, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "CAN ID:";
      this.groupBox2.Controls.Add((Control) this.txtOutgoingEA);
      this.groupBox2.Controls.Add((Control) this.label5);
      this.groupBox2.Controls.Add((Control) this.txtOutgoingID);
      this.groupBox2.Controls.Add((Control) this.label6);
      this.groupBox2.Location = new Point(12, 140);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(300, 63);
      this.groupBox2.TabIndex = 9;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = " Tester side (Outgoing) ";
      this.txtOutgoingEA.Location = new Point(254, 25);
      this.txtOutgoingEA.Name = "txtOutgoingEA";
      this.txtOutgoingEA.Size = new Size(40, 20);
      this.txtOutgoingEA.TabIndex = 7;
      this.txtOutgoingEA.Text = "01";
      this.txtOutgoingEA.KeyPress += new KeyPressEventHandler(this.EditBoxes_KeyPress);
      this.txtOutgoingEA.Leave += new EventHandler(this.txtOutgoingEA_Leave);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(152, 28);
      this.label5.Name = "label5";
      this.label5.Size = new Size(96, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "Extended Address:";
      this.txtOutgoingID.Location = new Point(58, 25);
      this.txtOutgoingID.Name = "txtOutgoingID";
      this.txtOutgoingID.Size = new Size(80, 20);
      this.txtOutgoingID.TabIndex = 5;
      this.txtOutgoingID.Text = "0020";
      this.txtOutgoingID.KeyPress += new KeyPressEventHandler(this.EditBoxes_KeyPress);
      this.txtOutgoingID.Leave += new EventHandler(this.txtOutgoingID_Leave);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(6, 28);
      this.label6.Name = "label6";
      this.label6.Size = new Size(46, 13);
      this.label6.TabIndex = 4;
      this.label6.Text = "CAN ID:";
      this.groupBox3.Controls.Add((Control) this.txtSeparationTimeTx);
      this.groupBox3.Controls.Add((Control) this.label7);
      this.groupBox3.Controls.Add((Control) this.txtSeparationTime);
      this.groupBox3.Controls.Add((Control) this.label8);
      this.groupBox3.Location = new Point(11, 209);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(300, 63);
      this.groupBox3.TabIndex = 10;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = " Separation time (hex) ";
      this.txtSeparationTimeTx.Location = new Point(238, 25);
      this.txtSeparationTimeTx.MaxLength = 4;
      this.txtSeparationTimeTx.Name = "txtSeparationTimeTx";
      this.txtSeparationTimeTx.Size = new Size(56, 20);
      this.txtSeparationTimeTx.TabIndex = 7;
      this.txtSeparationTimeTx.Text = "A";
      this.txtSeparationTimeTx.KeyPress += new KeyPressEventHandler(this.EditBoxes_KeyPress);
      this.txtSeparationTimeTx.Leave += new EventHandler(this.txtSeparationTimeTx_Leave);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(150, 28);
      this.label7.Name = "label7";
      this.label7.Size = new Size(84, 13);
      this.label7.TabIndex = 6;
      this.label7.Text = "On transmission:";
      this.txtSeparationTime.Location = new Point(83, 25);
      this.txtSeparationTime.MaxLength = 2;
      this.txtSeparationTime.Name = "txtSeparationTime";
      this.txtSeparationTime.Size = new Size(56, 20);
      this.txtSeparationTime.TabIndex = 5;
      this.txtSeparationTime.Text = "A";
      this.txtSeparationTime.KeyPress += new KeyPressEventHandler(this.EditBoxes_KeyPress);
      this.txtSeparationTime.Leave += new EventHandler(this.txtSeparationTime_Leave);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(6, 28);
      this.label8.Name = "label8";
      this.label8.Size = new Size(71, 13);
      this.label8.TabIndex = 4;
      this.label8.Text = "On reception:";
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(323, 330);
      this.ControlBox = false;
      this.Controls.Add((Control) this.groupBox3);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.nudBlockSize);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.cbbType);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddFlowControlForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add ISO15765 flow control...";
      this.nudBlockSize.EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
