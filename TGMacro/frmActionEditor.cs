using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using CSInputs.Enums;
using CSInputs.ReadInput;
using CSInputs.Structs;
using TGMacro.Actions;
using TGMacro.Enums;
using TGMacro.Interfaces;
using TGMacro.Properties;
using TGMacro.Statics;
using TGMacro.UI.CustomControls;

namespace TGMacro;

public class frmActionEditor : DoubleBufferForm
{
	public IMacroAction Action = null;

	private CSInputs.Enums.KeyboardKeys _secondaryKey;

	private CSInputs.Enums.MouseKeys _secondaryMouseKey;

	private bool _isListeningForTriggerKey = false;

	private IContainer components = null;

	private FlattenComboBox cbActionType;

	private NumericUpDown txtSleepAmount;

	private FlattenComboBox cbKeyStatus;

	private Panel pnlMousePos;

	private NumericUpDown mPosY;

	private Label lblPosition;

	private NumericUpDown mPosX;

	private Label label1;

	private Label label3;

	private Panel pnlSleepAmount;

	private Label lblWaitAmount;

	private Panel pnlActionType;

	private Label lblActionType;

	private Panel pnlKeyStatus;

	private Label lblKeyStatus;

	private FlowLayoutPanel flowLayoutPanel1;

	private Label lblGrabCurInfo;

	private Panel pnlMouseMoveMethod;

	private Label lblPositioning;

	private FlattenComboBox cbMouseMoveMethod;

	private Panel pnlKeys;

	private Label lblKey;

	private HoverButton btnSave;

	private TextBox txtKeySetter;

	private Panel pnlMain;

	private ToolTip toolTip1;

	public Panel pnlTop;

	public HoverPictureBox btnClose;

	public Label lblTitle;

	private Panel pnlText;

	private TextBox txtText;

	private Label lblText;

	private Label lblLoading;

	private PictureBox btnHelpActions;

	private PictureBox btnHelpActionEditor;

	private Panel pnlScrollAmount;

	private Label lblScrollAmount;

	private NumericUpDown txtScrollAmount;

	private Panel pnlDontMoveMouse;

	private CheckBox cbDontMoveMouse;

	private CSInputs.Enums.KeyboardKeys SecondaryKey
	{
		get
		{
			return _secondaryKey;
		}
		set
		{
			_secondaryKey = value;
			if (_secondaryKey != CSInputs.Enums.KeyboardKeys.None)
			{
				SecondaryMouseKey = CSInputs.Enums.MouseKeys.None;
			}
		}
	}

	private CSInputs.Enums.MouseKeys SecondaryMouseKey
	{
		get
		{
			return _secondaryMouseKey;
		}
		set
		{
			_secondaryMouseKey = value;
			if (_secondaryMouseKey != CSInputs.Enums.MouseKeys.None)
			{
				SecondaryKey = CSInputs.Enums.KeyboardKeys.None;
			}
		}
	}

	private bool IsListeningForKey
	{
		get
		{
			return _isListeningForTriggerKey;
		}
		set
		{
			_isListeningForTriggerKey = value;
			viewKeys();
			cbActionType.Enabled = !value;
		}
	}

	public frmActionEditor(IMacroAction action = null)
	{
		InitializeComponent();
		lblLoading.BringToFront();
		pnlTop.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		BackColor = AppSession.Settings.ColorPalette.LightColor;
		lblLoading.ForeColor = AppSession.Settings.ColorPalette.LightColor;
		ForeColor = AppSession.Settings.ColorPalette.TextColor;
		txtScrollAmount.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		txtScrollAmount.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		mPosX.BackColor = AppSession.Settings.ColorPalette.LighterColor;
		mPosX.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		mPosY.BackColor = AppSession.Settings.ColorPalette.LighterColor;
		mPosY.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		txtKeySetter.BackColor = AppSession.Settings.ColorPalette.LighterColor;
		txtKeySetter.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		txtSleepAmount.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		txtSleepAmount.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		txtText.BackColor = AppSession.Settings.ColorPalette.LighterColor;
		txtText.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		btnSave.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		btnSave.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		Action = action;
		base.Shown += FrmActionEditor_Shown;
	}

	private void LoadLanguage()
	{
		txtKeySetter.Text = AppSession.ActiveLanguage.text_click_here_to_set_key;
		lblActionType.Text = AppSession.ActiveLanguage.text_action_type;
		lblKey.Text = AppSession.ActiveLanguage.txt_keys;
		lblText.Text = AppSession.ActiveLanguage.string_text;
		lblKeyStatus.Text = AppSession.ActiveLanguage.text_key_status;
		lblPosition.Text = AppSession.ActiveLanguage.text_position;
		lblPositioning.Text = AppSession.ActiveLanguage.text_positioning;
		lblWaitAmount.Text = AppSession.ActiveLanguage.text_wait_amount;
		lblScrollAmount.Text = AppSession.ActiveLanguage.text_scroll_amount;
		cbKeyStatus.Items.Add(AppSession.ActiveLanguage.text_key_status_01);
		cbKeyStatus.Items.Add(AppSession.ActiveLanguage.text_key_status_02);
		cbKeyStatus.Items.Add(AppSession.ActiveLanguage.text_key_status_03);
		cbKeyStatus.SelectedIndex = 0;
		cbMouseMoveMethod.Items.Add(AppSession.ActiveLanguage.text_mouse_move_method_01_abs);
		cbMouseMoveMethod.Items.Add(AppSession.ActiveLanguage.text_mouse_move_method_02_rel);
		cbMouseMoveMethod.SelectedIndex = 0;
		lblGrabCurInfo.Text = AppSession.ActiveLanguage.info_grab_cursor_position;
		cbActionType.Items.Add(AppSession.ActiveLanguage.text_action_type_01_mouse_move);
		cbActionType.Items.Add(AppSession.ActiveLanguage.text_action_type_02_mouse_button);
		cbActionType.Items.Add(AppSession.ActiveLanguage.text_action_type_03_keyboard_button);
		cbActionType.Items.Add(AppSession.ActiveLanguage.text_action_type_04_wait);
		cbActionType.Items.Add(AppSession.ActiveLanguage.text_action_type_05_text);
		cbActionType.SelectedIndex = 0;
		lblGrabCurInfo.Text = AppSession.ActiveLanguage.text_grab_cursor_position;
		toolTip1.SetToolTip(cbMouseMoveMethod, AppSession.ActiveLanguage.text_mouse_move_method_info);
		toolTip1.SetToolTip(btnHelpActionEditor, AppSession.ActiveLanguage.btnHelp + "?");
		toolTip1.SetToolTip(btnHelpActions, AppSession.ActiveLanguage.btnHelp + "?");
		btnSave.Text = AppSession.ActiveLanguage.btnSave;
	}

	private void viewKeys()
	{
		if (!IsListeningForKey)
		{
			if (SecondaryKey != CSInputs.Enums.KeyboardKeys.None)
			{
				txtKeySetter.Text = SecondaryKey.ToString();
			}
			else if (SecondaryMouseKey != CSInputs.Enums.MouseKeys.None)
			{
				txtKeySetter.Text = SecondaryMouseKey.ToString();
				pnlScrollAmount.Visible = SecondaryMouseKey.ToString().ToLower().Contains("wheel");
				flowLayoutPanel1.Controls.SetChildIndex(pnlScrollAmount, 3);
			}
			else
			{
				txtKeySetter.Text = AppSession.ActiveLanguage.text_click_here_to_set_key;
			}
			txtKeySetter.Enabled = true;
		}
	}

	private void FrmActionEditor_Shown(object sender, EventArgs e)
	{
		Application.DoEvents();
		base.TopMost = AppSession.TopMost;
		registerEvents();
		LoadLanguage();
		AppSession.InputListener.KeyboardInputs += Hook_KeyboardInputs;
		AppSession.InputListener.MouseInputs += InputListener_MouseInputs;
		if (Action == null)
		{
			Action = new KeyboardKeyAction();
		}
		else
		{
			fillForm();
		}
		lblLoading.Visible = false;
	}

	private void fillForm()
	{
		switch (Action.ActionType)
		{
		case ActionTypes.MoveMouse:
			cbActionType.SelectedIndex = 0;
			cbMouseMoveMethod.SelectedIndex = (int)(Action as MouseMoveAction).MouseMoveMethod;
			mPosX.Value = (Action as MouseMoveAction).MousePos.X;
			mPosY.Value = (Action as MouseMoveAction).MousePos.Y;
			break;
		case ActionTypes.MouseButton:
		{
			cbActionType.SelectedIndex = 1;
			MouseKeyAction mouseKeyAction = (MouseKeyAction)Action;
			SecondaryMouseKey = mouseKeyAction.MouseKey;
			cbKeyStatus.SelectedIndex = (int)mouseKeyAction.KeyMethod;
			if (mouseKeyAction.MouseMoveMethod == MousePositioning.Relative && mouseKeyAction.MousePos.X == 0 && mouseKeyAction.MousePos.Y == 0)
			{
				cbDontMoveMouse.Checked = true;
			}
			cbMouseMoveMethod.SelectedIndex = (int)mouseKeyAction.MouseMoveMethod;
			mPosX.Value = mouseKeyAction.MousePos.X;
			mPosY.Value = mouseKeyAction.MousePos.Y;
			txtScrollAmount.Value = mouseKeyAction.ScrollAmount;
			viewKeys();
			break;
		}
		case ActionTypes.KeyboardKey:
		{
			cbActionType.SelectedIndex = 2;
			KeyboardKeyAction keyboardKeyAction = (KeyboardKeyAction)Action;
			cbKeyStatus.SelectedIndex = (int)keyboardKeyAction.KeyMethod;
			SecondaryKey = keyboardKeyAction.KeyboardKey;
			viewKeys();
			break;
		}
		case ActionTypes.Wait:
			cbActionType.SelectedIndex = 3;
			txtSleepAmount.Value = (Action as WaitAction).SleepAmount;
			break;
		case ActionTypes.Text:
			cbActionType.SelectedIndex = 4;
			txtText.Text = (Action as TextAction).Text;
			break;
		}
	}

	private void registerEvents()
	{
		cbDontMoveMouse.CheckedChanged += delegate
		{
			pnlMouseMoveMethod.Visible = !cbDontMoveMouse.Checked;
			pnlMousePos.Visible = !cbDontMoveMouse.Checked;
		};
		btnHelpActionEditor.Click += delegate
		{
			try
			{
				Process.Start("https://trksyln.net/projects/TGM-Gaming-Macro-(TGMacro)-4/How-to-use-5#actions");
			}
			catch
			{
			}
		};
		btnHelpActions.Click += delegate
		{
			try
			{
				Process.Start("https://trksyln.net/projects/TGM-Gaming-Macro-(TGMacro)-4/How-to-use-5#actions");
			}
			catch
			{
			}
		};
		txtKeySetter.MouseUp += keySetterClick;
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
		base.KeyDown += delegate(object s, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				Close();
			}
		};
		btnSave.Click += delegate
		{
			switch (cbActionType.SelectedIndex)
			{
			case 0:
				Action = new MouseMoveAction();
				(Action as MouseMoveAction).MouseMoveMethod = (MousePositioning)cbMouseMoveMethod.SelectedIndex;
				(Action as MouseMoveAction).MousePos = new Point((int)mPosX.Value, (int)mPosY.Value);
				break;
			case 1:
				if (SecondaryMouseKey == CSInputs.Enums.MouseKeys.None)
				{
					Messenger.Show(AppSession.ActiveLanguage.info_no_key_selected, "Error");
					return;
				}
				Action = new MouseKeyAction();
				(Action as MouseKeyAction).MouseKey = SecondaryMouseKey;
				if (SecondaryMouseKey == CSInputs.Enums.MouseKeys.MouseWheelForward || SecondaryMouseKey == CSInputs.Enums.MouseKeys.MouseWheelBackward || SecondaryMouseKey == CSInputs.Enums.MouseKeys.MouseWheelLeft || SecondaryMouseKey == CSInputs.Enums.MouseKeys.MouseWheelRight)
				{
					(Action as MouseKeyAction).MouseMoveMethod = MousePositioning.Relative;
					(Action as MouseKeyAction).MousePos = new Point(0, 0);
					(Action as MouseKeyAction).ScrollAmount = (short)txtScrollAmount.Value;
					(Action as MouseKeyAction).KeyMethod = KeyPressMethods.Press;
				}
				else if (cbDontMoveMouse.Checked)
				{
					(Action as MouseKeyAction).MouseMoveMethod = MousePositioning.Relative;
					(Action as MouseKeyAction).MousePos = new Point(0, 0);
					(Action as MouseKeyAction).KeyMethod = (KeyPressMethods)cbKeyStatus.SelectedIndex;
				}
				else
				{
					(Action as MouseKeyAction).MouseMoveMethod = (MousePositioning)cbMouseMoveMethod.SelectedIndex;
					(Action as MouseKeyAction).MousePos = new Point((int)mPosX.Value, (int)mPosY.Value);
					(Action as MouseKeyAction).KeyMethod = (KeyPressMethods)cbKeyStatus.SelectedIndex;
				}
				break;
			case 2:
				if (SecondaryKey == CSInputs.Enums.KeyboardKeys.None)
				{
					Messenger.Show(AppSession.ActiveLanguage.info_no_key_selected, "Error");
					return;
				}
				Action = new KeyboardKeyAction();
				(Action as KeyboardKeyAction).KeyMethod = (KeyPressMethods)cbKeyStatus.SelectedIndex;
				(Action as KeyboardKeyAction).KeyboardKey = SecondaryKey;
				break;
			case 3:
				Action = new WaitAction();
				(Action as WaitAction).SleepAmount = (int)txtSleepAmount.Value;
				break;
			case 4:
				if (txtText.TextLength < 1)
				{
					Messenger.Show(AppSession.ActiveLanguage.info_no_text, "Error");
					return;
				}
				Action = new TextAction();
				(Action as TextAction).Text = txtText.Text;
				break;
			}
			base.DialogResult = DialogResult.OK;
		};
		cbActionType.SelectedIndexChanged += delegate
		{
			SecondaryKey = CSInputs.Enums.KeyboardKeys.None;
			SecondaryMouseKey = CSInputs.Enums.MouseKeys.None;
			switch (cbActionType.SelectedIndex)
			{
			case 0:
				setPresetMouseMove();
				break;
			case 1:
				setPresetMouseKey();
				break;
			case 2:
				setPresetKeyboardKey();
				break;
			case 3:
				setPresetWait();
				break;
			case 4:
				setPresetText();
				break;
			}
		};
	}

	private void InputListener_MouseInputs(MouseData data, ref ModifierKey modifierKey)
	{
		if (IsListeningForKey && cbActionType.SelectedIndex == 1 && data.Flags == KeyFlags.Up)
		{
			SecondaryMouseKey = data.Key;
			IsListeningForKey = false;
		}
	}

	private void Hook_KeyboardInputs(KeyboardData data, ref ModifierKey modifierKey)
	{
		if (modifierKey.Alt && data.Key == CSInputs.Enums.KeyboardKeys.X && data.Flags == KeyFlags.Down)
		{
			mPosX.Value = Cursor.Position.X;
			mPosY.Value = Cursor.Position.Y;
		}
		if (IsListeningForKey && data.Key == CSInputs.Enums.KeyboardKeys.Escape)
		{
			IsListeningForKey = false;
		}
		else if (IsListeningForKey && cbActionType.SelectedIndex == 2)
		{
			SecondaryKey = data.Key;
			IsListeningForKey = false;
		}
	}

	private void keySetterClick(object sender, EventArgs e)
	{
		txtKeySetter.Enabled = false;
		if (!IsListeningForKey)
		{
			IsListeningForKey = !IsListeningForKey;
			if (IsListeningForKey)
			{
				txtKeySetter.Text = AppSession.ActiveLanguage.text_press_any_key_to_set;
			}
		}
	}

	private void reset()
	{
		pnlDontMoveMouse.Visible = false;
		pnlKeyStatus.Visible = false;
		pnlKeys.Visible = false;
		pnlMouseMoveMethod.Visible = false;
		pnlMousePos.Visible = false;
		pnlSleepAmount.Visible = false;
		pnlText.Visible = false;
		pnlScrollAmount.Visible = false;
		SecondaryKey = CSInputs.Enums.KeyboardKeys.None;
		SecondaryMouseKey = CSInputs.Enums.MouseKeys.None;
		viewKeys();
	}

	private void setPresetMouseKey()
	{
		reset();
		pnlKeys.Visible = true;
		pnlKeyStatus.Visible = true;
		pnlDontMoveMouse.Visible = true;
		pnlMouseMoveMethod.Visible = !cbDontMoveMouse.Checked;
		pnlMousePos.Visible = !cbDontMoveMouse.Checked;
		flowLayoutPanel1.Controls.SetChildIndex(pnlKeys, 1);
		flowLayoutPanel1.Controls.SetChildIndex(pnlKeyStatus, 2);
		flowLayoutPanel1.Controls.SetChildIndex(pnlScrollAmount, 3);
		flowLayoutPanel1.Controls.SetChildIndex(pnlDontMoveMouse, 4);
		flowLayoutPanel1.Controls.SetChildIndex(pnlMouseMoveMethod, 5);
		flowLayoutPanel1.Controls.SetChildIndex(pnlMousePos, 6);
	}

	private void setPresetKeyboardKey()
	{
		reset();
		pnlKeyStatus.Visible = true;
		pnlKeys.Visible = true;
	}

	private void setPresetMouseMove()
	{
		reset();
		pnlMouseMoveMethod.Visible = true;
		pnlMousePos.Visible = true;
	}

	private void setPresetWait()
	{
		reset();
		pnlSleepAmount.Visible = true;
	}

	private void setPresetText()
	{
		reset();
		pnlText.Visible = true;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TGMacro.frmActionEditor));
		this.cbActionType = new TGMacro.UI.CustomControls.FlattenComboBox();
		this.txtSleepAmount = new System.Windows.Forms.NumericUpDown();
		this.cbKeyStatus = new TGMacro.UI.CustomControls.FlattenComboBox();
		this.pnlMousePos = new System.Windows.Forms.Panel();
		this.lblGrabCurInfo = new System.Windows.Forms.Label();
		this.mPosY = new System.Windows.Forms.NumericUpDown();
		this.lblPosition = new System.Windows.Forms.Label();
		this.mPosX = new System.Windows.Forms.NumericUpDown();
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.pnlSleepAmount = new System.Windows.Forms.Panel();
		this.lblWaitAmount = new System.Windows.Forms.Label();
		this.pnlActionType = new System.Windows.Forms.Panel();
		this.btnHelpActions = new System.Windows.Forms.PictureBox();
		this.lblActionType = new System.Windows.Forms.Label();
		this.pnlKeyStatus = new System.Windows.Forms.Panel();
		this.lblKeyStatus = new System.Windows.Forms.Label();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.pnlDontMoveMouse = new System.Windows.Forms.Panel();
		this.cbDontMoveMouse = new System.Windows.Forms.CheckBox();
		this.pnlText = new System.Windows.Forms.Panel();
		this.txtText = new System.Windows.Forms.TextBox();
		this.lblText = new System.Windows.Forms.Label();
		this.pnlKeys = new System.Windows.Forms.Panel();
		this.txtKeySetter = new System.Windows.Forms.TextBox();
		this.lblKey = new System.Windows.Forms.Label();
		this.pnlScrollAmount = new System.Windows.Forms.Panel();
		this.lblScrollAmount = new System.Windows.Forms.Label();
		this.txtScrollAmount = new System.Windows.Forms.NumericUpDown();
		this.pnlMouseMoveMethod = new System.Windows.Forms.Panel();
		this.lblPositioning = new System.Windows.Forms.Label();
		this.cbMouseMoveMethod = new TGMacro.UI.CustomControls.FlattenComboBox();
		this.btnSave = new TGMacro.UI.CustomControls.HoverButton();
		this.pnlMain = new System.Windows.Forms.Panel();
		this.pnlTop = new System.Windows.Forms.Panel();
		this.btnHelpActionEditor = new System.Windows.Forms.PictureBox();
		this.btnClose = new TGMacro.UI.CustomControls.HoverPictureBox();
		this.lblTitle = new System.Windows.Forms.Label();
		this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
		this.lblLoading = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.txtSleepAmount).BeginInit();
		this.pnlMousePos.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.mPosY).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.mPosX).BeginInit();
		this.pnlSleepAmount.SuspendLayout();
		this.pnlActionType.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnHelpActions).BeginInit();
		this.pnlKeyStatus.SuspendLayout();
		this.flowLayoutPanel1.SuspendLayout();
		this.pnlDontMoveMouse.SuspendLayout();
		this.pnlText.SuspendLayout();
		this.pnlKeys.SuspendLayout();
		this.pnlScrollAmount.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.txtScrollAmount).BeginInit();
		this.pnlMouseMoveMethod.SuspendLayout();
		this.pnlMain.SuspendLayout();
		this.pnlTop.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnHelpActionEditor).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.btnClose).BeginInit();
		base.SuspendLayout();
		this.cbActionType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.cbActionType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
		this.cbActionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbActionType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cbActionType.FormattingEnabled = true;
		this.cbActionType.Location = new System.Drawing.Point(0, 19);
		this.cbActionType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbActionType.Name = "cbActionType";
		this.cbActionType.Size = new System.Drawing.Size(180, 23);
		this.cbActionType.TabIndex = 4;
		this.txtSleepAmount.Location = new System.Drawing.Point(6, 19);
		this.txtSleepAmount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtSleepAmount.Maximum = new decimal(new int[4] { 2147483647, 0, 0, 0 });
		this.txtSleepAmount.Name = "txtSleepAmount";
		this.txtSleepAmount.Size = new System.Drawing.Size(93, 22);
		this.txtSleepAmount.TabIndex = 23;
		this.txtSleepAmount.Value = new decimal(new int[4] { 500, 0, 0, 0 });
		this.cbKeyStatus.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.cbKeyStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
		this.cbKeyStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbKeyStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cbKeyStatus.FormattingEnabled = true;
		this.cbKeyStatus.Location = new System.Drawing.Point(0, 19);
		this.cbKeyStatus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbKeyStatus.Name = "cbKeyStatus";
		this.cbKeyStatus.Size = new System.Drawing.Size(198, 23);
		this.cbKeyStatus.TabIndex = 24;
		this.pnlMousePos.Controls.Add(this.lblGrabCurInfo);
		this.pnlMousePos.Controls.Add(this.mPosY);
		this.pnlMousePos.Controls.Add(this.lblPosition);
		this.pnlMousePos.Controls.Add(this.mPosX);
		this.pnlMousePos.Controls.Add(this.label1);
		this.pnlMousePos.Controls.Add(this.label3);
		this.pnlMousePos.Location = new System.Drawing.Point(0, 368);
		this.pnlMousePos.Margin = new System.Windows.Forms.Padding(0);
		this.pnlMousePos.Name = "pnlMousePos";
		this.pnlMousePos.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
		this.pnlMousePos.Size = new System.Drawing.Size(210, 69);
		this.pnlMousePos.TabIndex = 26;
		this.pnlMousePos.Visible = false;
		this.lblGrabCurInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.lblGrabCurInfo.Font = new System.Drawing.Font("Segoe UI", 7.5f);
		this.lblGrabCurInfo.ForeColor = System.Drawing.Color.Crimson;
		this.lblGrabCurInfo.Location = new System.Drawing.Point(0, 49);
		this.lblGrabCurInfo.Name = "lblGrabCurInfo";
		this.lblGrabCurInfo.Size = new System.Drawing.Size(210, 15);
		this.lblGrabCurInfo.TabIndex = 32;
		this.lblGrabCurInfo.Text = "(Alt+X get current cursor position)";
		this.lblGrabCurInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.mPosY.Location = new System.Drawing.Point(125, 24);
		this.mPosY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.mPosY.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.mPosY.Minimum = new decimal(new int[4] { 10000, 0, 0, -2147483648 });
		this.mPosY.Name = "mPosY";
		this.mPosY.Size = new System.Drawing.Size(75, 22);
		this.mPosY.TabIndex = 31;
		this.lblPosition.AutoSize = true;
		this.lblPosition.Location = new System.Drawing.Point(0, 0);
		this.lblPosition.Name = "lblPosition";
		this.lblPosition.Size = new System.Drawing.Size(60, 17);
		this.lblPosition.TabIndex = 27;
		this.lblPosition.Text = "Position :";
		this.mPosX.Location = new System.Drawing.Point(23, 24);
		this.mPosX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.mPosX.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.mPosX.Minimum = new decimal(new int[4] { 10000, 0, 0, -2147483648 });
		this.mPosX.Name = "mPosX";
		this.mPosX.Size = new System.Drawing.Size(75, 22);
		this.mPosX.TabIndex = 30;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(3, 25);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(21, 17);
		this.label1.TabIndex = 28;
		this.label1.Text = "X: ";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(100, 25);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(18, 17);
		this.label3.TabIndex = 29;
		this.label3.Text = "Y:";
		this.pnlSleepAmount.Controls.Add(this.lblWaitAmount);
		this.pnlSleepAmount.Controls.Add(this.txtSleepAmount);
		this.pnlSleepAmount.Location = new System.Drawing.Point(0, 437);
		this.pnlSleepAmount.Margin = new System.Windows.Forms.Padding(0);
		this.pnlSleepAmount.Name = "pnlSleepAmount";
		this.pnlSleepAmount.Size = new System.Drawing.Size(210, 50);
		this.pnlSleepAmount.TabIndex = 27;
		this.pnlSleepAmount.Visible = false;
		this.lblWaitAmount.AutoSize = true;
		this.lblWaitAmount.Location = new System.Drawing.Point(0, 0);
		this.lblWaitAmount.Name = "lblWaitAmount";
		this.lblWaitAmount.Size = new System.Drawing.Size(123, 17);
		this.lblWaitAmount.TabIndex = 27;
		this.lblWaitAmount.Text = "Scroll Amount : (px)";
		this.pnlActionType.Controls.Add(this.btnHelpActions);
		this.pnlActionType.Controls.Add(this.lblActionType);
		this.pnlActionType.Controls.Add(this.cbActionType);
		this.pnlActionType.Location = new System.Drawing.Point(0, 0);
		this.pnlActionType.Margin = new System.Windows.Forms.Padding(0);
		this.pnlActionType.Name = "pnlActionType";
		this.pnlActionType.Size = new System.Drawing.Size(210, 57);
		this.pnlActionType.TabIndex = 29;
		this.btnHelpActions.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnHelpActions.Image = TGMacro.Properties.Resources.help_20;
		this.btnHelpActions.Location = new System.Drawing.Point(185, 20);
		this.btnHelpActions.Name = "btnHelpActions";
		this.btnHelpActions.Size = new System.Drawing.Size(20, 20);
		this.btnHelpActions.TabIndex = 28;
		this.btnHelpActions.TabStop = false;
		this.lblActionType.AutoSize = true;
		this.lblActionType.Location = new System.Drawing.Point(0, 0);
		this.lblActionType.Name = "lblActionType";
		this.lblActionType.Size = new System.Drawing.Size(84, 17);
		this.lblActionType.TabIndex = 27;
		this.lblActionType.Text = "Action type :";
		this.pnlKeyStatus.Controls.Add(this.lblKeyStatus);
		this.pnlKeyStatus.Controls.Add(this.cbKeyStatus);
		this.pnlKeyStatus.Location = new System.Drawing.Point(0, 254);
		this.pnlKeyStatus.Margin = new System.Windows.Forms.Padding(0);
		this.pnlKeyStatus.Name = "pnlKeyStatus";
		this.pnlKeyStatus.Size = new System.Drawing.Size(210, 57);
		this.pnlKeyStatus.TabIndex = 29;
		this.pnlKeyStatus.Visible = false;
		this.lblKeyStatus.AutoSize = true;
		this.lblKeyStatus.Location = new System.Drawing.Point(0, 0);
		this.lblKeyStatus.Name = "lblKeyStatus";
		this.lblKeyStatus.Size = new System.Drawing.Size(73, 17);
		this.lblKeyStatus.TabIndex = 27;
		this.lblKeyStatus.Text = "Key status :";
		this.flowLayoutPanel1.AutoSize = true;
		this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.flowLayoutPanel1.Controls.Add(this.pnlActionType);
		this.flowLayoutPanel1.Controls.Add(this.pnlDontMoveMouse);
		this.flowLayoutPanel1.Controls.Add(this.pnlText);
		this.flowLayoutPanel1.Controls.Add(this.pnlKeys);
		this.flowLayoutPanel1.Controls.Add(this.pnlScrollAmount);
		this.flowLayoutPanel1.Controls.Add(this.pnlKeyStatus);
		this.flowLayoutPanel1.Controls.Add(this.pnlMouseMoveMethod);
		this.flowLayoutPanel1.Controls.Add(this.pnlMousePos);
		this.flowLayoutPanel1.Controls.Add(this.pnlSleepAmount);
		this.flowLayoutPanel1.Controls.Add(this.btnSave);
		this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(10, 38);
		this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(212, 532);
		this.flowLayoutPanel1.TabIndex = 30;
		this.pnlDontMoveMouse.Controls.Add(this.cbDontMoveMouse);
		this.pnlDontMoveMouse.Location = new System.Drawing.Point(0, 57);
		this.pnlDontMoveMouse.Margin = new System.Windows.Forms.Padding(0);
		this.pnlDontMoveMouse.Name = "pnlDontMoveMouse";
		this.pnlDontMoveMouse.Size = new System.Drawing.Size(210, 36);
		this.pnlDontMoveMouse.TabIndex = 36;
		this.pnlDontMoveMouse.Visible = false;
		this.cbDontMoveMouse.AutoSize = true;
		this.cbDontMoveMouse.ForeColor = System.Drawing.Color.Firebrick;
		this.cbDontMoveMouse.Location = new System.Drawing.Point(6, 7);
		this.cbDontMoveMouse.Name = "cbDontMoveMouse";
		this.cbDontMoveMouse.Size = new System.Drawing.Size(161, 21);
		this.cbDontMoveMouse.TabIndex = 29;
		this.cbDontMoveMouse.Text = "Don't move the mouse";
		this.cbDontMoveMouse.UseVisualStyleBackColor = true;
		this.pnlText.Controls.Add(this.txtText);
		this.pnlText.Controls.Add(this.lblText);
		this.pnlText.Location = new System.Drawing.Point(0, 93);
		this.pnlText.Margin = new System.Windows.Forms.Padding(0);
		this.pnlText.Name = "pnlText";
		this.pnlText.Size = new System.Drawing.Size(210, 57);
		this.pnlText.TabIndex = 34;
		this.pnlText.Visible = false;
		this.txtText.Location = new System.Drawing.Point(6, 20);
		this.txtText.Name = "txtText";
		this.txtText.Size = new System.Drawing.Size(194, 22);
		this.txtText.TabIndex = 28;
		this.lblText.AutoSize = true;
		this.lblText.Location = new System.Drawing.Point(0, 0);
		this.lblText.Name = "lblText";
		this.lblText.Size = new System.Drawing.Size(38, 17);
		this.lblText.TabIndex = 27;
		this.lblText.Text = "Text :";
		this.pnlKeys.Controls.Add(this.txtKeySetter);
		this.pnlKeys.Controls.Add(this.lblKey);
		this.pnlKeys.Location = new System.Drawing.Point(0, 150);
		this.pnlKeys.Margin = new System.Windows.Forms.Padding(0);
		this.pnlKeys.Name = "pnlKeys";
		this.pnlKeys.Size = new System.Drawing.Size(210, 54);
		this.pnlKeys.TabIndex = 33;
		this.pnlKeys.Visible = false;
		this.txtKeySetter.Location = new System.Drawing.Point(6, 20);
		this.txtKeySetter.Name = "txtKeySetter";
		this.txtKeySetter.ReadOnly = true;
		this.txtKeySetter.Size = new System.Drawing.Size(194, 22);
		this.txtKeySetter.TabIndex = 28;
		this.lblKey.AutoSize = true;
		this.lblKey.Location = new System.Drawing.Point(0, 0);
		this.lblKey.Name = "lblKey";
		this.lblKey.Size = new System.Drawing.Size(35, 17);
		this.lblKey.TabIndex = 27;
		this.lblKey.Text = "Key :";
		this.pnlScrollAmount.Controls.Add(this.lblScrollAmount);
		this.pnlScrollAmount.Controls.Add(this.txtScrollAmount);
		this.pnlScrollAmount.Location = new System.Drawing.Point(0, 204);
		this.pnlScrollAmount.Margin = new System.Windows.Forms.Padding(0);
		this.pnlScrollAmount.Name = "pnlScrollAmount";
		this.pnlScrollAmount.Size = new System.Drawing.Size(210, 50);
		this.pnlScrollAmount.TabIndex = 35;
		this.pnlScrollAmount.Visible = false;
		this.lblScrollAmount.AutoSize = true;
		this.lblScrollAmount.Location = new System.Drawing.Point(0, 0);
		this.lblScrollAmount.Name = "lblScrollAmount";
		this.lblScrollAmount.Size = new System.Drawing.Size(120, 17);
		this.lblScrollAmount.TabIndex = 27;
		this.lblScrollAmount.Text = "Wait amount : (ms)";
		this.txtScrollAmount.Location = new System.Drawing.Point(3, 19);
		this.txtScrollAmount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtScrollAmount.Maximum = new decimal(new int[4] { 32767, 0, 0, 0 });
		this.txtScrollAmount.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		this.txtScrollAmount.Name = "txtScrollAmount";
		this.txtScrollAmount.Size = new System.Drawing.Size(93, 22);
		this.txtScrollAmount.TabIndex = 23;
		this.txtScrollAmount.Value = new decimal(new int[4] { 60, 0, 0, 0 });
		this.pnlMouseMoveMethod.Controls.Add(this.lblPositioning);
		this.pnlMouseMoveMethod.Controls.Add(this.cbMouseMoveMethod);
		this.pnlMouseMoveMethod.Location = new System.Drawing.Point(0, 311);
		this.pnlMouseMoveMethod.Margin = new System.Windows.Forms.Padding(0);
		this.pnlMouseMoveMethod.Name = "pnlMouseMoveMethod";
		this.pnlMouseMoveMethod.Size = new System.Drawing.Size(210, 57);
		this.pnlMouseMoveMethod.TabIndex = 31;
		this.pnlMouseMoveMethod.Visible = false;
		this.lblPositioning.AutoSize = true;
		this.lblPositioning.Location = new System.Drawing.Point(0, 0);
		this.lblPositioning.Name = "lblPositioning";
		this.lblPositioning.Size = new System.Drawing.Size(78, 17);
		this.lblPositioning.TabIndex = 27;
		this.lblPositioning.Text = "Positioning :";
		this.cbMouseMoveMethod.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.cbMouseMoveMethod.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
		this.cbMouseMoveMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbMouseMoveMethod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cbMouseMoveMethod.FormattingEnabled = true;
		this.cbMouseMoveMethod.Location = new System.Drawing.Point(0, 19);
		this.cbMouseMoveMethod.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.cbMouseMoveMethod.Name = "cbMouseMoveMethod";
		this.cbMouseMoveMethod.Size = new System.Drawing.Size(198, 23);
		this.cbMouseMoveMethod.TabIndex = 24;
		this.btnSave.BackColor = System.Drawing.SystemColors.ControlLight;
		this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(91, 91, 91);
		this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSave.HoverColor = null;
		this.btnSave.Image = TGMacro.Properties.Resources.save_32;
		this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnSave.Location = new System.Drawing.Point(3, 489);
		this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(206, 41);
		this.btnSave.TabIndex = 34;
		this.btnSave.Text = "Save";
		this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
		this.btnSave.UseVisualStyleBackColor = false;
		this.pnlMain.AutoSize = true;
		this.pnlMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnlMain.Controls.Add(this.pnlTop);
		this.pnlMain.Controls.Add(this.flowLayoutPanel1);
		this.pnlMain.Location = new System.Drawing.Point(0, 0);
		this.pnlMain.MinimumSize = new System.Drawing.Size(228, 2);
		this.pnlMain.Name = "pnlMain";
		this.pnlMain.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
		this.pnlMain.Size = new System.Drawing.Size(228, 580);
		this.pnlMain.TabIndex = 33;
		this.pnlTop.BackColor = System.Drawing.SystemColors.ControlLight;
		this.pnlTop.Controls.Add(this.btnHelpActionEditor);
		this.pnlTop.Controls.Add(this.btnClose);
		this.pnlTop.Controls.Add(this.lblTitle);
		this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnlTop.Font = new System.Drawing.Font("Century Gothic", 9f);
		this.pnlTop.Location = new System.Drawing.Point(0, 0);
		this.pnlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.pnlTop.Name = "pnlTop";
		this.pnlTop.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
		this.pnlTop.Size = new System.Drawing.Size(226, 30);
		this.pnlTop.TabIndex = 31;
		this.btnHelpActionEditor.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnHelpActionEditor.Image = TGMacro.Properties.Resources.help_20;
		this.btnHelpActionEditor.Location = new System.Drawing.Point(170, 5);
		this.btnHelpActionEditor.Name = "btnHelpActionEditor";
		this.btnHelpActionEditor.Size = new System.Drawing.Size(20, 20);
		this.btnHelpActionEditor.TabIndex = 13;
		this.btnHelpActionEditor.TabStop = false;
		this.btnClose.BackgroundImage = TGMacro.Properties.Resources.close_16;
		this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnClose.HoverImage = TGMacro.Properties.Resources.close_16_hover;
		this.btnClose.Location = new System.Drawing.Point(196, 0);
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
		this.lblTitle.Size = new System.Drawing.Size(90, 30);
		this.lblTitle.TabIndex = 6;
		this.lblTitle.Text = "Action Editor";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblLoading.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblLoading.Font = new System.Drawing.Font("Century Gothic", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 162);
		this.lblLoading.Location = new System.Drawing.Point(0, 0);
		this.lblLoading.Name = "lblLoading";
		this.lblLoading.Size = new System.Drawing.Size(228, 416);
		this.lblLoading.TabIndex = 34;
		this.lblLoading.Text = "Loading..";
		this.lblLoading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AcceptButton = this.btnSave;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 17f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		base.ClientSize = new System.Drawing.Size(228, 513);
		base.Controls.Add(this.pnlMain);
		base.Controls.Add(this.lblLoading);
		this.Font = new System.Drawing.Font("Century Gothic", 9f);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		base.MaximizeBox = false;
		base.Name = "frmActionEditor";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "TGM Action Editor";
		((System.ComponentModel.ISupportInitialize)this.txtSleepAmount).EndInit();
		this.pnlMousePos.ResumeLayout(false);
		this.pnlMousePos.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.mPosY).EndInit();
		((System.ComponentModel.ISupportInitialize)this.mPosX).EndInit();
		this.pnlSleepAmount.ResumeLayout(false);
		this.pnlSleepAmount.PerformLayout();
		this.pnlActionType.ResumeLayout(false);
		this.pnlActionType.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnHelpActions).EndInit();
		this.pnlKeyStatus.ResumeLayout(false);
		this.pnlKeyStatus.PerformLayout();
		this.flowLayoutPanel1.ResumeLayout(false);
		this.pnlDontMoveMouse.ResumeLayout(false);
		this.pnlDontMoveMouse.PerformLayout();
		this.pnlText.ResumeLayout(false);
		this.pnlText.PerformLayout();
		this.pnlKeys.ResumeLayout(false);
		this.pnlKeys.PerformLayout();
		this.pnlScrollAmount.ResumeLayout(false);
		this.pnlScrollAmount.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.txtScrollAmount).EndInit();
		this.pnlMouseMoveMethod.ResumeLayout(false);
		this.pnlMouseMoveMethod.PerformLayout();
		this.pnlMain.ResumeLayout(false);
		this.pnlMain.PerformLayout();
		this.pnlTop.ResumeLayout(false);
		this.pnlTop.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnHelpActionEditor).EndInit();
		((System.ComponentModel.ISupportInitialize)this.btnClose).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
