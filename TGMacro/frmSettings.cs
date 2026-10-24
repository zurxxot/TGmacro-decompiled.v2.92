using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CSInputs.Enums;
using CSInputs.ReadInput;
using CSInputs.Structs;
using TGMacro.Bases;
using TGMacro.Enums;
using TGMacro.Properties;
using TGMacro.Statics;
using TGMacro.SysModels;
using TGMacro.UI.CustomControls;

namespace TGMacro;

public class frmSettings : DoubleBufferForm
{
	private CSInputs.Enums.KeyboardKeys ActivationKey;

	private CSInputs.Enums.KeyboardKeys ActivationKeyModifier;

	private CSInputs.Enums.KeyboardKeys RecordKey;

	private CSInputs.Enums.KeyboardKeys RecordKeyModifier;

	private TextBox listeningLabel;

	private bool _isListeningForTriggerKey = false;

	private IContainer components = null;

	private Panel pnlMain;

	public Panel pnlTop;

	public HoverPictureBox btnClose;

	public Label lblTitle;

	private Label label4;

	private Label label3;

	private HoverButton btnSave;

	private Label label2;

	private Label label1;

	private Panel pnlTextColor;

	private Panel pnlLighterColor;

	private Panel pnlDarkColor;

	private Panel pnlLightColor;

	private Panel pnlMidColor;

	private CheckBox cbLoadLastProject;

	private Label label7;

	private CheckBox cbEnableOnStart;

	private Label label6;

	private Panel pnlPalette;

	private TextBox txtRecordKey;

	private TextBox txtActivationKey;

	private ColorDialog colorDialog1;

	private HoverButton btnLoadDefaults;

	private FlattenComboBox cmbLanguage;

	private CheckBox cbMoveToTrayWhenMinimized;

	private Label label5;

	private bool IsListeningForKey
	{
		get
		{
			return _isListeningForTriggerKey;
		}
		set
		{
			_isListeningForTriggerKey = value;
			listeningLabel.Enabled = !_isListeningForTriggerKey;
			ViewKeysOnKeySetterTextBox();
		}
	}

	public frmSettings()
	{
		InitializeComponent();
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
		btnLoadDefaults.Click += delegate
		{
			AppSession.Settings = new TGMacro.SysModels.Settings();
			LoadSettings();
			Messenger.Show("Default settings are loaded. Save to apply.", "Notify");
		};
		btnSave.Click += BtnSave_Click;
		btnClose.Click += delegate
		{
			Close();
		};
		base.Shown += FrmSettings_Shown;
		txtActivationKey.MouseUp += EventKeySetterClick;
		txtRecordKey.MouseUp += EventKeySetterClick;
		foreach (Panel item in pnlPalette.Controls)
		{
			item.Click += delegate
			{
				if (colorDialog1.ShowDialog() == DialogResult.OK)
				{
					item.BackColor = colorDialog1.Color;
				}
			};
		}
		pnlTop.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		ForeColor = AppSession.Settings.ColorPalette.TextColor;
		BackColor = AppSession.Settings.ColorPalette.LightColor;
		btnSave.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		btnSave.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		btnLoadDefaults.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		btnLoadDefaults.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		txtActivationKey.BackColor = AppSession.Settings.ColorPalette.LighterColor;
		txtActivationKey.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		txtRecordKey.BackColor = AppSession.Settings.ColorPalette.LighterColor;
		txtRecordKey.ForeColor = AppSession.Settings.ColorPalette.TextColor;
	}

	private void LoadSettings()
	{
		AppSession.InputListener.KeyboardInputs += KeyboardInputs;
		cmbLanguage.SelectedIndex = cmbLanguage.FindString(AppSession.Settings.Language.ToLower());
		cbEnableOnStart.Checked = AppSession.Settings.EnableOnStart;
		cbLoadLastProject.Checked = AppSession.Settings.LoadLastMacroOnLoad;
		cbMoveToTrayWhenMinimized.Checked = AppSession.Settings.MoveToTrayWhenMinimized;
		pnlDarkColor.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		pnlLightColor.BackColor = AppSession.Settings.ColorPalette.LightColor;
		pnlLighterColor.BackColor = AppSession.Settings.ColorPalette.LighterColor;
		pnlMidColor.BackColor = AppSession.Settings.ColorPalette.MidColor;
		pnlTextColor.BackColor = AppSession.Settings.ColorPalette.TextColor;
		ActivationKey = (CSInputs.Enums.KeyboardKeys)AppSession.Settings.ActivationKey;
		ActivationKeyModifier = (CSInputs.Enums.KeyboardKeys)AppSession.Settings.ActivationKeyModifier;
		RecordKey = (CSInputs.Enums.KeyboardKeys)AppSession.Settings.RecordKey;
		RecordKeyModifier = (CSInputs.Enums.KeyboardKeys)AppSession.Settings.RecordKeyModifier;
		ViewKeysOnKeySetterTextBox();
	}

	private void BtnSave_Click(object sender, EventArgs e)
	{
		AppSession.Settings.ColorPalette.DarkColor = pnlDarkColor.BackColor;
		AppSession.Settings.ColorPalette.LightColor = pnlLightColor.BackColor;
		AppSession.Settings.ColorPalette.LighterColor = pnlLighterColor.BackColor;
		AppSession.Settings.ColorPalette.MidColor = pnlMidColor.BackColor;
		AppSession.Settings.ColorPalette.TextColor = pnlTextColor.BackColor;
		AppSession.Settings.ActivationKey = (int)ActivationKey;
		AppSession.Settings.ActivationKeyModifier = (int)ActivationKeyModifier;
		AppSession.Settings.RecordKey = (int)RecordKey;
		AppSession.Settings.RecordKeyModifier = (int)RecordKeyModifier;
		LanguageBase languageBase = (AppSession.ActiveLanguage = cmbLanguage.SelectedItem as LanguageBase);
		AppSession.Settings.Language = languageBase.language_name;
		AppSession.Settings.LoadLastMacroOnLoad = cbLoadLastProject.Checked;
		AppSession.Settings.EnableOnStart = cbEnableOnStart.Checked;
		AppSession.Settings.MoveToTrayWhenMinimized = cbMoveToTrayWhenMinimized.Checked;
		SharedMethods.SaveSettings();
	}

	private void FrmSettings_Shown(object sender, EventArgs e)
	{
		cmbLanguage.DataSource = AppSession.Languages;
		LoadSettings();
	}

	private void EventKeySetterClick(object sender, EventArgs e)
	{
		if (!IsListeningForKey)
		{
			TextBox textBox = (listeningLabel = (TextBox)sender);
			IsListeningForKey = !IsListeningForKey;
			if (IsListeningForKey)
			{
				textBox.Text = AppSession.ActiveLanguage.text_press_any_key_to_set;
			}
		}
	}

	private void ViewKeysOnKeySetterTextBox()
	{
		txtActivationKey.Text = ((ActivationKeyModifier != CSInputs.Enums.KeyboardKeys.None) ? (ActivationKeyModifier.ToString() + " + " + ActivationKey) : ActivationKey.ToString());
		txtRecordKey.Text = ((RecordKeyModifier != CSInputs.Enums.KeyboardKeys.None) ? (RecordKeyModifier.ToString() + " + " + RecordKey) : RecordKey.ToString());
	}

	private void KeyboardInputs(KeyboardData data, ref ModifierKey modifierKey)
	{
		if (IsListeningForKey && data.Key == CSInputs.Enums.KeyboardKeys.Escape)
		{
			IsListeningForKey = false;
		}
		else if (IsListeningForKey && Enum.IsDefined(typeof(KeysForTrigger), (KeysForTrigger)data.Key))
		{
			if (listeningLabel == txtActivationKey)
			{
				ActivationKeyModifier = SharedMethods.ModifierKeyToKey(modifierKey);
				ActivationKey = data.Key;
			}
			else
			{
				RecordKeyModifier = SharedMethods.ModifierKeyToKey(modifierKey);
				RecordKey = data.Key;
			}
			IsListeningForKey = false;
			ViewKeysOnKeySetterTextBox();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TGMacro.frmSettings));
		this.pnlMain = new System.Windows.Forms.Panel();
		this.cbMoveToTrayWhenMinimized = new System.Windows.Forms.CheckBox();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbLanguage = new TGMacro.UI.CustomControls.FlattenComboBox();
		this.btnLoadDefaults = new TGMacro.UI.CustomControls.HoverButton();
		this.txtRecordKey = new System.Windows.Forms.TextBox();
		this.txtActivationKey = new System.Windows.Forms.TextBox();
		this.pnlPalette = new System.Windows.Forms.Panel();
		this.pnlLighterColor = new System.Windows.Forms.Panel();
		this.pnlMidColor = new System.Windows.Forms.Panel();
		this.pnlLightColor = new System.Windows.Forms.Panel();
		this.pnlDarkColor = new System.Windows.Forms.Panel();
		this.pnlTextColor = new System.Windows.Forms.Panel();
		this.cbLoadLastProject = new System.Windows.Forms.CheckBox();
		this.label7 = new System.Windows.Forms.Label();
		this.cbEnableOnStart = new System.Windows.Forms.CheckBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.pnlTop = new System.Windows.Forms.Panel();
		this.btnClose = new TGMacro.UI.CustomControls.HoverPictureBox();
		this.lblTitle = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.btnSave = new TGMacro.UI.CustomControls.HoverButton();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.colorDialog1 = new System.Windows.Forms.ColorDialog();
		this.pnlMain.SuspendLayout();
		this.pnlPalette.SuspendLayout();
		this.pnlTop.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnClose).BeginInit();
		base.SuspendLayout();
		this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnlMain.Controls.Add(this.cbMoveToTrayWhenMinimized);
		this.pnlMain.Controls.Add(this.label5);
		this.pnlMain.Controls.Add(this.cmbLanguage);
		this.pnlMain.Controls.Add(this.btnLoadDefaults);
		this.pnlMain.Controls.Add(this.txtRecordKey);
		this.pnlMain.Controls.Add(this.txtActivationKey);
		this.pnlMain.Controls.Add(this.pnlPalette);
		this.pnlMain.Controls.Add(this.cbLoadLastProject);
		this.pnlMain.Controls.Add(this.label7);
		this.pnlMain.Controls.Add(this.cbEnableOnStart);
		this.pnlMain.Controls.Add(this.label6);
		this.pnlMain.Controls.Add(this.label4);
		this.pnlMain.Controls.Add(this.pnlTop);
		this.pnlMain.Controls.Add(this.label3);
		this.pnlMain.Controls.Add(this.btnSave);
		this.pnlMain.Controls.Add(this.label2);
		this.pnlMain.Controls.Add(this.label1);
		this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnlMain.Location = new System.Drawing.Point(0, 0);
		this.pnlMain.Name = "pnlMain";
		this.pnlMain.Size = new System.Drawing.Size(387, 391);
		this.pnlMain.TabIndex = 16;
		this.cbMoveToTrayWhenMinimized.AutoSize = true;
		this.cbMoveToTrayWhenMinimized.Location = new System.Drawing.Point(220, 116);
		this.cbMoveToTrayWhenMinimized.Name = "cbMoveToTrayWhenMinimized";
		this.cbMoveToTrayWhenMinimized.Size = new System.Drawing.Size(15, 14);
		this.cbMoveToTrayWhenMinimized.TabIndex = 68;
		this.cbMoveToTrayWhenMinimized.UseVisualStyleBackColor = true;
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(33, 115);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(166, 16);
		this.label5.TabIndex = 67;
		this.label5.Text = "Hide to tray when minimized :";
		this.cmbLanguage.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.cmbLanguage.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
		this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cmbLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cmbLanguage.FormattingEnabled = true;
		this.cmbLanguage.Location = new System.Drawing.Point(162, 148);
		this.cmbLanguage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cmbLanguage.Name = "cmbLanguage";
		this.cmbLanguage.Size = new System.Drawing.Size(180, 21);
		this.cmbLanguage.TabIndex = 66;
		this.btnLoadDefaults.BackColor = System.Drawing.SystemColors.ControlLight;
		this.btnLoadDefaults.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnLoadDefaults.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(91, 91, 91);
		this.btnLoadDefaults.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadDefaults.HoverColor = null;
		this.btnLoadDefaults.Location = new System.Drawing.Point(36, 310);
		this.btnLoadDefaults.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnLoadDefaults.Name = "btnLoadDefaults";
		this.btnLoadDefaults.Size = new System.Drawing.Size(111, 41);
		this.btnLoadDefaults.TabIndex = 65;
		this.btnLoadDefaults.Text = "Load Defaults";
		this.btnLoadDefaults.UseVisualStyleBackColor = false;
		this.txtRecordKey.Location = new System.Drawing.Point(162, 218);
		this.txtRecordKey.Multiline = true;
		this.txtRecordKey.Name = "txtRecordKey";
		this.txtRecordKey.ReadOnly = true;
		this.txtRecordKey.Size = new System.Drawing.Size(176, 25);
		this.txtRecordKey.TabIndex = 64;
		this.txtRecordKey.Text = "Click here to set key";
		this.txtRecordKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.txtActivationKey.Location = new System.Drawing.Point(162, 181);
		this.txtActivationKey.Multiline = true;
		this.txtActivationKey.Name = "txtActivationKey";
		this.txtActivationKey.ReadOnly = true;
		this.txtActivationKey.Size = new System.Drawing.Size(176, 25);
		this.txtActivationKey.TabIndex = 63;
		this.txtActivationKey.Text = "Click here to set key";
		this.txtActivationKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.pnlPalette.BackColor = System.Drawing.SystemColors.ControlLight;
		this.pnlPalette.Controls.Add(this.pnlLighterColor);
		this.pnlPalette.Controls.Add(this.pnlMidColor);
		this.pnlPalette.Controls.Add(this.pnlLightColor);
		this.pnlPalette.Controls.Add(this.pnlDarkColor);
		this.pnlPalette.Controls.Add(this.pnlTextColor);
		this.pnlPalette.Location = new System.Drawing.Point(162, 256);
		this.pnlPalette.Name = "pnlPalette";
		this.pnlPalette.Size = new System.Drawing.Size(122, 29);
		this.pnlPalette.TabIndex = 62;
		this.pnlLighterColor.BackColor = System.Drawing.Color.FromArgb(35, 35, 35);
		this.pnlLighterColor.Location = new System.Drawing.Point(8, 6);
		this.pnlLighterColor.Name = "pnlLighterColor";
		this.pnlLighterColor.Size = new System.Drawing.Size(16, 16);
		this.pnlLighterColor.TabIndex = 48;
		this.pnlMidColor.BackColor = System.Drawing.Color.FromArgb(15, 15, 15);
		this.pnlMidColor.Location = new System.Drawing.Point(52, 6);
		this.pnlMidColor.Name = "pnlMidColor";
		this.pnlMidColor.Size = new System.Drawing.Size(16, 16);
		this.pnlMidColor.TabIndex = 51;
		this.pnlLightColor.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
		this.pnlLightColor.Location = new System.Drawing.Point(30, 6);
		this.pnlLightColor.Name = "pnlLightColor";
		this.pnlLightColor.Size = new System.Drawing.Size(16, 16);
		this.pnlLightColor.TabIndex = 52;
		this.pnlDarkColor.BackColor = System.Drawing.Color.Black;
		this.pnlDarkColor.Location = new System.Drawing.Point(74, 6);
		this.pnlDarkColor.Name = "pnlDarkColor";
		this.pnlDarkColor.Size = new System.Drawing.Size(16, 16);
		this.pnlDarkColor.TabIndex = 50;
		this.pnlTextColor.BackColor = System.Drawing.Color.White;
		this.pnlTextColor.Location = new System.Drawing.Point(96, 6);
		this.pnlTextColor.Name = "pnlTextColor";
		this.pnlTextColor.Size = new System.Drawing.Size(16, 16);
		this.pnlTextColor.TabIndex = 49;
		this.cbLoadLastProject.AutoSize = true;
		this.cbLoadLastProject.Location = new System.Drawing.Point(220, 89);
		this.cbLoadLastProject.Name = "cbLoadLastProject";
		this.cbLoadLastProject.Size = new System.Drawing.Size(15, 14);
		this.cbLoadLastProject.TabIndex = 61;
		this.cbLoadLastProject.UseVisualStyleBackColor = true;
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(33, 88);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(143, 16);
		this.label7.TabIndex = 60;
		this.label7.Text = "Load last macro on start :";
		this.cbEnableOnStart.AutoSize = true;
		this.cbEnableOnStart.Location = new System.Drawing.Point(220, 62);
		this.cbEnableOnStart.Name = "cbEnableOnStart";
		this.cbEnableOnStart.Size = new System.Drawing.Size(15, 14);
		this.cbEnableOnStart.TabIndex = 59;
		this.cbEnableOnStart.UseVisualStyleBackColor = true;
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(33, 60);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(94, 16);
		this.label6.TabIndex = 58;
		this.label6.Text = "Enable on start :";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(33, 184);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(90, 16);
		this.label4.TabIndex = 57;
		this.label4.Text = "Activation key :";
		this.pnlTop.BackColor = System.Drawing.SystemColors.ControlLight;
		this.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnlTop.Controls.Add(this.btnClose);
		this.pnlTop.Controls.Add(this.lblTitle);
		this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnlTop.Font = new System.Drawing.Font("Century Gothic", 9f);
		this.pnlTop.Location = new System.Drawing.Point(0, 0);
		this.pnlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.pnlTop.MinimumSize = new System.Drawing.Size(2, 30);
		this.pnlTop.Name = "pnlTop";
		this.pnlTop.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
		this.pnlTop.Size = new System.Drawing.Size(385, 30);
		this.pnlTop.TabIndex = 16;
		this.btnClose.BackgroundImage = TGMacro.Properties.Resources.close_16;
		this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnClose.HoverImage = TGMacro.Properties.Resources.close_16_hover;
		this.btnClose.Location = new System.Drawing.Point(353, 0);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(20, 28);
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
		this.lblTitle.Size = new System.Drawing.Size(60, 30);
		this.lblTitle.TabIndex = 6;
		this.lblTitle.Text = "Settings";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(33, 221);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(73, 16);
		this.label3.TabIndex = 55;
		this.label3.Text = "Record key :";
		this.btnSave.BackColor = System.Drawing.SystemColors.ControlLight;
		this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(91, 91, 91);
		this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSave.HoverColor = null;
		this.btnSave.Image = TGMacro.Properties.Resources.save_16;
		this.btnSave.Location = new System.Drawing.Point(285, 310);
		this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(53, 41);
		this.btnSave.TabIndex = 35;
		this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btnSave.UseVisualStyleBackColor = false;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(33, 148);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(72, 16);
		this.label2.TabIndex = 54;
		this.label2.Text = "Language : ";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(33, 264);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(84, 16);
		this.label1.TabIndex = 53;
		this.label1.Text = "Color palette :";
		base.AcceptButton = this.btnSave;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 15f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(387, 391);
		base.Controls.Add(this.pnlMain);
		this.Font = new System.Drawing.Font("Century Gothic", 7.8f);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmSettings";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Settings";
		this.pnlMain.ResumeLayout(false);
		this.pnlMain.PerformLayout();
		this.pnlPalette.ResumeLayout(false);
		this.pnlTop.ResumeLayout(false);
		this.pnlTop.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnClose).EndInit();
		base.ResumeLayout(false);
	}
}
