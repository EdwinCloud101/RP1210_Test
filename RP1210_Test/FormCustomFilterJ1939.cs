
// Type: RP1210_Test.FormCustomFilterJ1939




using Peak.RP1210C;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RP1210_Test
{
  public class FormCustomFilterJ1939 : Form
  {
    private IContainer components;
    private Button btnCancel;
    private Button btnOK;
    private Label label1;
    private CheckBox chbPGN;
    private CheckBox chbPriority;
    private CheckBox chbSA;
    private CheckBox chbDA;
    private NumericUpDown nudPriority;
    private ComboBox cbbSA;
    private ComboBox cbbDA;
    private TextBox txtPGN;
    private Label label2;

    public FormCustomFilterJ1939()
    {
      this.InitializeComponent();
      for (int index = 0; index < 256; ++index)
      {
        if (index != 254)
        {
          if (index != (int) byte.MaxValue)
            this.cbbSA.Items.Add((object) index);
          this.cbbDA.Items.Add((object) index);
        }
      }
      this.cbbSA.SelectedIndex = 249;
      this.cbbDA.SelectedIndex = 254;
    }

    private void txtPGN_Leave(object sender, EventArgs e) => this.PGN = uint.Parse(this.txtPGN.Text);

    private void txtPGN_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!(sender is TextBox textBox) || e.KeyChar == '\b')
        return;
      e.Handled = !uint.TryParse(textBox.Text + e.KeyChar.ToString(), out uint _);
    }

    private void chbPGN_CheckedChanged(object sender, EventArgs e) => this.txtPGN.Enabled = this.chbPGN.Checked;

    private void chbPriority_CheckedChanged(object sender, EventArgs e) => this.nudPriority.Enabled = this.chbPriority.Checked;

    private void chbSA_CheckedChanged(object sender, EventArgs e) => this.cbbSA.Enabled = this.chbSA.Checked;

    private void chbDA_CheckedChanged(object sender, EventArgs e) => this.cbbDA.Enabled = this.chbDA.Checked;

    private RP1210CJ1939Filter GetFalgs() => (RP1210CJ1939Filter) (0 | (this.chbPGN.Checked ? 1 : 0) | (this.chbPriority.Checked ? 2 : 0) | (this.chbSA.Checked ? 4 : 0) | (this.chbDA.Checked ? 8 : 0));

    private void SetFalgs(RP1210CJ1939Filter value)
    {
      this.chbPGN.Checked = (value & RP1210CJ1939Filter.FILTER_PGN) == RP1210CJ1939Filter.FILTER_PGN;
      this.chbPriority.Checked = (value & RP1210CJ1939Filter.FILTER_PRIORITY) == RP1210CJ1939Filter.FILTER_PRIORITY;
      this.chbSA.Checked = (value & RP1210CJ1939Filter.FILTER_SOURCE) == RP1210CJ1939Filter.FILTER_SOURCE;
      this.chbDA.Checked = (value & RP1210CJ1939Filter.FILTER_DESTINATION) == RP1210CJ1939Filter.FILTER_DESTINATION;
    }

    public uint PGN
    {
      get => uint.Parse(this.txtPGN.Text);
      set
      {
        if (value > 262143U)
          value = 262143U;
        this.txtPGN.Text = value.ToString();
      }
    }

    public byte Priority
    {
      get => (byte) this.nudPriority.Value;
      set
      {
        if (value > (byte) 7)
          value = (byte) 7;
        this.nudPriority.Value = (Decimal) value;
      }
    }

    public byte SourceAddress
    {
      get => byte.Parse(this.cbbSA.Text);
      set
      {
        if (value == byte.MaxValue || value == (byte) 254)
          value = (byte) 253;
        this.cbbSA.SelectedIndex = (int) value;
      }
    }

    public byte DestinationAddress
    {
      get => byte.Parse(this.cbbDA.Text);
      set => this.cbbDA.SelectedIndex = value >= (byte) 254 ? 254 : (int) value;
    }

    public RP1210CJ1939Filter Falgs
    {
      get => this.GetFalgs();
      set => this.SetFalgs(value);
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
      this.label1 = new Label();
      this.chbPGN = new CheckBox();
      this.chbPriority = new CheckBox();
      this.chbSA = new CheckBox();
      this.chbDA = new CheckBox();
      this.nudPriority = new NumericUpDown();
      this.cbbSA = new ComboBox();
      this.cbbDA = new ComboBox();
      this.txtPGN = new TextBox();
      this.label2 = new Label();
      this.nudPriority.BeginInit();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(166, 153);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 11;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(85, 153);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 10;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(35, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Flags:";
      this.chbPGN.AutoSize = true;
      this.chbPGN.Location = new Point(15, 35);
      this.chbPGN.Name = "chbPGN";
      this.chbPGN.Size = new Size(49, 17);
      this.chbPGN.TabIndex = 2;
      this.chbPGN.Text = "PGN";
      this.chbPGN.UseVisualStyleBackColor = true;
      this.chbPGN.CheckedChanged += new EventHandler(this.chbPGN_CheckedChanged);
      this.chbPriority.AutoSize = true;
      this.chbPriority.Location = new Point(15, 60);
      this.chbPriority.Name = "chbPriority";
      this.chbPriority.Size = new Size(57, 17);
      this.chbPriority.TabIndex = 4;
      this.chbPriority.Text = "Priority";
      this.chbPriority.UseVisualStyleBackColor = true;
      this.chbPriority.CheckedChanged += new EventHandler(this.chbPriority_CheckedChanged);
      this.chbSA.AutoSize = true;
      this.chbSA.Location = new Point(15, 87);
      this.chbSA.Name = "chbSA";
      this.chbSA.Size = new Size(60, 17);
      this.chbSA.TabIndex = 6;
      this.chbSA.Text = "Source";
      this.chbSA.UseVisualStyleBackColor = true;
      this.chbSA.CheckedChanged += new EventHandler(this.chbSA_CheckedChanged);
      this.chbDA.AutoSize = true;
      this.chbDA.Location = new Point(15, 114);
      this.chbDA.Name = "chbDA";
      this.chbDA.Size = new Size(79, 17);
      this.chbDA.TabIndex = 8;
      this.chbDA.Text = "Destination";
      this.chbDA.UseVisualStyleBackColor = true;
      this.chbDA.CheckedChanged += new EventHandler(this.chbDA_CheckedChanged);
      this.nudPriority.Enabled = false;
      this.nudPriority.Location = new Point(121, 59);
      this.nudPriority.Maximum = new Decimal(new int[4]
      {
        7,
        0,
        0,
        0
      });
      this.nudPriority.Name = "nudPriority";
      this.nudPriority.Size = new Size(120, 20);
      this.nudPriority.TabIndex = 5;
      this.cbbSA.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbbSA.Enabled = false;
      this.cbbSA.FormattingEnabled = true;
      this.cbbSA.Location = new Point(121, 85);
      this.cbbSA.Name = "cbbSA";
      this.cbbSA.Size = new Size(121, 21);
      this.cbbSA.TabIndex = 7;
      this.cbbDA.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbbDA.Enabled = false;
      this.cbbDA.FormattingEnabled = true;
      this.cbbDA.Location = new Point(121, 112);
      this.cbbDA.Name = "cbbDA";
      this.cbbDA.Size = new Size(121, 21);
      this.cbbDA.TabIndex = 9;
      this.txtPGN.Enabled = false;
      this.txtPGN.Location = new Point(121, 33);
      this.txtPGN.Name = "txtPGN";
      this.txtPGN.Size = new Size(120, 20);
      this.txtPGN.TabIndex = 3;
      this.txtPGN.Text = "0";
      this.txtPGN.KeyPress += new KeyPressEventHandler(this.txtPGN_KeyPress);
      this.txtPGN.Leave += new EventHandler(this.txtPGN_Leave);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(118, 9);
      this.label2.Name = "label2";
      this.label2.Size = new Size(69, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Values (dec):";
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size((int) byte.MaxValue, 188);
      this.ControlBox = false;
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtPGN);
      this.Controls.Add((Control) this.cbbDA);
      this.Controls.Add((Control) this.cbbSA);
      this.Controls.Add((Control) this.nudPriority);
      this.Controls.Add((Control) this.chbDA);
      this.Controls.Add((Control) this.chbSA);
      this.Controls.Add((Control) this.chbPriority);
      this.Controls.Add((Control) this.chbPGN);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FormCustomFilterJ1939);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Custom J1939 Filter...";
      this.nudPriority.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
