
// Type: RP1210_Test.AddressClaimingForm




using RP1210_Test.HelpClasses;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace RP1210_Test
{
  public class AddressClaimingForm : Form
  {
    private RP1210J1939Network m_Network;
    private RP1210J1939Name m_Name;
    private byte m_Addr;
    private IContainer components;
    private GroupBox groupBox1;
    private TextBox txtName64;
    private Label label1;
    private GroupBox groupBox2;
    private TextBox txtIN;
    private ComboBox cbbAAC;
    private NumericUpDown nudIG;
    private NumericUpDown nudVSI;
    private NumericUpDown nudVS;
    private NumericUpDown nudFunction;
    private NumericUpDown nudFI;
    private NumericUpDown nudECU;
    private NumericUpDown nudMC;
    private Label label10;
    private Label label6;
    private Label label7;
    private Label label8;
    private Label label9;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label2;
    private GroupBox groupBox3;
    private NumericUpDown nudAddr;
    private Label label11;
    private Button btnOK;
    private Button btnCancel;

    public AddressClaimingForm()
      : this(new RP1210J1939Network((byte) 249))
    {
    }

    public AddressClaimingForm(RP1210J1939Network network)
    {
      this.InitializeComponent();
      this.Network = network;
      this.RefreshAddress();
      this.RefreshValues();
    }

    private void RefreshAddress() => this.nudAddr.Value = (Decimal) this.m_Addr;

    private void Refresh64BitValue() => this.txtName64.Text = this.m_Name.AsNumber.ToString("X16");

    private void RefreshValues()
    {
      this.Refresh64BitValue();
      this.cbbAAC.SelectedIndex = this.m_Name.ArbitraryAddressCapable ? 1 : 0;
      this.nudIG.Value = (Decimal) this.m_Name.IndustryGroup;
      this.nudVSI.Value = (Decimal) this.m_Name.VehicleSystemInstance;
      this.nudVS.Value = (Decimal) this.m_Name.VehicleSystem;
      this.nudFunction.Value = (Decimal) this.m_Name.Function;
      this.nudFI.Value = (Decimal) this.m_Name.FunctionInstance;
      this.nudECU.Value = (Decimal) this.m_Name.ECUInstance;
      this.nudMC.Value = (Decimal) this.m_Name.ManufactureCode;
      this.txtIN.Text = this.m_Name.IdentifyNumber.ToString();
    }

    private void txtName64_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!(sender is TextBox textBox) || e.KeyChar == '\b')
        return;
      e.Handled = !ulong.TryParse(textBox.Text + e.KeyChar.ToString(), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out ulong _);
    }

    private void txtName64_Leave(object sender, EventArgs e)
    {
      this.m_Name.AsNumber = Convert.ToUInt64(this.txtName64.Text, 16);
      this.RefreshValues();
    }

    private void nudIG_ValueChanged(object sender, EventArgs e)
    {
      if (!(sender is NumericUpDown numericUpDown))
        return;
      switch (Convert.ToInt32(numericUpDown.Tag))
      {
        case 1:
          this.m_Name.IndustryGroup = (byte) numericUpDown.Value;
          break;
        case 2:
          this.m_Name.VehicleSystemInstance = (byte) numericUpDown.Value;
          break;
        case 3:
          this.m_Name.VehicleSystem = (byte) numericUpDown.Value;
          break;
        case 4:
          this.m_Name.Function = (byte) numericUpDown.Value;
          break;
        case 5:
          this.m_Name.FunctionInstance = (byte) numericUpDown.Value;
          break;
        case 6:
          this.m_Name.ECUInstance = (byte) numericUpDown.Value;
          break;
        case 7:
          this.m_Name.ManufactureCode = (ushort) numericUpDown.Value;
          break;
      }
      this.Refresh64BitValue();
    }

    private void cbbAAC_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.m_Name.ArbitraryAddressCapable = this.cbbAAC.SelectedIndex > 0;
      this.Refresh64BitValue();
    }

    private void txtIN_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!(sender is TextBox textBox) || e.KeyChar == '\b')
        return;
      e.Handled = !uint.TryParse(textBox.Text + e.KeyChar.ToString(), out uint _);
    }

    private void txtIN_Leave(object sender, EventArgs e)
    {
      uint uint32 = Convert.ToUInt32(this.txtIN.Text);
      if (uint32 > 2097151U)
        this.txtIN.Text = 2097151.ToString();
      this.m_Name.IdentifyNumber = uint32;
      this.Refresh64BitValue();
    }

    private void nudAddr_ValueChanged(object sender, EventArgs e) => this.m_Addr = (byte) this.nudAddr.Value;

    public RP1210J1939Network Network
    {
      get
      {
        this.m_Network.Name = this.m_Name;
        this.m_Network.Address = this.m_Addr;
        return new RP1210J1939Network(this.m_Network);
      }
      set
      {
        this.m_Network = new RP1210J1939Network(value);
        this.m_Name = this.m_Network.Name;
        this.m_Addr = this.m_Network.Address;
      }
    }

    public byte Address
    {
      get => this.m_Network.Address;
      set => this.m_Network.Address = value;
    }

    public RP1210J1939Name J1939Name
    {
      get
      {
        this.m_Network.Name = this.m_Name;
        return this.m_Network.Name;
      }
      set
      {
        this.m_Network.Name = new RP1210J1939Name(value);
        this.m_Name = new RP1210J1939Name(value);
        this.RefreshValues();
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupBox1 = new GroupBox();
      this.txtName64 = new TextBox();
      this.label1 = new Label();
      this.groupBox2 = new GroupBox();
      this.txtIN = new TextBox();
      this.cbbAAC = new ComboBox();
      this.nudIG = new NumericUpDown();
      this.nudVSI = new NumericUpDown();
      this.nudVS = new NumericUpDown();
      this.nudFunction = new NumericUpDown();
      this.nudFI = new NumericUpDown();
      this.nudECU = new NumericUpDown();
      this.nudMC = new NumericUpDown();
      this.label10 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.label9 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.groupBox3 = new GroupBox();
      this.nudAddr = new NumericUpDown();
      this.label11 = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.nudIG.BeginInit();
      this.nudVSI.BeginInit();
      this.nudVS.BeginInit();
      this.nudFunction.BeginInit();
      this.nudFI.BeginInit();
      this.nudECU.BeginInit();
      this.nudMC.BeginInit();
      this.groupBox3.SuspendLayout();
      this.nudAddr.BeginInit();
      this.SuspendLayout();
      this.groupBox1.Controls.Add((Control) this.txtName64);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Location = new Point(12, 77);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(290, 60);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Name as 64-Bit value";
      this.txtName64.Location = new Point(106, 23);
      this.txtName64.MaxLength = 16;
      this.txtName64.Name = "txtName64";
      this.txtName64.Size = new Size(178, 20);
      this.txtName64.TabIndex = 1;
      this.txtName64.KeyPress += new KeyPressEventHandler(this.txtName64_KeyPress);
      this.txtName64.Leave += new EventHandler(this.txtName64_Leave);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 26);
      this.label1.Name = "label1";
      this.label1.Size = new Size(94, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "64 Bit Name (hex):";
      this.groupBox2.Controls.Add((Control) this.txtIN);
      this.groupBox2.Controls.Add((Control) this.cbbAAC);
      this.groupBox2.Controls.Add((Control) this.nudIG);
      this.groupBox2.Controls.Add((Control) this.nudVSI);
      this.groupBox2.Controls.Add((Control) this.nudVS);
      this.groupBox2.Controls.Add((Control) this.nudFunction);
      this.groupBox2.Controls.Add((Control) this.nudFI);
      this.groupBox2.Controls.Add((Control) this.nudECU);
      this.groupBox2.Controls.Add((Control) this.nudMC);
      this.groupBox2.Controls.Add((Control) this.label10);
      this.groupBox2.Controls.Add((Control) this.label6);
      this.groupBox2.Controls.Add((Control) this.label7);
      this.groupBox2.Controls.Add((Control) this.label8);
      this.groupBox2.Controls.Add((Control) this.label9);
      this.groupBox2.Controls.Add((Control) this.label5);
      this.groupBox2.Controls.Add((Control) this.label4);
      this.groupBox2.Controls.Add((Control) this.label3);
      this.groupBox2.Controls.Add((Control) this.label2);
      this.groupBox2.Location = new Point(12, 143);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(290, 235);
      this.groupBox2.TabIndex = 2;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Name as Parameters";
      this.txtIN.Location = new Point(205, 204);
      this.txtIN.MaxLength = 7;
      this.txtIN.Name = "txtIN";
      this.txtIN.Size = new Size(79, 20);
      this.txtIN.TabIndex = 17;
      this.txtIN.Text = "0";
      this.txtIN.KeyPress += new KeyPressEventHandler(this.txtIN_KeyPress);
      this.txtIN.Leave += new EventHandler(this.txtIN_Leave);
      this.cbbAAC.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbbAAC.FormattingEnabled = true;
      this.cbbAAC.Items.AddRange(new object[2]
      {
        (object) "NO",
        (object) "YES"
      });
      this.cbbAAC.Location = new Point(205, 23);
      this.cbbAAC.Name = "cbbAAC";
      this.cbbAAC.Size = new Size(79, 21);
      this.cbbAAC.TabIndex = 1;
      this.cbbAAC.SelectedIndexChanged += new EventHandler(this.cbbAAC_SelectedIndexChanged);
      this.nudIG.Location = new Point(205, 46);
      this.nudIG.Maximum = new Decimal(new int[4]
      {
        7,
        0,
        0,
        0
      });
      this.nudIG.Name = "nudIG";
      this.nudIG.Size = new Size(79, 20);
      this.nudIG.TabIndex = 3;
      this.nudIG.Tag = (object) "1";
      this.nudIG.ValueChanged += new EventHandler(this.nudIG_ValueChanged);
      this.nudVSI.Location = new Point(205, 68);
      this.nudVSI.Maximum = new Decimal(new int[4]
      {
        15,
        0,
        0,
        0
      });
      this.nudVSI.Name = "nudVSI";
      this.nudVSI.Size = new Size(79, 20);
      this.nudVSI.TabIndex = 5;
      this.nudVSI.Tag = (object) "2";
      this.nudVSI.ValueChanged += new EventHandler(this.nudIG_ValueChanged);
      this.nudVS.Location = new Point(205, 91);
      this.nudVS.Maximum = new Decimal(new int[4]
      {
        (int) sbyte.MaxValue,
        0,
        0,
        0
      });
      this.nudVS.Name = "nudVS";
      this.nudVS.Size = new Size(79, 20);
      this.nudVS.TabIndex = 7;
      this.nudVS.Tag = (object) "3";
      this.nudVS.ValueChanged += new EventHandler(this.nudIG_ValueChanged);
      this.nudFunction.Location = new Point(205, 113);
      this.nudFunction.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.nudFunction.Name = "nudFunction";
      this.nudFunction.Size = new Size(79, 20);
      this.nudFunction.TabIndex = 9;
      this.nudFunction.Tag = (object) "4";
      this.nudFunction.ValueChanged += new EventHandler(this.nudIG_ValueChanged);
      this.nudFI.Location = new Point(205, 135);
      this.nudFI.Maximum = new Decimal(new int[4]
      {
        31,
        0,
        0,
        0
      });
      this.nudFI.Name = "nudFI";
      this.nudFI.Size = new Size(79, 20);
      this.nudFI.TabIndex = 11;
      this.nudFI.Tag = (object) "5";
      this.nudFI.ValueChanged += new EventHandler(this.nudIG_ValueChanged);
      this.nudECU.Location = new Point(205, 157);
      this.nudECU.Maximum = new Decimal(new int[4]
      {
        7,
        0,
        0,
        0
      });
      this.nudECU.Name = "nudECU";
      this.nudECU.Size = new Size(79, 20);
      this.nudECU.TabIndex = 13;
      this.nudECU.Tag = (object) "6";
      this.nudECU.ValueChanged += new EventHandler(this.nudIG_ValueChanged);
      this.nudMC.Location = new Point(205, 180);
      this.nudMC.Maximum = new Decimal(new int[4]
      {
        2047,
        0,
        0,
        0
      });
      this.nudMC.Name = "nudMC";
      this.nudMC.Size = new Size(79, 20);
      this.nudMC.TabIndex = 15;
      this.nudMC.Tag = (object) "7";
      this.nudMC.ValueChanged += new EventHandler(this.nudIG_ValueChanged);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(6, 207);
      this.label10.Name = "label10";
      this.label10.Size = new Size(84, 13);
      this.label10.TabIndex = 16;
      this.label10.Text = "Identity Number:";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(6, 182);
      this.label6.Name = "label6";
      this.label6.Size = new Size(98, 13);
      this.label6.TabIndex = 14;
      this.label6.Text = "Manufacture Code:";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(6, 159);
      this.label7.Name = "label7";
      this.label7.Size = new Size(76, 13);
      this.label7.TabIndex = 12;
      this.label7.Text = "ECU Instance:";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(6, 137);
      this.label8.Name = "label8";
      this.label8.Size = new Size(95, 13);
      this.label8.TabIndex = 10;
      this.label8.Text = "Function Instance:";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(6, 115);
      this.label9.Name = "label9";
      this.label9.Size = new Size(51, 13);
      this.label9.TabIndex = 8;
      this.label9.Text = "Function:";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(6, 93);
      this.label5.Name = "label5";
      this.label5.Size = new Size(82, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "Vehicle System:";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 70);
      this.label4.Name = "label4";
      this.label4.Size = new Size(126, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Vehicle System Instance:";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 48);
      this.label3.Name = "label3";
      this.label3.Size = new Size(79, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Industry Group:";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 26);
      this.label2.Name = "label2";
      this.label2.Size = new Size(131, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Arbitrary Address Capable:";
      this.groupBox3.Controls.Add((Control) this.nudAddr);
      this.groupBox3.Controls.Add((Control) this.label11);
      this.groupBox3.Location = new Point(12, 11);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(290, 60);
      this.groupBox3.TabIndex = 0;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Address";
      this.nudAddr.Location = new Point(205, 24);
      this.nudAddr.Maximum = new Decimal(new int[4]
      {
        253,
        0,
        0,
        0
      });
      this.nudAddr.Name = "nudAddr";
      this.nudAddr.Size = new Size(79, 20);
      this.nudAddr.TabIndex = 1;
      this.nudAddr.Value = new Decimal(new int[4]
      {
        249,
        0,
        0,
        0
      });
      this.nudAddr.ValueChanged += new EventHandler(this.nudAddr_ValueChanged);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(6, 26);
      this.label11.Name = "label11";
      this.label11.Size = new Size(87, 13);
      this.label11.TabIndex = 0;
      this.label11.Text = "Address to claim:";
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(146, 384);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(227, 384);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(314, 420);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.groupBox3);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.groupBox1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddressClaimingForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "J1939 Network Management";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.nudIG.EndInit();
      this.nudVSI.EndInit();
      this.nudVS.EndInit();
      this.nudFunction.EndInit();
      this.nudFI.EndInit();
      this.nudECU.EndInit();
      this.nudMC.EndInit();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.nudAddr.EndInit();
      this.ResumeLayout(false);
    }
  }
}
