
// Type: RP1210_Test.FormCustomFilterISO15765




using Peak.RP1210C;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace RP1210_Test
{
  public class FormCustomFilterISO15765 : Form
  {
    private IContainer components;
    private TextBox txtHeader;
    private Label label3;
    private TextBox txtMask;
    private Label label2;
    private ComboBox cbbType;
    private Label label1;
    private Button btnCancel;
    private Button btnOK;
    private Label label4;
    private Label label5;
    private TextBox txtAddrHeader;
    private TextBox txtAddrMask;

    public FormCustomFilterISO15765()
    {
      this.InitializeComponent();
      this.cbbType.SelectedIndex = 0;
      this.txtMask.Text = this.txtHeader.Text = "0000";
      this.txtAddrMask.Text = this.txtAddrHeader.Text = "00";
    }

    private void txtMaskHeader_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!(sender is TextBox textBox) || e.KeyChar == '\b')
        return;
      e.Handled = !uint.TryParse(textBox.Text + e.KeyChar.ToString(), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out uint _);
    }

    private void txtMask_Leave(object sender, EventArgs e) => this.Mask = uint.Parse(this.txtMask.Text, NumberStyles.HexNumber);

    private void txtHeader_Leave(object sender, EventArgs e) => this.Header = uint.Parse(this.txtHeader.Text, NumberStyles.HexNumber);

    private void txtAddrMaskHeader_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!(sender is TextBox textBox) || e.KeyChar == '\b')
        return;
      e.Handled = !byte.TryParse(textBox.Text + e.KeyChar.ToString(), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out byte _);
    }

    private void txtAddrMask_Leave(object sender, EventArgs e) => this.ExtendedAddressMask = byte.Parse(this.txtAddrMask.Text, NumberStyles.HexNumber);

    private void txtAddrHeader_Leave(object sender, EventArgs e) => this.ExtendedAddressHeader = byte.Parse(this.txtAddrHeader.Text, NumberStyles.HexNumber);

    private void cbbType_SelectedIndexChanged(object sender, EventArgs e)
    {
      int selectedIndex = this.cbbType.SelectedIndex;
      this.txtMask.MaxLength = this.txtHeader.MaxLength = selectedIndex == 1 || selectedIndex == 3 ? 8 : 4;
      this.txtAddrHeader.Enabled = selectedIndex > 1;
      this.txtAddrMask.Enabled = selectedIndex > 1;
      this.txtHeader_Leave((object) this, new EventArgs());
      this.txtMask_Leave((object) this, new EventArgs());
    }

    public RP1210CISO15765MsgType MessageType
    {
      get => (RP1210CISO15765MsgType) this.cbbType.SelectedIndex;
      set => this.cbbType.SelectedIndex = (int) value;
    }

    public uint Mask
    {
      get => uint.Parse(this.txtMask.Text, NumberStyles.HexNumber);
      set => this.txtMask.Text = value.ToString(string.Format("X{0}", (object) this.txtMask.MaxLength));
    }

    public uint Header
    {
      get => uint.Parse(this.txtHeader.Text, NumberStyles.HexNumber);
      set => this.txtHeader.Text = value.ToString(string.Format("X{0}", (object) this.txtHeader.MaxLength));
    }

    public byte ExtendedAddressMask
    {
      get => byte.Parse(this.txtAddrMask.Text, NumberStyles.HexNumber);
      set => this.txtAddrMask.Text = value.ToString("X2");
    }

    public byte ExtendedAddressHeader
    {
      get => byte.Parse(this.txtAddrHeader.Text, NumberStyles.HexNumber);
      set => this.txtAddrHeader.Text = value.ToString("X2");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtHeader = new TextBox();
      this.label3 = new Label();
      this.txtMask = new TextBox();
      this.label2 = new Label();
      this.cbbType = new ComboBox();
      this.label1 = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.label4 = new Label();
      this.label5 = new Label();
      this.txtAddrHeader = new TextBox();
      this.txtAddrMask = new TextBox();
      this.SuspendLayout();
      this.txtHeader.CharacterCasing = CharacterCasing.Upper;
      this.txtHeader.Location = new Point(323, 34);
      this.txtHeader.MaxLength = 4;
      this.txtHeader.Name = "txtHeader";
      this.txtHeader.Size = new Size(100, 20);
      this.txtHeader.TabIndex = 5;
      this.txtHeader.Text = "0000";
      this.txtHeader.KeyPress += new KeyPressEventHandler(this.txtMaskHeader_KeyPress);
      this.txtHeader.Leave += new EventHandler(this.txtHeader_Leave);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(320, 9);
      this.label3.Name = "label3";
      this.label3.Size = new Size(71, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Header (hex):";
      this.txtMask.CharacterCasing = CharacterCasing.Upper;
      this.txtMask.Location = new Point(217, 34);
      this.txtMask.MaxLength = 4;
      this.txtMask.Name = "txtMask";
      this.txtMask.Size = new Size(100, 20);
      this.txtMask.TabIndex = 3;
      this.txtMask.Text = "0000";
      this.txtMask.KeyPress += new KeyPressEventHandler(this.txtMaskHeader_KeyPress);
      this.txtMask.Leave += new EventHandler(this.txtMask_Leave);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(214, 9);
      this.label2.Name = "label2";
      this.label2.Size = new Size(62, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Mask (hex):";
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
      this.cbbType.Size = new Size(184, 21);
      this.cbbType.TabIndex = 1;
      this.cbbType.SelectedIndexChanged += new EventHandler(this.cbbType_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(34, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Type:";
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(348, (int) sbyte.MaxValue);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 11;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(267, (int) sbyte.MaxValue);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 10;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(320, 68);
      this.label4.Name = "label4";
      this.label4.Size = new Size(117, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "Ext. Addr Header (hex):";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(214, 68);
      this.label5.Name = "label5";
      this.label5.Size = new Size(108, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "Ext. Addr Mask (hex):";
      this.txtAddrHeader.CharacterCasing = CharacterCasing.Upper;
      this.txtAddrHeader.Location = new Point(323, 95);
      this.txtAddrHeader.MaxLength = 2;
      this.txtAddrHeader.Name = "txtAddrHeader";
      this.txtAddrHeader.Size = new Size(100, 20);
      this.txtAddrHeader.TabIndex = 9;
      this.txtAddrHeader.Text = "00";
      this.txtAddrHeader.KeyPress += new KeyPressEventHandler(this.txtAddrMaskHeader_KeyPress);
      this.txtAddrHeader.Leave += new EventHandler(this.txtAddrHeader_Leave);
      this.txtAddrMask.CharacterCasing = CharacterCasing.Upper;
      this.txtAddrMask.Location = new Point(217, 95);
      this.txtAddrMask.MaxLength = 2;
      this.txtAddrMask.Name = "txtAddrMask";
      this.txtAddrMask.Size = new Size(100, 20);
      this.txtAddrMask.TabIndex = 7;
      this.txtAddrMask.Text = "00";
      this.txtAddrMask.KeyPress += new KeyPressEventHandler(this.txtAddrMaskHeader_KeyPress);
      this.txtAddrMask.Leave += new EventHandler(this.txtAddrMask_Leave);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(435, 162);
      this.ControlBox = false;
      this.Controls.Add((Control) this.txtAddrHeader);
      this.Controls.Add((Control) this.txtAddrMask);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.txtHeader);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.txtMask);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.cbbType);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FormCustomFilterISO15765);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Custom ISO15765 Filter...";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
