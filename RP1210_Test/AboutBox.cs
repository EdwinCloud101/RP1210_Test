
// Type: RP1210_Test.AboutBox




using RP1210_Test.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace RP1210_Test
{
  internal class AboutBox : Form
  {
    private IContainer components;
    private Label labelProductName;
    private Label labelCopyright;
    private Button okButton;
    private PictureBox picBackground;
    private Label laCopyright2;
    private Label laSupport2;
    private Label laSupport;
    private LinkLabel linkMail;
    private LinkLabel linkWeb;
    private Label laMail;
    private Label laWeb;

    public AboutBox(string implementationName, string dllVersion, string apiVersion)
    {
      this.InitializeComponent();
      this.Text = string.Format("About {0}", (object) this.AssemblyTitle);
      this.labelProductName.Text = "v" + this.AssemblyVersion + " (DLL v" + dllVersion + " - API v" + apiVersion + ")";
      this.labelCopyright.Text = this.AssemblyCopyright;
    }

    public string AssemblyTitle
    {
      get
      {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyTitleAttribute), false);
        if (customAttributes.Length != 0)
        {
          AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute) customAttributes[0];
          if (assemblyTitleAttribute.Title != "")
            return assemblyTitleAttribute.Title;
        }
        return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
      }
    }

    public string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

    public string AssemblyDescription
    {
      get
      {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false);
        return customAttributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute) customAttributes[0]).Description;
      }
    }

    public string AssemblyProduct
    {
      get
      {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyProductAttribute), false);
        return customAttributes.Length == 0 ? "" : ((AssemblyProductAttribute) customAttributes[0]).Product;
      }
    }

    public string AssemblyCopyright
    {
      get
      {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);
        return customAttributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute) customAttributes[0]).Copyright;
      }
    }

    public string AssemblyCompany
    {
      get
      {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyCompanyAttribute), false);
        return customAttributes.Length == 0 ? "" : ((AssemblyCompanyAttribute) customAttributes[0]).Company;
      }
    }

    private void AboutBox_Load(object sender, EventArgs e)
    {
      this.okButton.Parent = (Control) this.picBackground;
      this.labelProductName.Parent = (Control) this.picBackground;
      this.labelCopyright.Parent = (Control) this.picBackground;
      this.laCopyright2.Parent = (Control) this.picBackground;
      this.laSupport.Parent = (Control) this.picBackground;
      this.laSupport2.Parent = (Control) this.picBackground;
      this.laWeb.Parent = (Control) this.picBackground;
      this.laMail.Parent = (Control) this.picBackground;
      this.linkWeb.Parent = (Control) this.picBackground;
      this.linkMail.Parent = (Control) this.picBackground;
    }

    private void linkWeb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start("http://www.peak-system.com");

    private void linkMail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start("mailto:support@peak-system.com");

    private void linkWeb_MouseEnter(object sender, EventArgs e)
    {
      if (!(sender is LinkLabel linkLabel))
        return;
      linkLabel.LinkColor = Color.DarkOrange;
    }

    private void linkWeb_MouseLeave(object sender, EventArgs e)
    {
      if (!(sender is LinkLabel linkLabel))
        return;
      linkLabel.LinkColor = Color.LightSkyBlue;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.labelProductName = new Label();
      this.labelCopyright = new Label();
      this.okButton = new Button();
      this.picBackground = new PictureBox();
      this.laCopyright2 = new Label();
      this.laSupport2 = new Label();
      this.laSupport = new Label();
      this.linkMail = new LinkLabel();
      this.linkWeb = new LinkLabel();
      this.laMail = new Label();
      this.laWeb = new Label();
      ((ISupportInitialize) this.picBackground).BeginInit();
      this.SuspendLayout();
      this.labelProductName.AutoSize = true;
      this.labelProductName.BackColor = Color.Transparent;
      this.labelProductName.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold);
      this.labelProductName.ForeColor = Color.White;
      this.labelProductName.Location = new Point(118, 98);
      this.labelProductName.Margin = new Padding(6, 0, 3, 0);
      this.labelProductName.MaximumSize = new Size(0, 17);
      this.labelProductName.Name = "labelProductName";
      this.labelProductName.Size = new Size(96, 17);
      this.labelProductName.TabIndex = 27;
      this.labelProductName.Text = "Product Name";
      this.labelProductName.TextAlign = ContentAlignment.MiddleLeft;
      this.labelCopyright.AutoSize = true;
      this.labelCopyright.BackColor = Color.Transparent;
      this.labelCopyright.Font = new Font("Segoe UI", 9f);
      this.labelCopyright.ForeColor = Color.White;
      this.labelCopyright.Location = new Point(118, 133);
      this.labelCopyright.Margin = new Padding(6, 0, 3, 0);
      this.labelCopyright.MaximumSize = new Size(0, 17);
      this.labelCopyright.Name = "labelCopyright";
      this.labelCopyright.Size = new Size(60, 15);
      this.labelCopyright.TabIndex = 28;
      this.labelCopyright.Text = "Copyright";
      this.labelCopyright.TextAlign = ContentAlignment.MiddleLeft;
      this.okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okButton.DialogResult = DialogResult.Cancel;
      this.okButton.Location = new Point(302, 307);
      this.okButton.Name = "okButton";
      this.okButton.Size = new Size(75, 20);
      this.okButton.TabIndex = 31;
      this.okButton.Text = "&OK";
      this.picBackground.BackgroundImage = (Image) Resources.AboutForm;
      this.picBackground.BackgroundImageLayout = ImageLayout.None;
      this.picBackground.Dock = DockStyle.Fill;
      this.picBackground.Location = new Point(0, 0);
      this.picBackground.Name = "picBackground";
      this.picBackground.Size = new Size(406, 349);
      this.picBackground.TabIndex = 33;
      this.picBackground.TabStop = false;
      this.laCopyright2.AutoSize = true;
      this.laCopyright2.BackColor = Color.Transparent;
      this.laCopyright2.Font = new Font("Segoe UI", 9f);
      this.laCopyright2.ForeColor = Color.White;
      this.laCopyright2.Location = new Point(118, 149);
      this.laCopyright2.Name = "laCopyright2";
      this.laCopyright2.Size = new Size(104, 15);
      this.laCopyright2.TabIndex = 34;
      this.laCopyright2.Text = "All rights reserved.";
      this.laSupport2.AutoSize = true;
      this.laSupport2.BackColor = Color.Transparent;
      this.laSupport2.Font = new Font("Segoe UI", 9f);
      this.laSupport2.ForeColor = Color.White;
      this.laSupport2.Location = new Point(118, 192);
      this.laSupport2.Name = "laSupport2";
      this.laSupport2.Size = new Size(159, 45);
      this.laSupport2.TabIndex = 36;
      this.laSupport2.Text = "PEAK-System Technik GmbH\r\nOtto-Roehm-Str. 69\r\n64293 Darmstadt, Germany\r\n";
      this.laSupport.AutoSize = true;
      this.laSupport.BackColor = Color.Transparent;
      this.laSupport.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.laSupport.ForeColor = Color.White;
      this.laSupport.Location = new Point(118, 170);
      this.laSupport.Name = "laSupport";
      this.laSupport.Size = new Size(225, 17);
      this.laSupport.TabIndex = 35;
      this.laSupport.Text = "Support and technical information:";
      this.linkMail.ActiveLinkColor = Color.DarkOrange;
      this.linkMail.AutoSize = true;
      this.linkMail.BackColor = Color.Transparent;
      this.linkMail.Font = new Font("Segoe UI", 9f);
      this.linkMail.LinkBehavior = LinkBehavior.HoverUnderline;
      this.linkMail.LinkColor = Color.LightSkyBlue;
      this.linkMail.Location = new Point(167, 259);
      this.linkMail.Name = "linkMail";
      this.linkMail.Size = new Size(153, 15);
      this.linkMail.TabIndex = 40;
      this.linkMail.TabStop = true;
      this.linkMail.Text = "support@peak-system.com";
      this.linkMail.VisitedLinkColor = Color.DarkOrange;
      this.linkMail.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkMail_LinkClicked);
      this.linkMail.MouseEnter += new EventHandler(this.linkWeb_MouseEnter);
      this.linkMail.MouseLeave += new EventHandler(this.linkWeb_MouseLeave);
      this.linkWeb.ActiveLinkColor = Color.DarkOrange;
      this.linkWeb.AutoSize = true;
      this.linkWeb.BackColor = Color.Transparent;
      this.linkWeb.Font = new Font("Segoe UI", 9f);
      this.linkWeb.LinkBehavior = LinkBehavior.HoverUnderline;
      this.linkWeb.LinkColor = Color.LightSkyBlue;
      this.linkWeb.Location = new Point(167, 243);
      this.linkWeb.Name = "linkWeb";
      this.linkWeb.Size = new Size(166, 15);
      this.linkWeb.TabIndex = 39;
      this.linkWeb.TabStop = true;
      this.linkWeb.Text = "http://www.peak-system.com";
      this.linkWeb.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkWeb_LinkClicked);
      this.linkWeb.MouseEnter += new EventHandler(this.linkWeb_MouseEnter);
      this.linkWeb.MouseLeave += new EventHandler(this.linkWeb_MouseLeave);
      this.laMail.AutoSize = true;
      this.laMail.BackColor = Color.Transparent;
      this.laMail.Font = new Font("Segoe UI", 9f);
      this.laMail.ForeColor = Color.White;
      this.laMail.Location = new Point(118, 259);
      this.laMail.Name = "laMail";
      this.laMail.Size = new Size(44, 15);
      this.laMail.TabIndex = 38;
      this.laMail.Text = "E-Mail:";
      this.laWeb.AutoSize = true;
      this.laWeb.BackColor = Color.Transparent;
      this.laWeb.Font = new Font("Segoe UI", 9f);
      this.laWeb.ForeColor = Color.White;
      this.laWeb.Location = new Point(118, 243);
      this.laWeb.Name = "laWeb";
      this.laWeb.Size = new Size(34, 15);
      this.laWeb.TabIndex = 37;
      this.laWeb.Text = "Web:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(406, 349);
      this.Controls.Add((Control) this.linkMail);
      this.Controls.Add((Control) this.linkWeb);
      this.Controls.Add((Control) this.laMail);
      this.Controls.Add((Control) this.laWeb);
      this.Controls.Add((Control) this.laSupport2);
      this.Controls.Add((Control) this.laSupport);
      this.Controls.Add((Control) this.laCopyright2);
      this.Controls.Add((Control) this.labelProductName);
      this.Controls.Add((Control) this.labelCopyright);
      this.Controls.Add((Control) this.okButton);
      this.Controls.Add((Control) this.picBackground);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AboutBox);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (AboutBox);
      this.Load += new EventHandler(this.AboutBox_Load);
      ((ISupportInitialize) this.picBackground).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
