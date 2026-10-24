using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TGMacro.Properties;
using TGMacro.Statics;
using TGMacro.UI.CustomControls;

namespace TGMacro;

public class frmAbout : DoubleBufferForm
{
	private IContainer components = null;

	private Label label1;

	private Label label2;

	private PictureBox pictureBox1;

	private Panel pnlMain;

	public Panel pnlTop;

	public HoverPictureBox btnClose;

	public Label lblTitle;

	private HoverButton button1;

	private Label label3;

	public frmAbout()
	{
		InitializeComponent();
		pnlTop.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		BackColor = AppSession.Settings.ColorPalette.LightColor;
		lblTitle.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		label1.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		label2.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		base.Shown += FrmAbout_Shown;
	}

	private void FrmAbout_Shown(object sender, EventArgs ee)
	{
		base.TopMost = AppSession.TopMost;
		button1.Click += delegate
		{
			Process.Start("http://www.trksyln.net");
		};
		label1.Text = label1.Text.Replace("{0}", AppSession.Version);
		btnClose.Click += delegate
		{
			Close();
		};
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TGMacro.frmAbout));
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.pnlMain = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.pnlTop = new System.Windows.Forms.Panel();
		this.btnClose = new TGMacro.UI.CustomControls.HoverPictureBox();
		this.lblTitle = new System.Windows.Forms.Label();
		this.button1 = new TGMacro.UI.CustomControls.HoverButton();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.pnlMain.SuspendLayout();
		this.pnlTop.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnClose).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label1.Font = new System.Drawing.Font("Century Gothic", 16.2f);
		this.label1.Location = new System.Drawing.Point(159, 79);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(161, 25);
		this.label1.TabIndex = 0;
		this.label1.Text = "TGMacro v{0}";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Century Gothic", 10.2f);
		this.label2.Location = new System.Drawing.Point(163, 114);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(147, 19);
		this.label2.TabIndex = 1;
		this.label2.Text = "Tarık SAYLAN @trksyln";
		this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnlMain.Controls.Add(this.label3);
		this.pnlMain.Controls.Add(this.pnlTop);
		this.pnlMain.Controls.Add(this.button1);
		this.pnlMain.Controls.Add(this.pictureBox1);
		this.pnlMain.Controls.Add(this.label1);
		this.pnlMain.Controls.Add(this.label2);
		this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnlMain.Location = new System.Drawing.Point(0, 0);
		this.pnlMain.Name = "pnlMain";
		this.pnlMain.Size = new System.Drawing.Size(424, 236);
		this.pnlMain.TabIndex = 16;
		this.label3.BackColor = System.Drawing.SystemColors.ControlDark;
		this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label3.Font = new System.Drawing.Font("Century Gothic", 16.2f);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(0, 43);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(423, 25);
		this.label3.TabIndex = 18;
		this.label3.Text = "PREVIEW";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label3.Visible = false;
		this.pnlTop.BackColor = System.Drawing.SystemColors.ControlLight;
		this.pnlTop.Controls.Add(this.btnClose);
		this.pnlTop.Controls.Add(this.lblTitle);
		this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnlTop.Font = new System.Drawing.Font("Century Gothic", 9f);
		this.pnlTop.Location = new System.Drawing.Point(0, 0);
		this.pnlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.pnlTop.MaximumSize = new System.Drawing.Size(0, 30);
		this.pnlTop.MinimumSize = new System.Drawing.Size(0, 30);
		this.pnlTop.Name = "pnlTop";
		this.pnlTop.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
		this.pnlTop.Size = new System.Drawing.Size(422, 30);
		this.pnlTop.TabIndex = 16;
		this.btnClose.BackgroundImage = TGMacro.Properties.Resources.close_16;
		this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnClose.HoverImage = TGMacro.Properties.Resources.close_16_hover;
		this.btnClose.Location = new System.Drawing.Point(392, 0);
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
		this.lblTitle.Size = new System.Drawing.Size(52, 30);
		this.lblTitle.TabIndex = 6;
		this.lblTitle.Text = "About";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.button1.BackgroundImage = TGMacro.Properties.Resources.logo;
		this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
		this.button1.FlatAppearance.BorderSize = 0;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.HoverColor = System.Drawing.Color.FromArgb(20, 20, 20);
		this.button1.Location = new System.Drawing.Point(164, 141);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(156, 40);
		this.button1.TabIndex = 17;
		this.button1.UseVisualStyleBackColor = true;
		this.pictureBox1.Image = TGMacro.Properties.Resources.png;
		this.pictureBox1.Location = new System.Drawing.Point(50, 81);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(103, 113);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox1.TabIndex = 15;
		this.pictureBox1.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 15f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(424, 236);
		base.Controls.Add(this.pnlMain);
		this.Font = new System.Drawing.Font("Century Gothic", 7.8f);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmAbout";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "About";
		this.pnlMain.ResumeLayout(false);
		this.pnlMain.PerformLayout();
		this.pnlTop.ResumeLayout(false);
		this.pnlTop.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnClose).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
	}
}
