using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CSInputs.Enums;
using TGMacro.Enums;
using TGMacro.Properties;
using TGMacro.Statics;
using TGMacro.SysModels;
using TGMacro.Triggers;

namespace TGMacro.UI.CustomControls;

public class ucMacroItem : UserControl
{
	private MacroItem macro;

	private IContainer components = null;

	private PictureBox picTriggerType;

	private Label lblMacroName;

	private HoverButton btnDelete;

	private HoverButton btnEdit;

	private PictureBox picRunning;

	private Panel pnlEnable;

	private ToolTip toolTip1;

	private Label lblKeyInfo;

	public MacroItem Macro
	{
		get
		{
			return macro;
		}
		set
		{
			macro = value;
			refreshUI();
		}
	}

	public ucMacroItem(MacroItem item, Control parent)
	{
		InitializeComponent();
		lblMacroName.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		lblKeyInfo.BackColor = AppSession.Settings.ColorPalette.LightColor;
		picTriggerType.BackColor = AppSession.Settings.ColorPalette.LightColor;
		lblKeyInfo.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		Macro = item;
		base.Parent = parent;
		registerEvents();
		toolTip1.SetToolTip(pnlEnable, AppSession.ActiveLanguage.enable_disable_macro);
		toolTip1.SetToolTip(btnEdit, AppSession.ActiveLanguage.edit_macro);
		toolTip1.SetToolTip(btnDelete, AppSession.ActiveLanguage.macro_delete_title);
	}

	private void updateMacroStatus()
	{
		Macro.Enable = !Macro.Enable;
		if (Macro.Enable)
		{
			pnlEnable.BackColor = Color.FromArgb(76, 213, 18);
			lblMacroName.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		}
		else
		{
			pnlEnable.BackColor = Color.FromArgb(230, 71, 30);
			lblMacroName.ForeColor = Color.DarkRed;
			picRunning.Visible = false;
		}
	}

	private void registerEvents()
	{
		foreach (Control control in base.Controls)
		{
			if (control.GetType() != typeof(HoverButton))
			{
				control.Click += delegate
				{
					updateMacroStatus();
				};
			}
		}
		base.Click += delegate
		{
			updateMacroStatus();
		};
		btnEdit.Click += delegate
		{
			bool enable = AppSession.Enable;
			AppSession.Enable = false;
			frmMacroEditor frmMacroEditor = new frmMacroEditor(Macro);
			if (frmMacroEditor.ShowDialog() == DialogResult.OK)
			{
				Macro = frmMacroEditor.Macro;
			}
			AppSession.Enable = enable;
		};
		btnDelete.Click += delegate
		{
			if (Messenger.Show(AppSession.ActiveLanguage.btn_macro_delete.Replace("{macroname}", Macro.Name), AppSession.ActiveLanguage.macro_delete_title, "YES", "NO") == DialogResult.Yes)
			{
				((FlowLayoutPanel)base.Parent).Controls.Remove(this);
				macro.Trigger.Dispose();
				macro.Dispose();
				macro = null;
				Dispose();
			}
		};
	}

	private void refreshUI()
	{
		Macro.Trigger.TriggerRunningStatusChanged += delegate
		{
			if (base.IsHandleCreated)
			{
				Invoke((MethodInvoker)delegate
				{
					picRunning.Visible = Macro.Trigger.RunState != TriggerRunState.Stop;
				});
			}
		};
		picTriggerType.BackgroundImage = macro.Icon;
		lblMacroName.Text = macro.Name;
		if (Macro.Trigger != null)
		{
			switch (Macro.Trigger.TriggerType)
			{
			case TriggerTypes.Mouse:
				lblKeyInfo.Text = (((Macro.Trigger as MouseTrigger).PrimaryKey != CSInputs.Enums.KeyboardKeys.None) ? ((Macro.Trigger as MouseTrigger).PrimaryKey.ToString() + " + " + (Macro.Trigger as MouseTrigger).SecondaryKey) : (Macro.Trigger as MouseTrigger).SecondaryKey.ToString());
				break;
			case TriggerTypes.Keyboard:
				lblKeyInfo.Text = (((Macro.Trigger as KeyboardTrigger).PrimaryKey != CSInputs.Enums.KeyboardKeys.None) ? ((Macro.Trigger as KeyboardTrigger).PrimaryKey.ToString() + " + " + (Macro.Trigger as KeyboardTrigger).SecondaryKey) : (Macro.Trigger as KeyboardTrigger).SecondaryKey.ToString());
				break;
			case TriggerTypes.Pixel:
				lblKeyInfo.Text = "By Color";
				break;
			}
		}
		updateMacroStatus();
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
		this.components = new System.ComponentModel.Container();
		this.lblMacroName = new System.Windows.Forms.Label();
		this.pnlEnable = new System.Windows.Forms.Panel();
		this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
		this.lblKeyInfo = new System.Windows.Forms.Label();
		this.picTriggerType = new System.Windows.Forms.PictureBox();
		this.picRunning = new System.Windows.Forms.PictureBox();
		this.btnDelete = new TGMacro.UI.CustomControls.HoverButton();
		this.btnEdit = new TGMacro.UI.CustomControls.HoverButton();
		((System.ComponentModel.ISupportInitialize)this.picTriggerType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picRunning).BeginInit();
		base.SuspendLayout();
		this.lblMacroName.AutoEllipsis = true;
		this.lblMacroName.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMacroName.Location = new System.Drawing.Point(20, 0);
		this.lblMacroName.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
		this.lblMacroName.Name = "lblMacroName";
		this.lblMacroName.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
		this.lblMacroName.Size = new System.Drawing.Size(247, 40);
		this.lblMacroName.TabIndex = 1;
		this.lblMacroName.Text = "Macro 0001";
		this.lblMacroName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.pnlEnable.BackColor = System.Drawing.Color.FromArgb(230, 71, 30);
		this.pnlEnable.Dock = System.Windows.Forms.DockStyle.Left;
		this.pnlEnable.Location = new System.Drawing.Point(0, 0);
		this.pnlEnable.Name = "pnlEnable";
		this.pnlEnable.Size = new System.Drawing.Size(20, 65);
		this.pnlEnable.TabIndex = 17;
		this.lblKeyInfo.AutoEllipsis = true;
		this.lblKeyInfo.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.lblKeyInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.lblKeyInfo.Location = new System.Drawing.Point(20, 40);
		this.lblKeyInfo.Margin = new System.Windows.Forms.Padding(3);
		this.lblKeyInfo.Name = "lblKeyInfo";
		this.lblKeyInfo.Padding = new System.Windows.Forms.Padding(34, 0, 0, 0);
		this.lblKeyInfo.Size = new System.Drawing.Size(247, 25);
		this.lblKeyInfo.TabIndex = 18;
		this.lblKeyInfo.Text = "Control + W";
		this.lblKeyInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.picTriggerType.BackColor = System.Drawing.Color.Transparent;
		this.picTriggerType.BackgroundImage = TGMacro.Properties.Resources.keyboard_16px;
		this.picTriggerType.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.picTriggerType.Location = new System.Drawing.Point(27, 39);
		this.picTriggerType.Name = "picTriggerType";
		this.picTriggerType.Size = new System.Drawing.Size(20, 20);
		this.picTriggerType.TabIndex = 0;
		this.picTriggerType.TabStop = false;
		this.picRunning.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.picRunning.BackColor = System.Drawing.Color.Transparent;
		this.picRunning.Image = TGMacro.Properties.Resources.run_16;
		this.picRunning.Location = new System.Drawing.Point(191, 3);
		this.picRunning.Name = "picRunning";
		this.picRunning.Size = new System.Drawing.Size(20, 20);
		this.picRunning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.picRunning.TabIndex = 16;
		this.picRunning.TabStop = false;
		this.picRunning.Visible = false;
		this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDelete.BackColor = System.Drawing.Color.Transparent;
		this.btnDelete.BackgroundImage = TGMacro.Properties.Resources.del_16;
		this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnDelete.FlatAppearance.BorderSize = 0;
		this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDelete.HoverColor = null;
		this.btnDelete.Location = new System.Drawing.Point(243, 3);
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(20, 20);
		this.btnDelete.TabIndex = 15;
		this.btnDelete.UseVisualStyleBackColor = false;
		this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnEdit.BackColor = System.Drawing.Color.Transparent;
		this.btnEdit.BackgroundImage = TGMacro.Properties.Resources.edit_16;
		this.btnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnEdit.FlatAppearance.BorderSize = 0;
		this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnEdit.HoverColor = null;
		this.btnEdit.Location = new System.Drawing.Point(217, 3);
		this.btnEdit.Name = "btnEdit";
		this.btnEdit.Size = new System.Drawing.Size(20, 20);
		this.btnEdit.TabIndex = 14;
		this.btnEdit.UseVisualStyleBackColor = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 17f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.picTriggerType);
		base.Controls.Add(this.picRunning);
		base.Controls.Add(this.btnDelete);
		base.Controls.Add(this.btnEdit);
		base.Controls.Add(this.lblMacroName);
		base.Controls.Add(this.lblKeyInfo);
		base.Controls.Add(this.pnlEnable);
		this.Font = new System.Drawing.Font("Century Gothic", 9f);
		base.Name = "ucMacroItem";
		base.Size = new System.Drawing.Size(267, 65);
		((System.ComponentModel.ISupportInitialize)this.picTriggerType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picRunning).EndInit();
		base.ResumeLayout(false);
	}
}
