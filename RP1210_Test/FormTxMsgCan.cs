

// Type: RP1210_Test.FormTxMsgCan




using RP1210_Test.HelpClasses;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace RP1210_Test
{
    public class FormTxMsgCan : Form
    {
        private byte[] m_Data;
        private IContainer components;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtID;
        private NumericUpDown nudDLC;
        private TextBox txtD0;
        private TextBox txtD1;
        private TextBox txtD2;
        private TextBox txtD3;
        private TextBox txtD7;
        private TextBox txtD6;
        private TextBox txtD5;
        private TextBox txtD4;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private RadioButton rdbStd;
        private Label label12;
        private RadioButton rdbExt;
        private Button btnOK;
        private Button btnCancel;
        private Button btnRandom;
        private GroupBox groupBox1;
        private GroupBox grbInterval;
        private Label label13;
        private NumericUpDown nudInterval;

        public FormTxMsgCan()
          : this(false, new RP1210_MsgCan(0U, false, new byte[0]))
        {
        }

        public FormTxMsgCan(bool isBroadcast)
          : this(isBroadcast, new RP1210_MsgCan(0U, false, new byte[0]))
        {
        }

        public FormTxMsgCan(bool isBroadcast, RP1210_MsgCan msg)
        {
            this.InitializeComponent();
            this.Message = msg;
            this.ConfigureUI(isBroadcast);
        }

        private void ConfigureUI(bool isBroadcast)
        {
            this.grbInterval.Visible = isBroadcast;
            this.Height = isBroadcast ? 305 : 235;
            this.Text = isBroadcast ? "New CAN Broadcast Message" : "New Transmit CAN Message";
        }

        private RP1210_MsgCan CompileMessage()
        {
            RP1210_MsgCan rp1210MsgCan = new RP1210_MsgCan();
            rp1210MsgCan.Length = (int)this.nudDLC.Value;
            rp1210MsgCan.ID = Convert.ToUInt32(this.txtID.Text, 16);
            rp1210MsgCan.Extended = this.rdbExt.Checked;


            //rp1210MsgCan.Data = (byte[])this.m_Data?.Clone() ?? new byte[0];
            rp1210MsgCan.Data = new byte[2] { 0x01, 0x00 };


            return rp1210MsgCan;
        }

        private void LoadMessage(RP1210_MsgCan value)
        {
            this.txtID.Text = value.ID.ToString("X" + (value.Extended ? "8" : "3"));
            this.nudDLC.Value = (Decimal)value.Length;
            this.rdbExt.Checked = value.Extended;
            this.rdbStd.Checked = !this.rdbExt.Checked;
            for (int index = 0; index < value.Length; ++index)
                this.Controls.Find(string.Format("txtD{0}", (object)index), true)[0].Text = value.Data[index].ToString("X2");
        }

        private void txtD0_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(sender is TextBox textBox) || e.KeyChar == '\b')
            {
                return;
            }


            //string s = textBox.Text + e.KeyChar.ToString();
            //var numberStyle = NumberStyles.HexNumber;
            //ulong output = 0;
            //var shouldHandled =  !ulong.TryParse(s, numberStyle, (IFormatProvider)CultureInfo.InvariantCulture, out output);
            //e.Handled = shouldHandled;



            //e.Handled = !ulong.TryParse(textBox.Text + e.KeyChar.ToString(), NumberStyles.HexNumber, (IFormatProvider)CultureInfo.InvariantCulture, out ulong _);

        }

        private void txtD0_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "")
                textBox.Text = "0";
            textBox.Text = string.Format("{0:X2}", (object)Convert.ToByte(textBox.Text, 16));
        }

        private void rdbStd_CheckedChanged(object sender, EventArgs e)
        {
            this.txtID.MaxLength = this.rdbStd.Checked ? 3 : 8;
            this.txtID_Leave((object)this.txtID, new EventArgs());
        }

        private void txtID_Leave(object sender, EventArgs e)
        {
            if (this.txtID.Text == "")
                this.txtID.Text = "0";
            uint num = this.rdbStd.Checked ? 2047U : 536870911U;
            uint uint32 = Convert.ToUInt32(this.txtID.Text, 16);
            if (uint32 > num)
                this.txtID.Text = num.ToString("X" + (this.rdbStd.Checked ? "3" : "8"));
            else
                this.txtID.Text = uint32.ToString("X" + (this.rdbStd.Checked ? "3" : "8"));
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            byte[] buffer = new byte[(int)this.nudDLC.Value];
            new Random().NextBytes(buffer);
            for (int index = 0; index < buffer.Length; ++index)
                this.Controls.Find("txtD" + index.ToString(), true)[0].Text = buffer[index].ToString("X2");
        }

        public RP1210_MsgCan Message
        {
            get => this.CompileMessage();
            set => this.LoadMessage(value);
        }

        public ushort Interval
        {
            get => Convert.ToUInt16(this.nudInterval.Value);
            set => this.nudInterval.Value = (Decimal)value;
        }

        private void nudDLC_ValueChanged(object sender, EventArgs e)
        {
            Random random = new Random();
            this.m_Data = new byte[(int)this.nudDLC.Value];
            byte[] data = this.m_Data;
            random.NextBytes(data);
            for (int index = 0; index < 8; ++index)
            {
                string string1 = string.Format("txtD{0}", (object)index);
                var control1 = this.Controls.Find(string1, true);
                control1[0].Enabled = false;

                //this.Controls.Find (string1, true)?[0].Enabled = false;

                //this.Controls.Find (string.Format("txtD{0}", (object)index), true)?[0].Enabled = false;
            }

            for (int index = 0; index < this.m_Data.Length; ++index)
            {
                Control control = this.Controls.Find(string.Format("txtD{0}", (object)index), true)?[0];
                control.Text = string.Format("{0:X2} ", (object)this.m_Data[index]);
                control.Enabled = true;
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
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.txtID = new TextBox();
            this.nudDLC = new NumericUpDown();
            this.txtD0 = new TextBox();
            this.txtD1 = new TextBox();
            this.txtD2 = new TextBox();
            this.txtD3 = new TextBox();
            this.txtD7 = new TextBox();
            this.txtD6 = new TextBox();
            this.txtD5 = new TextBox();
            this.txtD4 = new TextBox();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.label8 = new Label();
            this.label9 = new Label();
            this.label10 = new Label();
            this.label11 = new Label();
            this.rdbStd = new RadioButton();
            this.label12 = new Label();
            this.rdbExt = new RadioButton();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.btnRandom = new Button();
            this.groupBox1 = new GroupBox();
            this.grbInterval = new GroupBox();
            this.label13 = new Label();
            this.nudInterval = new NumericUpDown();
            this.nudDLC.BeginInit();
            this.groupBox1.SuspendLayout();
            this.grbInterval.SuspendLayout();
            this.nudInterval.BeginInit();
            this.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID (Hex):";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(83, 16);
            this.label2.Name = "label2";
            this.label2.Size = new Size(31, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "DLC:";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(133, 16);
            this.label3.Name = "label3";
            this.label3.Size = new Size(61, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Data: (Hex)";
            this.txtID.CharacterCasing = CharacterCasing.Upper;
            this.txtID.Location = new Point(9, 32);
            this.txtID.Name = "txtID";
            this.txtID.Size = new Size(71, 20);
            this.txtID.TabIndex = 1;
            this.txtID.KeyPress += new KeyPressEventHandler(this.txtD0_KeyPress);
            this.txtID.Leave += new EventHandler(this.txtID_Leave);
            this.nudDLC.Location = new Point(86, 32);
            this.nudDLC.Maximum = new Decimal(new int[4]
            {
        8,
        0,
        0,
        0
            });
            this.nudDLC.Name = "nudDLC";
            this.nudDLC.Size = new Size(44, 20);
            this.nudDLC.TabIndex = 3;
            this.nudDLC.ValueChanged += new EventHandler(this.nudDLC_ValueChanged);
            this.txtD0.CharacterCasing = CharacterCasing.Upper;
            this.txtD0.Enabled = false;
            this.txtD0.Location = new Point(136, 32);
            this.txtD0.MaxLength = 2;
            this.txtD0.Name = "txtD0";
            this.txtD0.Size = new Size(29, 20);
            this.txtD0.TabIndex = 5;
            this.txtD0.Text = "00";
            this.txtD0.KeyPress += new KeyPressEventHandler(this.txtD0_KeyPress);
            this.txtD0.Leave += new EventHandler(this.txtD0_Leave);
            this.txtD1.CharacterCasing = CharacterCasing.Upper;
            this.txtD1.Enabled = false;
            this.txtD1.Location = new Point(171, 32);
            this.txtD1.MaxLength = 2;
            this.txtD1.Name = "txtD1";
            this.txtD1.Size = new Size(29, 20);
            this.txtD1.TabIndex = 6;
            this.txtD1.Text = "00";
            this.txtD1.KeyPress += new KeyPressEventHandler(this.txtD0_KeyPress);
            this.txtD1.Leave += new EventHandler(this.txtD0_Leave);
            this.txtD2.CharacterCasing = CharacterCasing.Upper;
            this.txtD2.Enabled = false;
            this.txtD2.Location = new Point(206, 32);
            this.txtD2.MaxLength = 2;
            this.txtD2.Name = "txtD2";
            this.txtD2.Size = new Size(29, 20);
            this.txtD2.TabIndex = 7;
            this.txtD2.Text = "00";
            this.txtD2.KeyPress += new KeyPressEventHandler(this.txtD0_KeyPress);
            this.txtD2.Leave += new EventHandler(this.txtD0_Leave);
            this.txtD3.CharacterCasing = CharacterCasing.Upper;
            this.txtD3.Enabled = false;
            this.txtD3.Location = new Point(241, 32);
            this.txtD3.MaxLength = 2;
            this.txtD3.Name = "txtD3";
            this.txtD3.Size = new Size(29, 20);
            this.txtD3.TabIndex = 8;
            this.txtD3.Text = "00";
            this.txtD3.KeyPress += new KeyPressEventHandler(this.txtD0_KeyPress);
            this.txtD3.Leave += new EventHandler(this.txtD0_Leave);
            this.txtD7.CharacterCasing = CharacterCasing.Upper;
            this.txtD7.Enabled = false;
            this.txtD7.Location = new Point(381, 32);
            this.txtD7.MaxLength = 2;
            this.txtD7.Name = "txtD7";
            this.txtD7.Size = new Size(29, 20);
            this.txtD7.TabIndex = 12;
            this.txtD7.Text = "00";
            this.txtD7.KeyPress += new KeyPressEventHandler(this.txtD0_KeyPress);
            this.txtD7.Leave += new EventHandler(this.txtD0_Leave);
            this.txtD6.CharacterCasing = CharacterCasing.Upper;
            this.txtD6.Enabled = false;
            this.txtD6.Location = new Point(346, 32);
            this.txtD6.MaxLength = 2;
            this.txtD6.Name = "txtD6";
            this.txtD6.Size = new Size(29, 20);
            this.txtD6.TabIndex = 11;
            this.txtD6.Text = "00";
            this.txtD6.KeyPress += new KeyPressEventHandler(this.txtD0_KeyPress);
            this.txtD6.Leave += new EventHandler(this.txtD0_Leave);
            this.txtD5.CharacterCasing = CharacterCasing.Upper;
            this.txtD5.Enabled = false;
            this.txtD5.Location = new Point(311, 32);
            this.txtD5.MaxLength = 2;
            this.txtD5.Name = "txtD5";
            this.txtD5.Size = new Size(29, 20);
            this.txtD5.TabIndex = 10;
            this.txtD5.Text = "00";
            this.txtD5.KeyPress += new KeyPressEventHandler(this.txtD0_KeyPress);
            this.txtD5.Leave += new EventHandler(this.txtD0_Leave);
            this.txtD4.CharacterCasing = CharacterCasing.Upper;
            this.txtD4.Enabled = false;
            this.txtD4.Location = new Point(276, 32);
            this.txtD4.MaxLength = 2;
            this.txtD4.Name = "txtD4";
            this.txtD4.Size = new Size(29, 20);
            this.txtD4.TabIndex = 9;
            this.txtD4.Text = "00";
            this.txtD4.KeyPress += new KeyPressEventHandler(this.txtD0_KeyPress);
            this.txtD4.Leave += new EventHandler(this.txtD0_Leave);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(143, 55);
            this.label4.Name = "label4";
            this.label4.Size = new Size(13, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "0";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(178, 55);
            this.label5.Name = "label5";
            this.label5.Size = new Size(13, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "1";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(214, 55);
            this.label6.Name = "label6";
            this.label6.Size = new Size(13, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "2";
            this.label7.AutoSize = true;
            this.label7.Location = new Point(249, 55);
            this.label7.Name = "label7";
            this.label7.Size = new Size(13, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "3";
            this.label8.AutoSize = true;
            this.label8.Location = new Point(284, 55);
            this.label8.Name = "label8";
            this.label8.Size = new Size(13, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "4";
            this.label9.AutoSize = true;
            this.label9.Location = new Point(318, 55);
            this.label9.Name = "label9";
            this.label9.Size = new Size(13, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "5";
            this.label10.AutoSize = true;
            this.label10.Location = new Point(354, 55);
            this.label10.Name = "label10";
            this.label10.Size = new Size(13, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "6";
            this.label11.AutoSize = true;
            this.label11.Location = new Point(389, 55);
            this.label11.Name = "label11";
            this.label11.Size = new Size(13, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "7";
            this.rdbStd.AutoSize = true;
            this.rdbStd.Location = new Point(9, 90);
            this.rdbStd.Name = "rdbStd";
            this.rdbStd.Size = new Size(68, 17);
            this.rdbStd.TabIndex = 23;
            this.rdbStd.TabStop = true;
            this.rdbStd.Text = "Standard";
            this.rdbStd.UseVisualStyleBackColor = true;
            this.rdbStd.CheckedChanged += new EventHandler(this.rdbStd_CheckedChanged);
            this.label12.AutoSize = true;
            this.label12.Location = new Point(6, 74);
            this.label12.Name = "label12";
            this.label12.Size = new Size(80, 13);
            this.label12.TabIndex = 21;
            this.label12.Text = "Message Type:";
            this.rdbExt.AutoSize = true;
            this.rdbExt.Location = new Point(10, 113);
            this.rdbExt.Name = "rdbExt";
            this.rdbExt.Size = new Size(70, 17);
            this.rdbExt.TabIndex = 24;
            this.rdbExt.TabStop = true;
            this.rdbExt.Text = "Extended";
            this.rdbExt.UseVisualStyleBackColor = true;
            this.rdbExt.CheckedChanged += new EventHandler(this.rdbStd_CheckedChanged);
            this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(278, 231);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(361, 231);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnRandom.Location = new Point(136, 74);
            this.btnRandom.Name = "btnRandom";
            this.btnRandom.Size = new Size(91, 23);
            this.btnRandom.TabIndex = 22;
            this.btnRandom.Text = "Random Data";
            this.btnRandom.UseVisualStyleBackColor = true;
            this.btnRandom.Visible = false;
            this.btnRandom.Click += new EventHandler(this.btnRandom_Click);
            this.groupBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.groupBox1.Controls.Add((Control)this.label1);
            this.groupBox1.Controls.Add((Control)this.btnRandom);
            this.groupBox1.Controls.Add((Control)this.label2);
            this.groupBox1.Controls.Add((Control)this.label3);
            this.groupBox1.Controls.Add((Control)this.txtID);
            this.groupBox1.Controls.Add((Control)this.rdbExt);
            this.groupBox1.Controls.Add((Control)this.nudDLC);
            this.groupBox1.Controls.Add((Control)this.label12);
            this.groupBox1.Controls.Add((Control)this.txtD0);
            this.groupBox1.Controls.Add((Control)this.rdbStd);
            this.groupBox1.Controls.Add((Control)this.txtD1);
            this.groupBox1.Controls.Add((Control)this.label11);
            this.groupBox1.Controls.Add((Control)this.txtD2);
            this.groupBox1.Controls.Add((Control)this.label10);
            this.groupBox1.Controls.Add((Control)this.txtD3);
            this.groupBox1.Controls.Add((Control)this.label9);
            this.groupBox1.Controls.Add((Control)this.txtD4);
            this.groupBox1.Controls.Add((Control)this.label8);
            this.groupBox1.Controls.Add((Control)this.txtD5);
            this.groupBox1.Controls.Add((Control)this.label7);
            this.groupBox1.Controls.Add((Control)this.txtD6);
            this.groupBox1.Controls.Add((Control)this.label6);
            this.groupBox1.Controls.Add((Control)this.txtD7);
            this.groupBox1.Controls.Add((Control)this.label5);
            this.groupBox1.Controls.Add((Control)this.label4);
            this.groupBox1.Location = new Point(12, 83);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(424, 142);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Message ";
            this.grbInterval.Controls.Add((Control)this.label13);
            this.grbInterval.Controls.Add((Control)this.nudInterval);
            this.grbInterval.Location = new Point(12, 12);
            this.grbInterval.Name = "grbInterval";
            this.grbInterval.Size = new Size(424, 66);
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
            this.AcceptButton = (IButtonControl)this.btnOK;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = (IButtonControl)this.btnCancel;
            this.ClientSize = new Size(448, 266);
            this.Controls.Add((Control)this.groupBox1);
            this.Controls.Add((Control)this.btnCancel);
            this.Controls.Add((Control)this.btnOK);
            this.Controls.Add((Control)this.grbInterval);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = nameof(FormTxMsgCan);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "New Transmit CAN Message";
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
