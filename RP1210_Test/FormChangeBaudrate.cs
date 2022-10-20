
// Type: RP1210_Test.FormChangeBaudrate




using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RP1210_Test
{
  public class FormChangeBaudrate : Form
  {
    private IContainer components;
    private Label label1;
    private RadioButton rdbChangeNow;
    private RadioButton rdbChangeLater;
    private Label label2;
    private ComboBox cbbBaudrates;
    private Label laCurrent;
    private Button btnOK;
    private Button btnCancel;

    public FormChangeBaudrate(uint current = 0)
    {
      this.InitializeComponent();
      this.CurrentBaudrate = current;
      this.InitializeValues();
    }

    private void InitializeValues()
    {
      uint[] numArray = new uint[4]
      {
        125000U,
        250000U,
        500000U,
        1000000U
      };
      this.laCurrent.Text = this.CurrentBaudrate.ToString();
      for (int index = 0; index < numArray.Length; ++index)
      {
        if ((int) numArray[index] != (int) this.CurrentBaudrate)
          this.cbbBaudrates.Items.Add((object) numArray[index]);
      }
      this.cbbBaudrates.SelectedIndex = 0;
      this.rdbChangeNow.Checked = true;
    }

    public uint CurrentBaudrate { get; set; }

    public uint NewBaudrate => uint.Parse(this.cbbBaudrates.Text);

    public bool ChangeNow
    {
      get => this.rdbChangeNow.Checked;
      set => this.rdbChangeNow.Checked = value;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.rdbChangeNow = new RadioButton();
      this.rdbChangeLater = new RadioButton();
      this.label2 = new Label();
      this.cbbBaudrates = new ComboBox();
      this.laCurrent = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(90, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Current Baudrate:";
      this.rdbChangeNow.AutoSize = true;
      this.rdbChangeNow.Location = new Point(13, 72);
      this.rdbChangeNow.Name = "rdbChangeNow";
      this.rdbChangeNow.Size = new Size(120, 17);
      this.rdbChangeNow.TabIndex = 4;
      this.rdbChangeNow.TabStop = true;
      this.rdbChangeNow.Text = "Change Immediately";
      this.rdbChangeNow.UseVisualStyleBackColor = true;
      this.rdbChangeLater.AutoSize = true;
      this.rdbChangeLater.Location = new Point(146, 72);
      this.rdbChangeLater.Name = "rdbChangeLater";
      this.rdbChangeLater.Size = new Size(115, 17);
      this.rdbChangeLater.TabIndex = 5;
      this.rdbChangeLater.TabStop = true;
      this.rdbChangeLater.Text = "Change After Send";
      this.rdbChangeLater.UseVisualStyleBackColor = true;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(14, 37);
      this.label2.Name = "label2";
      this.label2.Size = new Size(92, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Desired Baudrate:";
      this.cbbBaudrates.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbbBaudrates.FormattingEnabled = true;
      this.cbbBaudrates.Location = new Point(112, 34);
      this.cbbBaudrates.Name = "cbbBaudrates";
      this.cbbBaudrates.Size = new Size(121, 21);
      this.cbbBaudrates.TabIndex = 3;
      this.laCurrent.AutoSize = true;
      this.laCurrent.Location = new Point(118, 9);
      this.laCurrent.Name = "laCurrent";
      this.laCurrent.Size = new Size(43, 13);
      this.laCurrent.TabIndex = 1;
      this.laCurrent.Text = "250000";
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(105, 124);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 6;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(186, 124);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(273, 159);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.laCurrent);
      this.Controls.Add((Control) this.cbbBaudrates);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.rdbChangeLater);
      this.Controls.Add((Control) this.rdbChangeNow);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FormChangeBaudrate);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Change Baudrate";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
