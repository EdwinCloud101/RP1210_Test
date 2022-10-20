
// Type: RP1210_Test.FormCustomFilterCan




using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace RP1210_Test
{
  public class FormCustomFilterCan : Form
  {
    private IContainer components;
    private Button btnOK;
    private Button btnCancel;
    private Label label1;
    private ComboBox cbbType;
    private Label label2;
    private TextBox txtMask;
    private TextBox txtHeader;
    private Label label3;

    public FormCustomFilterCan()
    {
      this.InitializeComponent();
      this.cbbType.SelectedIndex = 0;
      this.txtMask.Text = this.txtHeader.Text = "0000";
    }

    private void cbbType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.txtMask.MaxLength = this.txtHeader.MaxLength = this.cbbType.SelectedIndex == 1 ? 8 : 4;
      this.txtHeader_Leave((object) this, new EventArgs());
      this.txtMask_Leave((object) this, new EventArgs());
    }

    private void txtMask_Leave(object sender, EventArgs e) => this.Mask = uint.Parse(this.txtMask.Text, NumberStyles.HexNumber);

    private void txtHeader_Leave(object sender, EventArgs e) => this.Header = uint.Parse(this.txtHeader.Text, NumberStyles.HexNumber);

    private void txtMaskHeader_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!(sender is TextBox textBox) || e.KeyChar == '\b')
        return;
      e.Handled = !uint.TryParse(textBox.Text + e.KeyChar.ToString(), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out uint _);
    }

    public uint Mask
    {
      get => uint.Parse(this.txtMask.Text, NumberStyles.HexNumber);
      set => this.txtMask.Text = value.ToString("X" + (this.Extended ? "8" : "4"));
    }

    public uint Header
    {
      get => uint.Parse(this.txtHeader.Text, NumberStyles.HexNumber);
      set => this.txtHeader.Text = value.ToString("X" + (this.Extended ? "8" : "4"));
    }

    public bool Extended
    {
      get => this.cbbType.SelectedIndex > 0;
      set => this.cbbType.SelectedIndex = value ? 1 : 0;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.label1 = new Label();
      this.cbbType = new ComboBox();
      this.label2 = new Label();
      this.txtMask = new TextBox();
      this.txtHeader = new TextBox();
      this.label3 = new Label();
      this.SuspendLayout();
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(173, 70);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 6;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(254, 70);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(34, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Type:";
      this.cbbType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbbType.FormattingEnabled = true;
      this.cbbType.Items.AddRange(new object[2]
      {
        (object) "Standard",
        (object) "Extended"
      });
      this.cbbType.Location = new Point(15, 34);
      this.cbbType.Name = "cbbType";
      this.cbbType.Size = new Size(100, 21);
      this.cbbType.TabIndex = 1;
      this.cbbType.SelectedIndexChanged += new EventHandler(this.cbbType_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(121, 9);
      this.label2.Name = "label2";
      this.label2.Size = new Size(62, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Mask (hex):";
      this.txtMask.CharacterCasing = CharacterCasing.Upper;
      this.txtMask.Location = new Point(124, 34);
      this.txtMask.MaxLength = 4;
      this.txtMask.Name = "txtMask";
      this.txtMask.Size = new Size(100, 20);
      this.txtMask.TabIndex = 3;
      this.txtMask.Text = "0000";
      this.txtMask.KeyPress += new KeyPressEventHandler(this.txtMaskHeader_KeyPress);
      this.txtMask.Leave += new EventHandler(this.txtMask_Leave);
      this.txtHeader.CharacterCasing = CharacterCasing.Upper;
      this.txtHeader.Location = new Point(230, 34);
      this.txtHeader.MaxLength = 4;
      this.txtHeader.Name = "txtHeader";
      this.txtHeader.Size = new Size(100, 20);
      this.txtHeader.TabIndex = 5;
      this.txtHeader.Text = "0000";
      this.txtHeader.KeyPress += new KeyPressEventHandler(this.txtMaskHeader_KeyPress);
      this.txtHeader.Leave += new EventHandler(this.txtHeader_Leave);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(227, 9);
      this.label3.Name = "label3";
      this.label3.Size = new Size(71, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Header (hex):";
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(341, 108);
      this.ControlBox = false;
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
      this.Name = nameof (FormCustomFilterCan);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Custom CAN Filter...";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
