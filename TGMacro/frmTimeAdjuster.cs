using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TGMacro.Properties;
using TGMacro.Statics;
using TGMacro.UI.CustomControls;

namespace TGMacro;

public class frmTimeAdjuster : DoubleBufferForm
{
	private IContainer components = null;

	private NumericUpDown numericUpDown1;

	private Label label1;

	private Label label2;

	private HoverButton btnSave;

	private Panel pnlMain;

	public Panel pnlTop;

	public HoverPictureBox btnClose;

	public Label lblTitle;

	public int SleepAdjustValue => (int)numericUpDown1.Value;

	public frmTimeAdjuster()
	{
		InitializeComponent();
		pnlTop.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		BackColor = AppSession.Settings.ColorPalette.LightColor;
		ForeColor = AppSession.Settings.ColorPalette.TextColor;
		btnSave.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		btnSave.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		numericUpDown1.BackColor = AppSession.Settings.ColorPalette.LighterColor;
		numericUpDown1.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		base.Shown += FrmTimeAdjuster_Shown;
		label1.Text = AppSession.ActiveLanguage.text_adjust_time_info;
	}

	private void FrmTimeAdjuster_Shown(object sender, EventArgs ee)
	{
		base.TopMost = AppSession.TopMost;
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
		AppSession.TopMostStatusChanged += delegate
		{
			base.TopMost = AppSession.TopMost;
		};
		btnSave.Click += delegate
		{
			base.DialogResult = DialogResult.OK;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TGMacro.frmTimeAdjuster));
		this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.btnSave = new TGMacro.UI.CustomControls.HoverButton();
		this.pnlMain = new System.Windows.Forms.Panel();
		this.pnlTop = new System.Windows.Forms.Panel();
		this.btnClose = new TGMacro.UI.CustomControls.HoverPictureBox();
		this.lblTitle = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1).BeginInit();
		this.pnlMain.SuspendLayout();
		this.pnlTop.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnClose).BeginInit();
		base.SuspendLayout();
		this.numericUpDown1.Location = new System.Drawing.Point(124, 93);
		this.numericUpDown1.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.numericUpDown1.Minimum = new decimal(new int[4] { 10000, 0, 0, -2147483648 });
		this.numericUpDown1.Name = "numericUpDown1";
		this.numericUpDown1.Size = new System.Drawing.Size(120, 22);
		this.numericUpDown1.TabIndex = 0;
		this.label1.Location = new System.Drawing.Point(13, 42);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(275, 46);
		this.label1.TabIndex = 1;
		this.label1.Text = "Add or subtract x amount from wait actions of script.";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(13, 95);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(86, 17);
		this.label2.TabIndex = 2;
		this.label2.Text = "Milliseconds :";
		this.btnSave.BackColor = System.Drawing.SystemColors.ControlLight;
		this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(91, 91, 91);
		this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSave.HoverColor = System.Drawing.Color.FromArgb(240, 240, 240);
		this.btnSave.Image = TGMacro.Properties.Resources.save_32;
		this.btnSave.Location = new System.Drawing.Point(105, 129);
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(100, 41);
		this.btnSave.TabIndex = 7;
		this.btnSave.Text = "Save";
		this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
		this.btnSave.UseVisualStyleBackColor = false;
		this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnlMain.Controls.Add(this.pnlTop);
		this.pnlMain.Controls.Add(this.label1);
		this.pnlMain.Controls.Add(this.btnSave);
		this.pnlMain.Controls.Add(this.numericUpDown1);
		this.pnlMain.Controls.Add(this.label2);
		this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnlMain.Location = new System.Drawing.Point(0, 0);
		this.pnlMain.Name = "pnlMain";
		this.pnlMain.Size = new System.Drawing.Size(299, 182);
		this.pnlMain.TabIndex = 13;
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
		this.pnlTop.Size = new System.Drawing.Size(297, 30);
		this.pnlTop.TabIndex = 8;
		this.btnClose.BackgroundImage = TGMacro.Properties.Resources.close_16;
		this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnClose.HoverImage = TGMacro.Properties.Resources.close_16_hover;
		this.btnClose.Location = new System.Drawing.Point(267, 0);
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
		this.lblTitle.Size = new System.Drawing.Size(92, 30);
		this.lblTitle.TabIndex = 6;
		this.lblTitle.Text = "Wait Adjuster";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		base.AcceptButton = this.btnSave;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 17f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(299, 182);
		base.Controls.Add(this.pnlMain);
		this.Font = new System.Drawing.Font("Century Gothic", 9f);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.Name = "frmTimeAdjuster";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Wait Adjuster";
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1).EndInit();
		this.pnlMain.ResumeLayout(false);
		this.pnlMain.PerformLayout();
		this.pnlTop.ResumeLayout(false);
		this.pnlTop.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnClose).EndInit();
		base.ResumeLayout(false);
	}
}
