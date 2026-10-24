using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TGMacro.Properties;
using TGMacro.Statics;
using TGMacro.UI.CustomControls;

namespace TGMacro;

public class frmMessage : DoubleBufferForm
{
	private int SpecialCode = 0;

	private IContainer components = null;

	private Panel pnlMain;

	public Panel pnlTop;

	public HoverPictureBox btnClose;

	public Label lblTitle;

	private Label lblMessage;

	private HoverButton btnOk;

	private Panel panel1;

	private Panel panel2;

	private Panel pnlNo;

	private HoverButton btnNo;

	private Panel pnlCancel;

	private HoverButton btnCancel;

	public frmMessage(string message, string title, string yesButtonText, string noButtonText, string cancelButtonText, int specialCode = 0)
	{
		InitializeComponent();
		SpecialCode = specialCode;
		lblMessage.Text = message;
		lblTitle.Text = title;
		pnlTop.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		ForeColor = AppSession.Settings.ColorPalette.TextColor;
		BackColor = AppSession.Settings.ColorPalette.LightColor;
		btnOk.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		btnNo.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		btnCancel.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		if (!string.IsNullOrWhiteSpace(yesButtonText))
		{
			btnOk.Text = yesButtonText;
		}
		if (!string.IsNullOrWhiteSpace(noButtonText))
		{
			pnlNo.Visible = true;
			btnNo.Text = noButtonText;
		}
		if (!string.IsNullOrWhiteSpace(cancelButtonText))
		{
			pnlCancel.Visible = true;
			btnCancel.Text = cancelButtonText;
		}
		switch (specialCode)
		{
		case 1:
			lblMessage.ForeColor = Color.DarkGreen;
			break;
		case 2:
			lblMessage.ForeColor = Color.Red;
			lblMessage.Font = new Font(lblMessage.Font, FontStyle.Bold);
			break;
		}
		base.KeyDown += FrmMessage_KeyDown;
		lblTitle.MouseDown += delegate(object s, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				SharedMethods.DragWindow(base.Handle);
			}
		};
		pnlTop.MouseDown += delegate(object s, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				SharedMethods.DragWindow(base.Handle);
			}
		};
		btnClose.Click += delegate
		{
			Close();
		};
	}

	private void FrmMessage_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Escape && SpecialCode == 0)
		{
			base.DialogResult = DialogResult.Cancel;
		}
		if (e.KeyCode == Keys.Return)
		{
			base.DialogResult = DialogResult.OK;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TGMacro.frmMessage));
		this.pnlMain = new System.Windows.Forms.Panel();
		this.lblMessage = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btnOk = new TGMacro.UI.CustomControls.HoverButton();
		this.pnlNo = new System.Windows.Forms.Panel();
		this.btnNo = new TGMacro.UI.CustomControls.HoverButton();
		this.pnlCancel = new System.Windows.Forms.Panel();
		this.btnCancel = new TGMacro.UI.CustomControls.HoverButton();
		this.pnlTop = new System.Windows.Forms.Panel();
		this.btnClose = new TGMacro.UI.CustomControls.HoverPictureBox();
		this.lblTitle = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.pnlMain.SuspendLayout();
		this.panel2.SuspendLayout();
		this.pnlNo.SuspendLayout();
		this.pnlCancel.SuspendLayout();
		this.pnlTop.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnClose).BeginInit();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.pnlMain.AutoSize = true;
		this.pnlMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnlMain.Controls.Add(this.lblMessage);
		this.pnlMain.Controls.Add(this.panel2);
		this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnlMain.Location = new System.Drawing.Point(0, 30);
		this.pnlMain.Name = "pnlMain";
		this.pnlMain.Padding = new System.Windows.Forms.Padding(15);
		this.pnlMain.Size = new System.Drawing.Size(463, 87);
		this.pnlMain.TabIndex = 16;
		this.lblMessage.AutoSize = true;
		this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMessage.Font = new System.Drawing.Font("Century Gothic", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 162);
		this.lblMessage.Location = new System.Drawing.Point(15, 15);
		this.lblMessage.MaximumSize = new System.Drawing.Size(431, 0);
		this.lblMessage.Name = "lblMessage";
		this.lblMessage.Size = new System.Drawing.Size(47, 17);
		this.lblMessage.TabIndex = 17;
		this.lblMessage.Text = "label1";
		this.panel2.AutoSize = true;
		this.panel2.Controls.Add(this.btnOk);
		this.panel2.Controls.Add(this.pnlNo);
		this.panel2.Controls.Add(this.pnlCancel);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel2.Location = new System.Drawing.Point(15, 30);
		this.panel2.MinimumSize = new System.Drawing.Size(0, 40);
		this.panel2.Name = "panel2";
		this.panel2.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
		this.panel2.Size = new System.Drawing.Size(431, 40);
		this.panel2.TabIndex = 38;
		this.btnOk.AutoSize = true;
		this.btnOk.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.btnOk.BackColor = System.Drawing.SystemColors.ControlLight;
		this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Yes;
		this.btnOk.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnOk.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(91, 91, 91);
		this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnOk.Font = new System.Drawing.Font("Century Gothic", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 162);
		this.btnOk.ForeColor = System.Drawing.Color.FromArgb(89, 166, 92);
		this.btnOk.HoverColor = null;
		this.btnOk.Location = new System.Drawing.Point(232, 10);
		this.btnOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnOk.MinimumSize = new System.Drawing.Size(0, 28);
		this.btnOk.Name = "btnOk";
		this.btnOk.Size = new System.Drawing.Size(38, 30);
		this.btnOk.TabIndex = 35;
		this.btnOk.Text = "OK";
		this.btnOk.UseVisualStyleBackColor = false;
		this.pnlNo.AutoSize = true;
		this.pnlNo.Controls.Add(this.btnNo);
		this.pnlNo.Dock = System.Windows.Forms.DockStyle.Right;
		this.pnlNo.Location = new System.Drawing.Point(270, 10);
		this.pnlNo.Name = "pnlNo";
		this.pnlNo.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
		this.pnlNo.Size = new System.Drawing.Size(50, 30);
		this.pnlNo.TabIndex = 38;
		this.pnlNo.Visible = false;
		this.btnNo.AutoSize = true;
		this.btnNo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.btnNo.BackColor = System.Drawing.SystemColors.ControlLight;
		this.btnNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
		this.btnNo.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnNo.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(91, 91, 91);
		this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnNo.Font = new System.Drawing.Font("Century Gothic", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 162);
		this.btnNo.ForeColor = System.Drawing.Color.Firebrick;
		this.btnNo.HoverColor = null;
		this.btnNo.Location = new System.Drawing.Point(10, 0);
		this.btnNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnNo.MinimumSize = new System.Drawing.Size(0, 28);
		this.btnNo.Name = "btnNo";
		this.btnNo.Size = new System.Drawing.Size(40, 30);
		this.btnNo.TabIndex = 37;
		this.btnNo.Text = "NO";
		this.btnNo.UseVisualStyleBackColor = false;
		this.pnlCancel.AutoSize = true;
		this.pnlCancel.Controls.Add(this.btnCancel);
		this.pnlCancel.Dock = System.Windows.Forms.DockStyle.Right;
		this.pnlCancel.Location = new System.Drawing.Point(320, 10);
		this.pnlCancel.Name = "pnlCancel";
		this.pnlCancel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
		this.pnlCancel.Size = new System.Drawing.Size(111, 30);
		this.pnlCancel.TabIndex = 39;
		this.pnlCancel.Visible = false;
		this.btnCancel.AutoSize = true;
		this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.btnCancel.BackColor = System.Drawing.SystemColors.ControlLight;
		this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(91, 91, 91);
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Century Gothic", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 162);
		this.btnCancel.ForeColor = System.Drawing.Color.Silver;
		this.btnCancel.HoverColor = null;
		this.btnCancel.Location = new System.Drawing.Point(10, 0);
		this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnCancel.MinimumSize = new System.Drawing.Size(0, 28);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(101, 30);
		this.btnCancel.TabIndex = 38;
		this.btnCancel.Text = "DOWNLOAD!";
		this.btnCancel.UseVisualStyleBackColor = false;
		this.pnlTop.BackColor = System.Drawing.SystemColors.ControlLight;
		this.pnlTop.Controls.Add(this.btnClose);
		this.pnlTop.Controls.Add(this.lblTitle);
		this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnlTop.Font = new System.Drawing.Font("Century Gothic", 9f);
		this.pnlTop.Location = new System.Drawing.Point(0, 0);
		this.pnlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.pnlTop.MaximumSize = new System.Drawing.Size(0, 30);
		this.pnlTop.MinimumSize = new System.Drawing.Size(0, 30);
		this.pnlTop.Name = "pnlTop";
		this.pnlTop.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
		this.pnlTop.Size = new System.Drawing.Size(463, 30);
		this.pnlTop.TabIndex = 16;
		this.btnClose.BackgroundImage = TGMacro.Properties.Resources.close_16;
		this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnClose.HoverImage = TGMacro.Properties.Resources.close_16_hover;
		this.btnClose.Location = new System.Drawing.Point(433, 0);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(20, 30);
		this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.btnClose.TabIndex = 9;
		this.btnClose.TabStop = false;
		this.lblTitle.AutoSize = true;
		this.lblTitle.BackColor = System.Drawing.Color.Transparent;
		this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
		this.lblTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblTitle.Font = new System.Drawing.Font("Century Gothic", 9f, System.Drawing.FontStyle.Bold);
		this.lblTitle.Location = new System.Drawing.Point(10, 0);
		this.lblTitle.MaximumSize = new System.Drawing.Size(0, 30);
		this.lblTitle.MinimumSize = new System.Drawing.Size(0, 30);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
		this.lblTitle.Size = new System.Drawing.Size(43, 30);
		this.lblTitle.TabIndex = 6;
		this.lblTitle.Text = "Error";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel1.AutoSize = true;
		this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.pnlMain);
		this.panel1.Controls.Add(this.pnlTop);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(465, 119);
		this.panel1.TabIndex = 17;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 15f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		base.ClientSize = new System.Drawing.Size(465, 119);
		base.Controls.Add(this.panel1);
		this.Font = new System.Drawing.Font("Century Gothic", 7.8f);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		this.MinimumSize = new System.Drawing.Size(465, 92);
		base.Name = "frmMessage";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Message";
		this.pnlMain.ResumeLayout(false);
		this.pnlMain.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.pnlNo.ResumeLayout(false);
		this.pnlNo.PerformLayout();
		this.pnlCancel.ResumeLayout(false);
		this.pnlCancel.PerformLayout();
		this.pnlTop.ResumeLayout(false);
		this.pnlTop.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnClose).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
