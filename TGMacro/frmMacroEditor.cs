using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CSInputs.Enums;
using CSInputs.ReadInput;
using CSInputs.Structs;
using TGMacro.Actions;
using TGMacro.Enums;
using TGMacro.Interfaces;
using TGMacro.Properties;
using TGMacro.Statics;
using TGMacro.SysModels;
using TGMacro.Triggers;
using TGMacro.UI.CustomControls;

namespace TGMacro;

public class frmMacroEditor : DoubleBufferForm
{
	private bool IsTesting = false;

	private bool _isRecording = false;

	private readonly Stopwatch st = new Stopwatch();

	private CSInputs.Enums.KeyboardKeys _secondaryKey;

	private CSInputs.Enums.MouseKeys _secondaryMouseKey;

	private CSInputs.Enums.KeyboardKeys PrimaryKey;

	private bool _isListeningForTriggerKey = false;

	private bool isSaved = true;

	public MacroItem Macro;

	private readonly bool LoadScript = false;

	private ActionTable actionTable;

	private Thread TestActionThread;

	private IContainer components = null;

	private GroupBox grpTrigger;

	private GroupBox grpActions;

	private HoverButton btnActionMoveUp;

	private HoverButton btnActionMoveDown;

	private TextBox txtMacroName;

	private FlattenComboBox cbTriggerType;

	private Label lblTrgKeys;

	private Label lblTrgMethod;

	private FlattenComboBox cbTriggerMethod;

	private Label lblTrgType;

	private HoverButton btnSave;

	private Panel pnl_keys;

	private Panel pnl_colors;

	private Label label3;

	private Label label1;

	private Label lblColorPos;

	private ColorDialog colorDialog1;

	private Panel pnlSelectedColor;

	private Label lblColor;

	private NumericUpDown mPosY;

	private NumericUpDown mPosX;

	private Label lblGrabInfo;

	private DatagridDoubleBuffer dtActions;

	private HoverButton btnAddAction;

	private HoverButton btnEditAction;

	private HoverButton btnDeleteAction;

	private HoverButton btnClone;

	private ToolTip toolTip1;

	private GroupBox grpName;

	private HoverButton btnTest;

	private HoverButton btnImportScript;

	private TextBox txtKeySetter;

	private Panel pnlMain;

	private GroupBox grpRecord;

	private Label lblMouseCapPos;

	private FlattenComboBox cbMousePositioning;

	private HoverButton btnClear;

	public Panel pnlTop;

	public HoverPictureBox btnClose;

	public Label lblTitle;

	private CheckBox cbRecordDelays;

	private CheckBox cbRecordKeyboardKeys;

	private CheckBox cbRecordMouseKeys;

	private CheckBox cbRecordMouseMovements;

	private Label lblRecordThis;

	private Label lblGrabInfo2;

	private HoverButton btnRecord;

	private ProgressBar progressBar1;

	private HoverButton btnAdjustTiming;

	private HoverButton btnMultiAction;

	public HoverPictureBox btnMin;

	private Label lblLoading;

	private PictureBox btnHelpMacroEditor;

	private PictureBox btnHelpTriggers;

	private NumericUpDown txtColorTolerance;

	private Label label4;

	private Label label2;

	private CheckBox cbTriggerOnce;

	private Label label5;

	private Label lblActionInfo;

	public bool IsRecording
	{
		get
		{
			return _isRecording;
		}
		set
		{
			_isRecording = value;
			recordCheck();
		}
	}

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
			txtMacroName.Enabled = !value;
			viewKeysOnKeySetterTextBox();
		}
	}

	private void recordCheck()
	{
		if (IsRecording)
		{
			btnRecord.Image = Resources.stop_record_16;
			btnRecord.Text = AppSession.ActiveLanguage.btn_record_on + "  { " + ((AppSession.Settings.RecordKeyModifier != 0) ? (((CSInputs.Enums.KeyboardKeys)AppSession.Settings.RecordKeyModifier/*cast due to .constrained prefix*/).ToString() + " + " + (CSInputs.Enums.KeyboardKeys)AppSession.Settings.RecordKey/*cast due to .constrained prefix*/) : ((CSInputs.Enums.KeyboardKeys)AppSession.Settings.RecordKey/*cast due to .constrained prefix*/).ToString()) + " }  ";
		}
		else
		{
			btnRecord.Image = Resources.record_16;
			btnRecord.Text = AppSession.ActiveLanguage.btn_record_off + "  { " + ((AppSession.Settings.RecordKeyModifier != 0) ? (((CSInputs.Enums.KeyboardKeys)AppSession.Settings.RecordKeyModifier/*cast due to .constrained prefix*/).ToString() + " + " + (CSInputs.Enums.KeyboardKeys)AppSession.Settings.RecordKey/*cast due to .constrained prefix*/) : ((CSInputs.Enums.KeyboardKeys)AppSession.Settings.RecordKey/*cast due to .constrained prefix*/).ToString()) + " }  ";
		}
	}

	private void addAction(IMacroAction action)
	{
		actionTable.Rows.Add(action, 0, action.Icon, action.VisibleText, action.VisibleValue);
		if (dtActions.SelectedRows.Count > 0)
		{
			dtActions.FirstDisplayedScrollingRowIndex = dtActions.Rows[dtActions.Rows.Count - 1].Index;
		}
	}

	private void Record()
	{
		if (IsRecording)
		{
			st.Start();
		}
		else
		{
			st.Reset();
			st.Stop();
		}
		IsRecording = !IsRecording;
	}

	public frmMacroEditor(MacroItem macro = null, bool loadScript = false)
	{
		InitializeComponent();
		lblLoading.BringToFront();
		pnlTop.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		lblLoading.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		BackColor = AppSession.Settings.ColorPalette.LightColor;
		txtKeySetter.BackColor = AppSession.Settings.ColorPalette.LighterColor;
		txtMacroName.BackColor = AppSession.Settings.ColorPalette.LighterColor;
		dtActions.BackgroundColor = AppSession.Settings.ColorPalette.LightColor;
		dtActions.ColumnHeadersDefaultCellStyle.BackColor = AppSession.Settings.ColorPalette.MidColor;
		dtActions.ColumnHeadersDefaultCellStyle.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		dtActions.DefaultCellStyle.BackColor = AppSession.Settings.ColorPalette.LightColor;
		dtActions.DefaultCellStyle.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		dtActions.DefaultCellStyle.SelectionBackColor = AppSession.Settings.ColorPalette.DarkColor;
		dtActions.DefaultCellStyle.SelectionForeColor = AppSession.Settings.ColorPalette.TextColor;
		btnRecord.BackColor = AppSession.Settings.ColorPalette.LighterColor;
		btnTest.BackColor = AppSession.Settings.ColorPalette.LighterColor;
		btnSave.BackColor = AppSession.Settings.ColorPalette.LighterColor;
		grpActions.BackColor = AppSession.Settings.ColorPalette.LightColor;
		grpName.BackColor = AppSession.Settings.ColorPalette.LightColor;
		grpRecord.BackColor = AppSession.Settings.ColorPalette.LightColor;
		grpTrigger.BackColor = AppSession.Settings.ColorPalette.LightColor;
		txtMacroName.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		txtKeySetter.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		ForeColor = AppSession.Settings.ColorPalette.TextColor;
		pnlMain.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		pnlSelectedColor.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		pnlTop.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		pnl_colors.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		pnl_keys.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		dtActions.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		grpActions.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		grpName.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		grpRecord.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		grpTrigger.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		base.TopMost = AppSession.TopMost;
		Macro = macro;
		LoadScript = loadScript;
		base.Shown += FrmMacroEditor_Shown;
	}

	private void LoadLanguage()
	{
		grpActions.Text = AppSession.ActiveLanguage.text_actions;
		grpName.Text = AppSession.ActiveLanguage.text_macro_name;
		grpTrigger.Text = AppSession.ActiveLanguage.text_triggering;
		toolTip1.SetToolTip(btnAddAction, AppSession.ActiveLanguage.btnAddAction);
		toolTip1.SetToolTip(btnEditAction, AppSession.ActiveLanguage.btnEditActions);
		toolTip1.SetToolTip(btnClone, AppSession.ActiveLanguage.btnCloneActions);
		toolTip1.SetToolTip(btnDeleteAction, AppSession.ActiveLanguage.btnDeleteActions);
		toolTip1.SetToolTip(btnActionMoveUp, AppSession.ActiveLanguage.btnMoveActionUp);
		toolTip1.SetToolTip(btnActionMoveDown, AppSession.ActiveLanguage.btnMoveActionsDown);
		toolTip1.SetToolTip(btnImportScript, AppSession.ActiveLanguage.btnImportActionsFromScript);
		toolTip1.SetToolTip(btnRecord, AppSession.ActiveLanguage.btnRecord_off);
		toolTip1.SetToolTip(btnClear, AppSession.ActiveLanguage.btnClearActions);
		toolTip1.SetToolTip(btnAdjustTiming, AppSession.ActiveLanguage.text_adjust_timing);
		toolTip1.SetToolTip(btnMultiAction, AppSession.ActiveLanguage.btnMultiAdd);
		toolTip1.SetToolTip(btnHelpTriggers, AppSession.ActiveLanguage.btnHelp + "?");
		toolTip1.SetToolTip(btnHelpMacroEditor, AppSession.ActiveLanguage.btnHelp + "?");
		lblTrgKeys.Text = AppSession.ActiveLanguage.txt_keys;
		lblTrgMethod.Text = AppSession.ActiveLanguage.txt_when;
		lblTrgType.Text = AppSession.ActiveLanguage.text_trigger_action_by;
		lblGrabInfo.Text = AppSession.ActiveLanguage.info_color_box;
		lblGrabInfo2.Text = AppSession.ActiveLanguage.info_getting_color_and_position;
		lblColor.Text = AppSession.ActiveLanguage.text_color;
		lblColorPos.Text = AppSession.ActiveLanguage.text_color_position;
		cbTriggerType.Items.Clear();
		cbTriggerType.Items.Add(AppSession.ActiveLanguage.text_key_events);
		cbTriggerType.Items.Add(AppSession.ActiveLanguage.text_pixel_events);
		btnSave.Text = AppSession.ActiveLanguage.btnSave;
		btnTest.Text = AppSession.ActiveLanguage.btnTest;
		cbMousePositioning.Items.Add(AppSession.ActiveLanguage.text_mouse_move_method_01_abs);
		cbMousePositioning.Items.Add(AppSession.ActiveLanguage.text_mouse_move_method_02_rel);
		cbMousePositioning.SelectedIndex = 0;
		grpRecord.Text = AppSession.ActiveLanguage.text_record_settings;
		lblMouseCapPos.Text = AppSession.ActiveLanguage.text_capture_mouse_pos;
		txtKeySetter.Text = AppSession.ActiveLanguage.text_click_here_to_set_key;
		lblRecordThis.Text = AppSession.ActiveLanguage.text_record;
		cbRecordDelays.Text = AppSession.ActiveLanguage.text_keys;
		cbRecordKeyboardKeys.Text = AppSession.ActiveLanguage.text_keyboard_keys;
		cbRecordMouseKeys.Text = AppSession.ActiveLanguage.text_mouse_buttons;
		cbRecordMouseMovements.Text = AppSession.ActiveLanguage.text_mouse_movements;
		btnRecord.Text = AppSession.ActiveLanguage.btn_record_off + "  { " + ((AppSession.Settings.RecordKeyModifier != 0) ? (((CSInputs.Enums.KeyboardKeys)AppSession.Settings.RecordKeyModifier/*cast due to .constrained prefix*/).ToString() + " + " + (CSInputs.Enums.KeyboardKeys)AppSession.Settings.RecordKey/*cast due to .constrained prefix*/) : ((CSInputs.Enums.KeyboardKeys)AppSession.Settings.RecordKey/*cast due to .constrained prefix*/).ToString()) + " }  ";
	}

	private void setDataSource()
	{
		actionTable = new ActionTable();
		actionTable.Columns.Add("", typeof(IMacroAction));
		actionTable.Columns.Add("No", typeof(int));
		actionTable.Columns.Add(" ", typeof(Image));
		actionTable.Columns.Add(AppSession.ActiveLanguage.text_action, typeof(string));
		actionTable.Columns.Add(AppSession.ActiveLanguage.text_value, typeof(string));
		dtActions.DataSource = actionTable;
		dtActions.ScrollBars = ScrollBars.Vertical;
	}

	private void updateActionInfoText()
	{
		int num = (from t in actionTable.AsEnumerable()
			where t.Field<IMacroAction>(0).ActionType == ActionTypes.Wait
			select (t.Field<IMacroAction>(0) as WaitAction).SleepAmount).Sum();
		lblActionInfo.Text = $"Action Count : {dtActions.Rows.Count.ToString()}     Total Wait : {num} ms  ({((double)num / 1000.0).ToString():n0} sec)";
	}

	private void viewKeysOnKeySetterTextBox()
	{
		if (!IsListeningForKey)
		{
			if (PrimaryKey != CSInputs.Enums.KeyboardKeys.None && SecondaryKey != CSInputs.Enums.KeyboardKeys.None)
			{
				txtKeySetter.Text = PrimaryKey.ToString() + " + " + SecondaryKey;
			}
			else if (PrimaryKey == CSInputs.Enums.KeyboardKeys.None && SecondaryKey != CSInputs.Enums.KeyboardKeys.None)
			{
				txtKeySetter.Text = SecondaryKey.ToString();
			}
			else if (PrimaryKey != CSInputs.Enums.KeyboardKeys.None && SecondaryMouseKey != CSInputs.Enums.MouseKeys.None)
			{
				txtKeySetter.Text = PrimaryKey.ToString() + " + " + SecondaryMouseKey;
			}
			else if (PrimaryKey == CSInputs.Enums.KeyboardKeys.None && SecondaryMouseKey != CSInputs.Enums.MouseKeys.None)
			{
				txtKeySetter.Text = SecondaryMouseKey.ToString();
			}
			else
			{
				txtKeySetter.Text = AppSession.ActiveLanguage.text_click_here_to_set_key;
			}
			txtKeySetter.Enabled = true;
		}
	}

	private void FrmMacroEditor_Shown(object sender, EventArgs e)
	{
		Application.DoEvents();
		LoadLanguage();
		RegisterEvents();
		setDataSource();
		loadRecordSettings();
		actionTable.DataChanged += delegate
		{
			updateActionInfoText();
		};
		if (Macro == null)
		{
			Macro = new MacroItem();
			txtMacroName.Text = "Macro " + (AppSession.Macros.Count + 1);
			setKeyEventPreset();
			if (LoadScript)
			{
				actionTable.LineNumberUpdateEnabled = false;
				ScriptParser.LoadScriptTo(actionTable);
				actionTable.LineNumberUpdateEnabled = true;
			}
		}
		else
		{
			FillTheForm();
		}
		AppSession.InputListener.KeyboardInputs += KeyboardInputs;
		AppSession.InputListener.MouseInputs += MouseInputs;
		AppSession.InputListener.MouseMovements += MouseMovements;
		lblLoading.Visible = false;
	}

	private void loadRecordSettings()
	{
		cbRecordKeyboardKeys.Checked = AppSession.Settings.RecordKeyboardKeys;
		cbRecordMouseKeys.Checked = AppSession.Settings.RecordMouseKeys;
		cbRecordMouseMovements.Checked = AppSession.Settings.RecordMouseMovements;
		cbRecordDelays.Checked = AppSession.Settings.RecordDelays;
	}

	private void setKeyEventPreset()
	{
		cbTriggerMethod.Items.Clear();
		cbTriggerMethod.Items.Add(AppSession.ActiveLanguage.text_key_press);
		cbTriggerMethod.Items.Add(AppSession.ActiveLanguage.text_key_hold);
		cbTriggerMethod.Items.Add(AppSession.ActiveLanguage.text_key_toggle);
		cbTriggerMethod.Items.Add(AppSession.ActiveLanguage.text_key_release);
		cbTriggerType.SelectedIndex = 0;
		cbTriggerMethod.SelectedIndex = 2;
		pnl_colors.Visible = false;
		pnl_keys.Visible = true;
	}

	private void setPresetColor()
	{
		cbTriggerMethod.Items.Clear();
		cbTriggerMethod.Items.Add(AppSession.ActiveLanguage.text_color_matches);
		cbTriggerMethod.Items.Add(AppSession.ActiveLanguage.text_color_not_matches);
		cbTriggerType.SelectedIndex = 1;
		cbTriggerMethod.SelectedIndex = 0;
		pnl_colors.Visible = true;
		pnl_keys.Visible = false;
	}

	private void FillTheForm()
	{
		txtMacroName.Text = Macro.Name;
		if (Macro.Trigger != null)
		{
			switch (Macro.Trigger.TriggerType)
			{
			case TriggerTypes.Mouse:
			{
				setKeyEventPreset();
				MouseTrigger mouseTrigger = (MouseTrigger)Macro.Trigger;
				cbTriggerMethod.SelectedIndex = (int)mouseTrigger.Method;
				PrimaryKey = mouseTrigger.PrimaryKey;
				SecondaryMouseKey = mouseTrigger.SecondaryKey;
				viewKeysOnKeySetterTextBox();
				break;
			}
			case TriggerTypes.Keyboard:
			{
				setKeyEventPreset();
				KeyboardTrigger keyboardTrigger = (KeyboardTrigger)Macro.Trigger;
				cbTriggerMethod.SelectedIndex = (int)keyboardTrigger.Method;
				PrimaryKey = keyboardTrigger.PrimaryKey;
				SecondaryKey = keyboardTrigger.SecondaryKey;
				viewKeysOnKeySetterTextBox();
				break;
			}
			case TriggerTypes.Pixel:
			{
				setPresetColor();
				ColorTrigger colorTrigger = (ColorTrigger)Macro.Trigger;
				cbTriggerMethod.SelectedIndex = (int)colorTrigger.Method;
				pnlSelectedColor.BackColor = colorTrigger.Color;
				cbTriggerOnce.Checked = colorTrigger.TriggerOnce;
				txtColorTolerance.Value = colorTrigger.ColorTolerance;
				mPosX.Value = colorTrigger.Pos.X;
				mPosY.Value = colorTrigger.Pos.Y;
				break;
			}
			}
		}
		if (Macro.Trigger.Actions.Count <= 0)
		{
			return;
		}
		actionTable.LineNumberUpdateEnabled = false;
		foreach (IMacroAction action in Macro.Trigger.Actions)
		{
			actionTable.Rows.Add(action, 0, action.Icon, action.VisibleText, action.VisibleValue);
		}
		actionTable.LineNumberUpdateEnabled = true;
	}

	private void MouseMovements(MouseData data, ref ModifierKey modifierKey)
	{
		if (IsRecording && cbRecordMouseMovements.Checked)
		{
			if (cbRecordDelays.Checked)
			{
				addAction(new WaitAction
				{
					SleepAmount = (int)st.ElapsedMilliseconds
				});
			}
			addAction(new MouseMoveAction
			{
				MouseMoveMethod = ((cbMousePositioning.SelectedIndex != 0) ? MousePositioning.Relative : MousePositioning.Absolute),
				MousePos = ((cbMousePositioning.SelectedIndex == 0) ? Cursor.Position : new Point(data.PositionRelative.X, data.PositionRelative.Y))
			});
			st.Restart();
		}
	}

	private void MouseInputs(MouseData data, ref ModifierKey modifierKey)
	{
		if (IsRecording && cbRecordMouseKeys.Checked)
		{
			if (st.ElapsedMilliseconds > 10 && cbRecordDelays.Checked)
			{
				addAction(new WaitAction
				{
					SleepAmount = (int)st.ElapsedMilliseconds
				});
			}
			addAction(new MouseKeyAction
			{
				MouseKey = data.Key,
				KeyMethod = ((data.Flags != KeyFlags.Up) ? KeyPressMethods.Down : KeyPressMethods.Up),
				MousePos = ((cbMousePositioning.SelectedIndex == 0) ? Cursor.Position : new Point(data.PositionRelative.X, data.PositionRelative.Y)),
				MouseMoveMethod = ((cbMousePositioning.SelectedIndex != 0) ? MousePositioning.Relative : MousePositioning.Absolute)
			});
			st.Restart();
		}
		if (IsListeningForKey && cbTriggerType.SelectedIndex == 0 && data.Flags == KeyFlags.Up)
		{
			PrimaryKey = SharedMethods.ModifierKeyToKey(modifierKey);
			SecondaryMouseKey = data.Key;
			IsListeningForKey = false;
		}
	}

	private void KeyboardInputs(KeyboardData data, ref ModifierKey modifierKey)
	{
		if ((int)SharedMethods.ModifierKeyToKey(modifierKey) == AppSession.Settings.RecordKeyModifier && (int)data.Key == AppSession.Settings.RecordKey && data.Flags == KeyFlags.Up)
		{
			Record();
		}
		if (IsTesting && data.Key == CSInputs.Enums.KeyboardKeys.Escape)
		{
			CancelTesting();
		}
		if (IsRecording && cbRecordKeyboardKeys.Checked)
		{
			if (st.ElapsedMilliseconds > 49 && cbRecordDelays.Checked)
			{
				addAction(new WaitAction
				{
					SleepAmount = (int)st.ElapsedMilliseconds
				});
			}
			if (modifierKey.Windows && data.Flags == KeyFlags.Up)
			{
				addAction(new KeyboardKeyAction
				{
					KeyboardKey = data.Key,
					KeyMethod = KeyPressMethods.Press
				});
			}
			else
			{
				addAction(new KeyboardKeyAction
				{
					KeyboardKey = data.Key,
					KeyMethod = ((data.Flags != KeyFlags.Up) ? KeyPressMethods.Down : KeyPressMethods.Up)
				});
			}
			st.Restart();
			return;
		}
		if (IsListeningForKey && data.Key == CSInputs.Enums.KeyboardKeys.Escape)
		{
			IsListeningForKey = false;
			return;
		}
		if (IsListeningForKey && cbTriggerType.SelectedIndex == 0)
		{
			if (!Enum.IsDefined(typeof(KeysForTrigger), (KeysForTrigger)data.Key))
			{
				return;
			}
			PrimaryKey = SharedMethods.ModifierKeyToKey(modifierKey);
			SecondaryKey = data.Key;
			IsListeningForKey = false;
		}
		if (cbTriggerType.SelectedIndex == 1)
		{
			if (modifierKey.Alt && data.Key == CSInputs.Enums.KeyboardKeys.C && data.Flags == KeyFlags.Down)
			{
				pnlSelectedColor.BackColor = SharedMethods.GetColorAt(Cursor.Position);
			}
			if (modifierKey.Alt && data.Key == CSInputs.Enums.KeyboardKeys.X && data.Flags == KeyFlags.Down)
			{
				mPosX.Value = Cursor.Position.X;
				mPosY.Value = Cursor.Position.Y;
			}
		}
	}

	private IMacroAction[] GetActions()
	{
		List<IMacroAction> list = new List<IMacroAction>();
		for (int i = 0; i < actionTable.Rows.Count; i++)
		{
			list.Add((IMacroAction)actionTable.Rows[i][0]);
		}
		return list.ToArray();
	}

	private void RegisterEvents()
	{
		cbRecordDelays.CheckedChanged += delegate
		{
			saveRecordSettings();
		};
		cbRecordKeyboardKeys.CheckedChanged += delegate
		{
			saveRecordSettings();
		};
		cbRecordMouseKeys.CheckedChanged += delegate
		{
			saveRecordSettings();
		};
		cbRecordMouseMovements.CheckedChanged += delegate
		{
			saveRecordSettings();
		};
		btnHelpMacroEditor.Click += delegate
		{
			try
			{
				Process.Start("https://trksyln.net/projects/TGM-Gaming-Macro-(TGMacro)-4/How-to-use-5#macroeditor");
			}
			catch
			{
			}
		};
		btnHelpTriggers.Click += delegate
		{
			try
			{
				Process.Start("https://trksyln.net/projects/TGM-Gaming-Macro-(TGMacro)-4/How-to-use-5#triggers");
			}
			catch
			{
			}
		};
		dtActions.CellDoubleClick += delegate(object s, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				IMacroAction action = (IMacroAction)actionTable.Rows[e.RowIndex][0];
				frmActionEditor frmActionEditor2 = new frmActionEditor(action);
				if (frmActionEditor2.ShowDialog() == DialogResult.OK)
				{
					actionTable.Rows[dtActions.SelectedRows[0].Index][0] = frmActionEditor2.Action;
					actionTable.Rows[dtActions.SelectedRows[0].Index][2] = frmActionEditor2.Action.Icon;
					actionTable.Rows[dtActions.SelectedRows[0].Index][3] = frmActionEditor2.Action.VisibleText;
					actionTable.Rows[dtActions.SelectedRows[0].Index][4] = frmActionEditor2.Action.VisibleValue;
				}
				dtActions.FirstDisplayedScrollingRowIndex = dtActions.SelectedRows[0].Index;
			}
		};
		btnAdjustTiming.Click += delegate
		{
			AdjustTiming();
		};
		btnClear.Click += delegate
		{
			ClearActions();
		};
		btnRecord.MouseUp += delegate(object s, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Record();
			}
		};
		txtKeySetter.MouseUp += EventKeySetterClick;
		btnClose.Click += delegate
		{
			Close();
		};
		base.FormClosing += FrmMacroEditor_FormClosing;
		btnMin.Click += delegate
		{
			base.WindowState = FormWindowState.Minimized;
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
		btnImportScript.Click += delegate
		{
			ScriptParser.LoadScriptTo(actionTable);
		};
		btnTest.Click += delegate
		{
			if (dtActions.Rows.Count < 1)
			{
				Messenger.Show(AppSession.ActiveLanguage.info_no_action_to_key, "Error");
			}
			else if (CheckActions())
			{
				if (TestActionThread == null || !TestActionThread.IsAlive)
				{
					StartTesting();
				}
				else
				{
					CancelTesting();
				}
			}
		};
		dtActions.BindingContextChanged += delegate
		{
			dtActions.Columns[0].Visible = false;
			dtActions.Columns[1].Width = 35;
			dtActions.Columns[2].Width = 20;
			dtActions.Columns[3].Width = (dtActions.Width - 21) / 2;
			dtActions.Columns[4].Width = (dtActions.Width - 21) / 2;
		};
		btnSave.Click += delegate
		{
			SaveMacro();
		};
		AppSession.TopMostStatusChanged += delegate
		{
			base.TopMost = AppSession.TopMost;
		};
		btnMultiAction.Click += delegate
		{
			if (dtActions.SelectedRows.Count < 2)
			{
				Messenger.Show("You need to select at two allActions.", "Error");
			}
			else
			{
				frmActionEditor frmActionEditor2 = new frmActionEditor();
				if (frmActionEditor2.ShowDialog() == DialogResult.OK)
				{
					actionTable.LineNumberUpdateEnabled = false;
					int num = 1;
					int num2 = 0;
					while (num2 < dtActions.SelectedRows.Count)
					{
						int index = dtActions.SelectedRows[num2].Index;
						DataRow dataRow = actionTable.NewRow();
						IMacroAction macroAction = (IMacroAction)(dataRow[0] = (IMacroAction)frmActionEditor2.Action.Clone());
						dataRow[2] = macroAction.Icon;
						dataRow[3] = macroAction.VisibleText;
						dataRow[4] = macroAction.VisibleValue;
						actionTable.Rows.InsertAt(dataRow, index);
						num2++;
						num++;
					}
					MoveSelectedRowsUp();
					actionTable.LineNumberUpdateEnabled = true;
					dtActions.ClearSelection();
				}
			}
		};
		btnAddAction.Click += delegate
		{
			frmActionEditor frmActionEditor2 = new frmActionEditor();
			if (frmActionEditor2.ShowDialog() == DialogResult.OK)
			{
				if (dtActions.SelectedRows.Count > 0)
				{
					int index = dtActions.SelectedRows[0].Index;
					DataRow dataRow = actionTable.NewRow();
					dataRow[0] = frmActionEditor2.Action;
					dataRow[1] = 1;
					dataRow[2] = frmActionEditor2.Action.Icon;
					dataRow[3] = frmActionEditor2.Action.VisibleText;
					dataRow[4] = frmActionEditor2.Action.VisibleValue;
					actionTable.Rows.InsertAt(dataRow, index + 1);
					dtActions.ClearSelection();
					dtActions.Rows[index + 1].Selected = true;
				}
				else
				{
					actionTable.Rows.Add(frmActionEditor2.Action, 0, frmActionEditor2.Action.Icon, frmActionEditor2.Action.VisibleText, frmActionEditor2.Action.VisibleValue);
					dtActions.Rows[dtActions.Rows.Count - 1].Selected = true;
				}
				isSaved = false;
			}
			if (dtActions.SelectedRows.Count > 0)
			{
				dtActions.FirstDisplayedScrollingRowIndex = dtActions.SelectedRows[0].Index;
			}
		};
		btnDeleteAction.Click += delegate
		{
			if (dtActions.SelectedRows.Count > 0)
			{
				actionTable.LineNumberUpdateEnabled = false;
				foreach (DataGridViewRow selectedRow in dtActions.SelectedRows)
				{
					actionTable.Rows.RemoveAt(selectedRow.Index);
				}
				actionTable.LineNumberUpdateEnabled = true;
				isSaved = false;
			}
		};
		btnEditAction.Click += delegate
		{
			EditAction();
		};
		btnClone.Click += delegate
		{
			CloneSelectedAction();
			isSaved = false;
		};
		btnActionMoveUp.Click += delegate
		{
			MoveSelectedRowsUp();
			isSaved = false;
		};
		btnActionMoveDown.Click += delegate
		{
			MoveSelectedRowsDown();
			isSaved = false;
		};
		cbTriggerType.SelectedIndexChanged += delegate
		{
			PrimaryKey = CSInputs.Enums.KeyboardKeys.None;
			SecondaryKey = CSInputs.Enums.KeyboardKeys.None;
			SecondaryMouseKey = CSInputs.Enums.MouseKeys.None;
			viewKeysOnKeySetterTextBox();
			if (cbTriggerType.SelectedIndex == 0)
			{
				setKeyEventPreset();
			}
			else
			{
				setPresetColor();
			}
		};
		pnlSelectedColor.Click += delegate
		{
			colorDialog1.Color = pnlSelectedColor.BackColor;
			if (colorDialog1.ShowDialog() == DialogResult.OK)
			{
				pnlSelectedColor.BackColor = colorDialog1.Color;
			}
		};
	}

	private void FrmMacroEditor_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (!isSaved && Messenger.Show(AppSession.ActiveLanguage.someUnsavedChanges, "Warning", "CONTINE", "CANCEL") != DialogResult.Yes)
		{
			e.Cancel = true;
		}
	}

	private void ClearActions()
	{
		if (actionTable.Rows.Count >= 1 && Messenger.Show(AppSession.ActiveLanguage.info_clear_all_actions, "Warning", "OK", "CANCEL") == DialogResult.Yes)
		{
			actionTable.Clear();
			isSaved = false;
		}
	}

	private void EditAction()
	{
		if (dtActions.SelectedRows.Count > 0)
		{
			IMacroAction action = (IMacroAction)actionTable.Rows[dtActions.SelectedRows[0].Index][0];
			frmActionEditor frmActionEditor2 = new frmActionEditor(action);
			if (frmActionEditor2.ShowDialog() == DialogResult.OK)
			{
				actionTable.Rows[dtActions.SelectedRows[0].Index][0] = frmActionEditor2.Action;
				actionTable.Rows[dtActions.SelectedRows[0].Index][2] = frmActionEditor2.Action.Icon;
				actionTable.Rows[dtActions.SelectedRows[0].Index][3] = frmActionEditor2.Action.VisibleText;
				actionTable.Rows[dtActions.SelectedRows[0].Index][4] = frmActionEditor2.Action.VisibleValue;
				isSaved = false;
			}
			dtActions.FirstDisplayedScrollingRowIndex = dtActions.SelectedRows[0].Index;
			updateActionInfoText();
		}
	}

	private void CloneSelectedAction()
	{
		if (dtActions.SelectedRows.Count > 0)
		{
			int count = dtActions.SelectedRows.Count;
			for (int num = count - 1; num >= 0; num--)
			{
				IMacroAction macroAction = (IMacroAction)(actionTable.Rows[dtActions.SelectedRows[num].Index][0] as IMacroAction).Clone();
				actionTable.Rows.Add(macroAction, 0, macroAction.Icon, macroAction.VisibleText, macroAction.VisibleValue);
			}
			dtActions.ClearSelection();
			for (int i = dtActions.Rows.Count - count; i < dtActions.Rows.Count; i++)
			{
				dtActions.Rows[i].Selected = true;
			}
			dtActions.FirstDisplayedScrollingRowIndex = dtActions.SelectedRows[0].Index;
		}
	}

	private void AdjustTiming()
	{
		if (actionTable.Rows.Count < 1)
		{
			return;
		}
		frmTimeAdjuster frmTimeAdjuster2 = new frmTimeAdjuster();
		if (frmTimeAdjuster2.ShowDialog() == DialogResult.OK)
		{
			for (int i = 0; i < actionTable.Rows.Count; i++)
			{
				if (actionTable.Rows[i][0].GetType() == typeof(WaitAction))
				{
					WaitAction waitAction = (WaitAction)actionTable.Rows[i][0];
					waitAction.SleepAmount += frmTimeAdjuster2.SleepAdjustValue;
					if (waitAction.SleepAmount < 0)
					{
						waitAction.SleepAmount = 5;
					}
					actionTable.Rows[i][3] = waitAction.VisibleValue;
				}
			}
			isSaved = false;
		}
		updateActionInfoText();
	}

	private void TestActions()
	{
		IMacroAction[] acts = GetActions();
		btnTest.Invoke((MethodInvoker)delegate
		{
			progressBar1.Maximum = acts.Length;
			progressBar1.Value = 0;
			progressBar1.Visible = true;
		});
		int i;
		for (i = 0; i < acts.Length; i++)
		{
			acts[i].DoJob();
			progressBar1.Invoke((MethodInvoker)delegate
			{
				progressBar1.Value = i;
			});
		}
		btnTest.Invoke((MethodInvoker)delegate
		{
			CancelTesting();
		});
	}

	private void StartTesting()
	{
		if (IsRecording)
		{
			Record();
		}
		base.Enabled = false;
		IsTesting = true;
		TestActionThread = new Thread((ThreadStart)delegate
		{
			TestActions();
		});
		TestActionThread.Start();
		btnTest.Text = AppSession.ActiveLanguage.btnTest_on;
	}

	private void CancelTesting()
	{
		base.Enabled = true;
		IsTesting = false;
		TestActionThread.Abort();
		btnTest.Text = AppSession.ActiveLanguage.btnTest;
		progressBar1.Value = progressBar1.Maximum;
		progressBar1.Visible = false;
	}

	private void MoveSelectedRowsDown()
	{
		if (dtActions.SelectedRows.Count <= 0)
		{
			return;
		}
		actionTable.LineNumberUpdateEnabled = false;
		int num = 0;
		int num2 = 0;
		List<DataRow> list = new List<DataRow>();
		for (int i = 0; i < dtActions.SelectedRows.Count; i++)
		{
			if (num2 < dtActions.SelectedRows[i].Index)
			{
				num2 = dtActions.SelectedRows[i].Index;
			}
		}
		if (num2 >= dtActions.Rows.Count - 1)
		{
			return;
		}
		for (int j = 0; j < dtActions.SelectedRows.Count; j++)
		{
			DataRow row = ((DataRowView)dtActions.SelectedRows[j].DataBoundItem).Row;
			list.Add(row);
		}
		foreach (DataRow item in list.OrderByDescending((DataRow t) => actionTable.Rows.IndexOf(t)))
		{
			int num3 = actionTable.MoveRow(item, ActionTable.TableRowDirection.Down);
			if (num < num3)
			{
				num = num3;
			}
		}
		dtActions.ClearSelection();
		for (int num4 = 0; num4 < list.Count; num4++)
		{
			dtActions.Rows[num - num4].Selected = true;
		}
		dtActions.FirstDisplayedScrollingRowIndex = dtActions.SelectedRows[0].Index;
		actionTable.LineNumberUpdateEnabled = true;
	}

	private void MoveSelectedRowsUp()
	{
		if (dtActions.SelectedRows.Count <= 0)
		{
			return;
		}
		actionTable.LineNumberUpdateEnabled = false;
		int num = 0;
		int num2 = int.MaxValue;
		List<DataRow> list = new List<DataRow>();
		for (int i = 0; i < dtActions.SelectedRows.Count; i++)
		{
			if (num2 > dtActions.SelectedRows[i].Index)
			{
				num2 = dtActions.SelectedRows[i].Index;
			}
		}
		if (num2 < 1)
		{
			return;
		}
		for (int j = 0; j < dtActions.SelectedRows.Count; j++)
		{
			DataRow row = ((DataRowView)dtActions.SelectedRows[j].DataBoundItem).Row;
			list.Add(row);
		}
		foreach (DataRow item in list.OrderBy((DataRow t) => actionTable.Rows.IndexOf(t)))
		{
			int num3 = actionTable.MoveRow(item, ActionTable.TableRowDirection.Up);
			if (num < num3)
			{
				num = num3;
			}
		}
		dtActions.ClearSelection();
		for (int num4 = 0; num4 < list.Count; num4++)
		{
			dtActions.Rows[num - num4].Selected = true;
		}
		dtActions.FirstDisplayedScrollingRowIndex = dtActions.SelectedRows[0].Index;
		actionTable.LineNumberUpdateEnabled = true;
	}

	private void EventKeySetterClick(object sender, EventArgs e)
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

	private bool CheckActions()
	{
		List<IMacroAction> allActions = GetActions().ToList();
		List<int> list = new List<int>();
		Dictionary<int, string> dictionary = new Dictionary<int, string>();
		for (int i = 0; i < allActions.Count; i++)
		{
			if (list.Contains(i))
			{
				continue;
			}
			IMacroAction macroAction = allActions[i];
			if (!(macroAction is KeyboardKeyAction { KeyMethod: not KeyPressMethods.Press }) && !(macroAction is MouseKeyAction { KeyMethod: not KeyPressMethods.Press }))
			{
				list.Add(i);
				continue;
			}
			if (macroAction is KeyboardKeyAction keyboardKeyAction2)
			{
				if (keyboardKeyAction2.KeyMethod == KeyPressMethods.Up)
				{
					dictionary.Add(i, $"Action - [{i + 1}] : You added an [Up] action for the '{keyboardKeyAction2.KeyboardKey}' key, but the [Up] action must come after the [Down] action.");
					list.Add(i);
					continue;
				}
				int num = findNextActionKey(ref allActions, keyboardKeyAction2, i);
				if (num <= -1)
				{
					dictionary.Add(i, $"Action - [ {i + 1}] : You added a [Down] action for the '{keyboardKeyAction2.KeyboardKey}' key, but did not add an [Up] action afterward.");
					continue;
				}
				list.Add(num);
			}
			if (!(macroAction is MouseKeyAction mouseKeyAction2))
			{
				continue;
			}
			if (mouseKeyAction2.KeyMethod == KeyPressMethods.Up)
			{
				dictionary.Add(i, $"Action - [{i + 1}] : You added an [Up] action for the '{mouseKeyAction2.MouseKey}' key, but the [Up] action must come after the [Down] action.");
				list.Add(i);
				continue;
			}
			int num2 = findNextActionKey(ref allActions, mouseKeyAction2, i);
			if (num2 > -1)
			{
				list.Add(num2);
			}
			else
			{
				dictionary.Add(i, $"Action - [ {i + 1}] : You added a [Down] action for the '{mouseKeyAction2.MouseKey}' key, but did not add an [Up] action afterward.");
			}
		}
		if (dictionary.Count < 1)
		{
			return true;
		}
		switch (Messenger.Show(dictionary.Count + " Errors found in your macro.", "Warning", "Try Fix", "Show Errors", "Cancel"))
		{
		case DialogResult.Yes:
			foreach (KeyValuePair<int, string> item in dictionary)
			{
				IMacroAction macroAction2 = allActions[item.Key];
				if (macroAction2 == null)
				{
					continue;
				}
				if (macroAction2 is KeyboardKeyAction keyboardKeyAction3)
				{
					int num3 = actionTable.FindIndexOf(macroAction2);
					if (num3 < 0)
					{
						continue;
					}
					dtActions.Rows[num3].DefaultCellStyle.BackColor = AppSession.Settings.ColorPalette.MidColor;
					dtActions.Rows[num3].DefaultCellStyle.SelectionBackColor = AppSession.Settings.ColorPalette.DarkColor;
					DataRow dataRow = actionTable.NewRow();
					KeyboardKeyAction keyboardKeyAction4 = (KeyboardKeyAction)keyboardKeyAction3.Clone();
					keyboardKeyAction4.KeyMethod = ((keyboardKeyAction3.KeyMethod == KeyPressMethods.Up) ? KeyPressMethods.Down : KeyPressMethods.Up);
					dataRow[0] = keyboardKeyAction4;
					dataRow[1] = 1;
					dataRow[2] = keyboardKeyAction4.Icon;
					dataRow[3] = keyboardKeyAction4.VisibleText;
					dataRow[4] = keyboardKeyAction4.VisibleValue;
					int pos = ((keyboardKeyAction3.KeyMethod == KeyPressMethods.Up) ? num3 : (num3 + 1));
					actionTable.Rows.InsertAt(dataRow, pos);
				}
				if (macroAction2 is MouseKeyAction mouseKeyAction3)
				{
					int num4 = actionTable.FindIndexOf(macroAction2);
					if (num4 >= 0)
					{
						dtActions.Rows[num4].DefaultCellStyle.BackColor = AppSession.Settings.ColorPalette.MidColor;
						dtActions.Rows[num4].DefaultCellStyle.SelectionBackColor = AppSession.Settings.ColorPalette.DarkColor;
						DataRow dataRow2 = actionTable.NewRow();
						MouseKeyAction mouseKeyAction4 = (MouseKeyAction)mouseKeyAction3.Clone();
						mouseKeyAction4.KeyMethod = ((mouseKeyAction3.KeyMethod == KeyPressMethods.Up) ? KeyPressMethods.Down : KeyPressMethods.Up);
						dataRow2[0] = mouseKeyAction4;
						dataRow2[1] = 1;
						dataRow2[2] = mouseKeyAction4.Icon;
						dataRow2[3] = mouseKeyAction4.VisibleText;
						dataRow2[4] = mouseKeyAction4.VisibleValue;
						int pos2 = ((mouseKeyAction3.KeyMethod == KeyPressMethods.Up) ? num4 : (num4 + 1));
						actionTable.Rows.InsertAt(dataRow2, pos2);
					}
				}
			}
			return false;
		case DialogResult.Cancel:
			return false;
		default:
		{
			for (int j = 0; j < allActions.Count; j++)
			{
				dtActions.Rows[j].DefaultCellStyle.BackColor = AppSession.Settings.ColorPalette.MidColor;
				dtActions.Rows[j].DefaultCellStyle.SelectionBackColor = AppSession.Settings.ColorPalette.DarkColor;
				if (dictionary.ContainsKey(j))
				{
					dtActions.Rows[j].DefaultCellStyle.BackColor = Color.DarkRed;
					dtActions.Rows[j].DefaultCellStyle.SelectionBackColor = Color.Firebrick;
					dictionary.TryGetValue(j, out var value);
					Messenger.Show(value, "Action Error");
				}
			}
			return false;
		}
		}
	}

	private int findNextActionKey(ref List<IMacroAction> allActions, IMacroAction passedAction, int firstKeyIndex)
	{
		for (int i = firstKeyIndex + 1; i < allActions.Count; i++)
		{
			IMacroAction macroAction = allActions[i];
			if (macroAction.GetType() == passedAction.GetType())
			{
				if (passedAction is KeyboardKeyAction keyboardKeyAction && macroAction is KeyboardKeyAction keyboardKeyAction2 && keyboardKeyAction.KeyboardKey == keyboardKeyAction2.KeyboardKey && keyboardKeyAction2.KeyMethod == KeyPressMethods.Up)
				{
					return i;
				}
				if (passedAction is MouseKeyAction mouseKeyAction && macroAction is MouseKeyAction mouseKeyAction2 && mouseKeyAction.MouseKey == mouseKeyAction2.MouseKey && mouseKeyAction2.KeyMethod == KeyPressMethods.Up)
				{
					return i;
				}
			}
		}
		return -1;
	}

	private void SaveMacro()
	{
		if (!CheckActions())
		{
			return;
		}
		if (txtMacroName.Text.Length < 2)
		{
			Messenger.Show(AppSession.ActiveLanguage.info_enter_macro_name, "Error");
			return;
		}
		if (dtActions.Rows.Count < 1)
		{
			Messenger.Show(AppSession.ActiveLanguage.info_no_action_to_save, "Error");
			return;
		}
		if (Macro.Trigger != null)
		{
			Macro.Trigger.Stop();
		}
		Macro.Name = txtMacroName.Text;
		switch (cbTriggerType.SelectedIndex)
		{
		case 0:
			if (SecondaryKey == CSInputs.Enums.KeyboardKeys.None && SecondaryMouseKey == CSInputs.Enums.MouseKeys.None)
			{
				Messenger.Show(AppSession.ActiveLanguage.info_no_key_selected, "Error");
				return;
			}
			if (SecondaryMouseKey != CSInputs.Enums.MouseKeys.None)
			{
				Macro.Trigger = new MouseTrigger();
				(Macro.Trigger as MouseTrigger).Method = (TriggerKeyMethods)cbTriggerMethod.SelectedIndex;
				(Macro.Trigger as MouseTrigger).PrimaryKey = PrimaryKey;
				(Macro.Trigger as MouseTrigger).SecondaryKey = SecondaryMouseKey;
			}
			if (SecondaryKey != CSInputs.Enums.KeyboardKeys.None)
			{
				Macro.Trigger = new KeyboardTrigger();
				(Macro.Trigger as KeyboardTrigger).Method = (TriggerKeyMethods)cbTriggerMethod.SelectedIndex;
				(Macro.Trigger as KeyboardTrigger).PrimaryKey = PrimaryKey;
				(Macro.Trigger as KeyboardTrigger).SecondaryKey = SecondaryKey;
			}
			break;
		case 1:
			Macro.Trigger = new ColorTrigger();
			(Macro.Trigger as ColorTrigger).Method = (ColorTriggerMethods)cbTriggerMethod.SelectedIndex;
			(Macro.Trigger as ColorTrigger).Pos = new Point((int)mPosX.Value, (int)mPosY.Value);
			(Macro.Trigger as ColorTrigger).Color = pnlSelectedColor.BackColor;
			(Macro.Trigger as ColorTrigger).TriggerOnce = cbTriggerOnce.Checked;
			(Macro.Trigger as ColorTrigger).ColorTolerance = (int)txtColorTolerance.Value;
			break;
		}
		for (int i = 0; i < actionTable.Rows.Count; i++)
		{
			Macro.Trigger.Actions.Add((IMacroAction)actionTable.Rows[i][0]);
		}
		isSaved = true;
		base.DialogResult = DialogResult.OK;
	}

	private void saveRecordSettings()
	{
		AppSession.Settings.RecordDelays = cbRecordDelays.Checked;
		AppSession.Settings.RecordKeyboardKeys = cbRecordKeyboardKeys.Checked;
		AppSession.Settings.RecordMouseKeys = cbRecordMouseKeys.Checked;
		AppSession.Settings.RecordMouseMovements = cbRecordMouseMovements.Checked;
		SharedMethods.SaveSettings();
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TGMacro.frmMacroEditor));
		this.grpTrigger = new System.Windows.Forms.GroupBox();
		this.btnHelpTriggers = new System.Windows.Forms.PictureBox();
		this.pnl_colors = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.cbTriggerOnce = new System.Windows.Forms.CheckBox();
		this.txtColorTolerance = new System.Windows.Forms.NumericUpDown();
		this.lblGrabInfo2 = new System.Windows.Forms.Label();
		this.mPosY = new System.Windows.Forms.NumericUpDown();
		this.mPosX = new System.Windows.Forms.NumericUpDown();
		this.pnlSelectedColor = new System.Windows.Forms.Panel();
		this.lblColor = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.lblColorPos = new System.Windows.Forms.Label();
		this.lblGrabInfo = new System.Windows.Forms.Label();
		this.pnl_keys = new System.Windows.Forms.Panel();
		this.txtKeySetter = new System.Windows.Forms.TextBox();
		this.lblTrgKeys = new System.Windows.Forms.Label();
		this.lblTrgMethod = new System.Windows.Forms.Label();
		this.cbTriggerMethod = new TGMacro.UI.CustomControls.FlattenComboBox();
		this.lblTrgType = new System.Windows.Forms.Label();
		this.cbTriggerType = new TGMacro.UI.CustomControls.FlattenComboBox();
		this.grpActions = new System.Windows.Forms.GroupBox();
		this.btnMultiAction = new TGMacro.UI.CustomControls.HoverButton();
		this.btnAdjustTiming = new TGMacro.UI.CustomControls.HoverButton();
		this.btnClear = new TGMacro.UI.CustomControls.HoverButton();
		this.btnImportScript = new TGMacro.UI.CustomControls.HoverButton();
		this.btnClone = new TGMacro.UI.CustomControls.HoverButton();
		this.dtActions = new TGMacro.UI.CustomControls.DatagridDoubleBuffer();
		this.lblActionInfo = new System.Windows.Forms.Label();
		this.btnDeleteAction = new TGMacro.UI.CustomControls.HoverButton();
		this.btnEditAction = new TGMacro.UI.CustomControls.HoverButton();
		this.btnActionMoveUp = new TGMacro.UI.CustomControls.HoverButton();
		this.btnAddAction = new TGMacro.UI.CustomControls.HoverButton();
		this.btnActionMoveDown = new TGMacro.UI.CustomControls.HoverButton();
		this.txtMacroName = new System.Windows.Forms.TextBox();
		this.colorDialog1 = new System.Windows.Forms.ColorDialog();
		this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
		this.grpName = new System.Windows.Forms.GroupBox();
		this.pnlMain = new System.Windows.Forms.Panel();
		this.progressBar1 = new System.Windows.Forms.ProgressBar();
		this.pnlTop = new System.Windows.Forms.Panel();
		this.btnHelpMacroEditor = new System.Windows.Forms.PictureBox();
		this.btnMin = new TGMacro.UI.CustomControls.HoverPictureBox();
		this.btnClose = new TGMacro.UI.CustomControls.HoverPictureBox();
		this.lblTitle = new System.Windows.Forms.Label();
		this.grpRecord = new System.Windows.Forms.GroupBox();
		this.btnRecord = new TGMacro.UI.CustomControls.HoverButton();
		this.cbRecordDelays = new System.Windows.Forms.CheckBox();
		this.cbRecordKeyboardKeys = new System.Windows.Forms.CheckBox();
		this.cbRecordMouseKeys = new System.Windows.Forms.CheckBox();
		this.cbRecordMouseMovements = new System.Windows.Forms.CheckBox();
		this.lblRecordThis = new System.Windows.Forms.Label();
		this.lblMouseCapPos = new System.Windows.Forms.Label();
		this.cbMousePositioning = new TGMacro.UI.CustomControls.FlattenComboBox();
		this.btnTest = new TGMacro.UI.CustomControls.HoverButton();
		this.btnSave = new TGMacro.UI.CustomControls.HoverButton();
		this.lblLoading = new System.Windows.Forms.Label();
		this.grpTrigger.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnHelpTriggers).BeginInit();
		this.pnl_colors.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.txtColorTolerance).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.mPosY).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.mPosX).BeginInit();
		this.pnl_keys.SuspendLayout();
		this.grpActions.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dtActions).BeginInit();
		this.grpName.SuspendLayout();
		this.pnlMain.SuspendLayout();
		this.pnlTop.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnHelpMacroEditor).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.btnMin).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.btnClose).BeginInit();
		this.grpRecord.SuspendLayout();
		base.SuspendLayout();
		this.grpTrigger.Controls.Add(this.btnHelpTriggers);
		this.grpTrigger.Controls.Add(this.pnl_colors);
		this.grpTrigger.Controls.Add(this.pnl_keys);
		this.grpTrigger.Controls.Add(this.lblTrgMethod);
		this.grpTrigger.Controls.Add(this.cbTriggerMethod);
		this.grpTrigger.Controls.Add(this.lblTrgType);
		this.grpTrigger.Controls.Add(this.cbTriggerType);
		this.grpTrigger.Location = new System.Drawing.Point(491, 45);
		this.grpTrigger.Name = "grpTrigger";
		this.grpTrigger.Size = new System.Drawing.Size(354, 363);
		this.grpTrigger.TabIndex = 0;
		this.grpTrigger.TabStop = false;
		this.grpTrigger.Text = "Triggering";
		this.btnHelpTriggers.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnHelpTriggers.Image = TGMacro.Properties.Resources.help_20;
		this.btnHelpTriggers.Location = new System.Drawing.Point(312, 28);
		this.btnHelpTriggers.Name = "btnHelpTriggers";
		this.btnHelpTriggers.Size = new System.Drawing.Size(20, 20);
		this.btnHelpTriggers.TabIndex = 13;
		this.btnHelpTriggers.TabStop = false;
		this.pnl_colors.Controls.Add(this.label5);
		this.pnl_colors.Controls.Add(this.label4);
		this.pnl_colors.Controls.Add(this.label2);
		this.pnl_colors.Controls.Add(this.cbTriggerOnce);
		this.pnl_colors.Controls.Add(this.txtColorTolerance);
		this.pnl_colors.Controls.Add(this.lblGrabInfo2);
		this.pnl_colors.Controls.Add(this.mPosY);
		this.pnl_colors.Controls.Add(this.mPosX);
		this.pnl_colors.Controls.Add(this.pnlSelectedColor);
		this.pnl_colors.Controls.Add(this.lblColor);
		this.pnl_colors.Controls.Add(this.label3);
		this.pnl_colors.Controls.Add(this.label1);
		this.pnl_colors.Controls.Add(this.lblColorPos);
		this.pnl_colors.Controls.Add(this.lblGrabInfo);
		this.pnl_colors.ForeColor = System.Drawing.SystemColors.Window;
		this.pnl_colors.Location = new System.Drawing.Point(17, 139);
		this.pnl_colors.Name = "pnl_colors";
		this.pnl_colors.Size = new System.Drawing.Size(318, 218);
		this.pnl_colors.TabIndex = 10;
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.Transparent;
		this.label5.Location = new System.Drawing.Point(102, 31);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(51, 17);
		this.label5.TabIndex = 24;
		this.label5.Text = "(1 ~ 10)";
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.Transparent;
		this.label4.Location = new System.Drawing.Point(32, 31);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(17, 17);
		this.label4.TabIndex = 23;
		this.label4.Text = "%";
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Transparent;
		this.label2.Location = new System.Drawing.Point(4, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(108, 17);
		this.label2.TabIndex = 22;
		this.label2.Text = "Color tolerance :";
		this.cbTriggerOnce.AutoSize = true;
		this.cbTriggerOnce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cbTriggerOnce.Location = new System.Drawing.Point(194, 29);
		this.cbTriggerOnce.Name = "cbTriggerOnce";
		this.cbTriggerOnce.Size = new System.Drawing.Size(101, 21);
		this.cbTriggerOnce.TabIndex = 21;
		this.cbTriggerOnce.Text = "Trigger once ";
		this.cbTriggerOnce.UseVisualStyleBackColor = true;
		this.txtColorTolerance.Location = new System.Drawing.Point(51, 28);
		this.txtColorTolerance.Maximum = new decimal(new int[4] { 10, 0, 0, 0 });
		this.txtColorTolerance.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		this.txtColorTolerance.Name = "txtColorTolerance";
		this.txtColorTolerance.Size = new System.Drawing.Size(45, 22);
		this.txtColorTolerance.TabIndex = 20;
		this.txtColorTolerance.Value = new decimal(new int[4] { 5, 0, 0, 0 });
		this.lblGrabInfo2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblGrabInfo2.Font = new System.Drawing.Font("Century Gothic", 9f, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 162);
		this.lblGrabInfo2.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
		this.lblGrabInfo2.Location = new System.Drawing.Point(16, 172);
		this.lblGrabInfo2.Name = "lblGrabInfo2";
		this.lblGrabInfo2.Size = new System.Drawing.Size(302, 40);
		this.lblGrabInfo2.TabIndex = 19;
		this.lblGrabInfo2.Text = "•Alt + X to get current cursor position\r\n•Alt + C to get screen color by cursor";
		this.mPosY.Location = new System.Drawing.Point(194, 79);
		this.mPosY.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.mPosY.Minimum = new decimal(new int[4] { 10000, 0, 0, -2147483648 });
		this.mPosY.Name = "mPosY";
		this.mPosY.Size = new System.Drawing.Size(104, 22);
		this.mPosY.TabIndex = 17;
		this.mPosX.Location = new System.Drawing.Point(51, 79);
		this.mPosX.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.mPosX.Minimum = new decimal(new int[4] { 10000, 0, 0, -2147483648 });
		this.mPosX.Name = "mPosX";
		this.mPosX.Size = new System.Drawing.Size(104, 22);
		this.mPosX.TabIndex = 16;
		this.pnlSelectedColor.BackColor = System.Drawing.Color.Red;
		this.pnlSelectedColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnlSelectedColor.Location = new System.Drawing.Point(20, 134);
		this.pnlSelectedColor.Name = "pnlSelectedColor";
		this.pnlSelectedColor.Size = new System.Drawing.Size(30, 30);
		this.pnlSelectedColor.TabIndex = 14;
		this.lblColor.AutoSize = true;
		this.lblColor.Location = new System.Drawing.Point(3, 105);
		this.lblColor.Name = "lblColor";
		this.lblColor.Size = new System.Drawing.Size(46, 17);
		this.lblColor.TabIndex = 13;
		this.lblColor.Text = "Color :";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(168, 82);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(18, 17);
		this.label3.TabIndex = 12;
		this.label3.Text = "Y:";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(28, 81);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(21, 17);
		this.label1.TabIndex = 11;
		this.label1.Text = "X: ";
		this.lblColorPos.AutoSize = true;
		this.lblColorPos.BackColor = System.Drawing.Color.Transparent;
		this.lblColorPos.Location = new System.Drawing.Point(4, 53);
		this.lblColorPos.Name = "lblColorPos";
		this.lblColorPos.Size = new System.Drawing.Size(96, 17);
		this.lblColorPos.TabIndex = 8;
		this.lblColorPos.Text = "Color position :";
		this.lblGrabInfo.Location = new System.Drawing.Point(56, 125);
		this.lblGrabInfo.Name = "lblGrabInfo";
		this.lblGrabInfo.Size = new System.Drawing.Size(259, 47);
		this.lblGrabInfo.TabIndex = 18;
		this.lblGrabInfo.Text = "You can set the color manually by clicking the colored box.";
		this.pnl_keys.Controls.Add(this.txtKeySetter);
		this.pnl_keys.Controls.Add(this.lblTrgKeys);
		this.pnl_keys.Location = new System.Drawing.Point(17, 139);
		this.pnl_keys.Name = "pnl_keys";
		this.pnl_keys.Size = new System.Drawing.Size(318, 76);
		this.pnl_keys.TabIndex = 9;
		this.txtKeySetter.Location = new System.Drawing.Point(23, 25);
		this.txtKeySetter.Multiline = true;
		this.txtKeySetter.Name = "txtKeySetter";
		this.txtKeySetter.ReadOnly = true;
		this.txtKeySetter.Size = new System.Drawing.Size(278, 25);
		this.txtKeySetter.TabIndex = 10;
		this.txtKeySetter.Text = "Click here to set key";
		this.txtKeySetter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.lblTrgKeys.AutoSize = true;
		this.lblTrgKeys.Location = new System.Drawing.Point(0, 0);
		this.lblTrgKeys.Name = "lblTrgKeys";
		this.lblTrgKeys.Size = new System.Drawing.Size(40, 17);
		this.lblTrgKeys.TabIndex = 7;
		this.lblTrgKeys.Text = "Keys :";
		this.lblTrgMethod.AutoSize = true;
		this.lblTrgMethod.Location = new System.Drawing.Point(17, 82);
		this.lblTrgMethod.Name = "lblTrgMethod";
		this.lblTrgMethod.Size = new System.Drawing.Size(49, 17);
		this.lblTrgMethod.TabIndex = 5;
		this.lblTrgMethod.Text = "When :";
		this.cbTriggerMethod.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
		this.cbTriggerMethod.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.cbTriggerMethod.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
		this.cbTriggerMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbTriggerMethod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cbTriggerMethod.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.cbTriggerMethod.FormattingEnabled = true;
		this.cbTriggerMethod.Location = new System.Drawing.Point(17, 104);
		this.cbTriggerMethod.Name = "cbTriggerMethod";
		this.cbTriggerMethod.Size = new System.Drawing.Size(318, 23);
		this.cbTriggerMethod.TabIndex = 4;
		this.lblTrgType.AutoSize = true;
		this.lblTrgType.Location = new System.Drawing.Point(17, 31);
		this.lblTrgType.Name = "lblTrgType";
		this.lblTrgType.Size = new System.Drawing.Size(118, 17);
		this.lblTrgType.TabIndex = 3;
		this.lblTrgType.Text = "Trigger actions by :";
		this.cbTriggerType.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
		this.cbTriggerType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.cbTriggerType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
		this.cbTriggerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbTriggerType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cbTriggerType.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.cbTriggerType.FormattingEnabled = true;
		this.cbTriggerType.Location = new System.Drawing.Point(17, 53);
		this.cbTriggerType.Name = "cbTriggerType";
		this.cbTriggerType.Size = new System.Drawing.Size(318, 23);
		this.cbTriggerType.TabIndex = 2;
		this.grpActions.Controls.Add(this.btnMultiAction);
		this.grpActions.Controls.Add(this.btnAdjustTiming);
		this.grpActions.Controls.Add(this.btnClear);
		this.grpActions.Controls.Add(this.btnImportScript);
		this.grpActions.Controls.Add(this.btnClone);
		this.grpActions.Controls.Add(this.dtActions);
		this.grpActions.Controls.Add(this.lblActionInfo);
		this.grpActions.Controls.Add(this.btnDeleteAction);
		this.grpActions.Controls.Add(this.btnEditAction);
		this.grpActions.Controls.Add(this.btnActionMoveUp);
		this.grpActions.Controls.Add(this.btnAddAction);
		this.grpActions.Controls.Add(this.btnActionMoveDown);
		this.grpActions.Location = new System.Drawing.Point(13, 127);
		this.grpActions.MinimumSize = new System.Drawing.Size(323, 0);
		this.grpActions.Name = "grpActions";
		this.grpActions.Size = new System.Drawing.Size(462, 535);
		this.grpActions.TabIndex = 2;
		this.grpActions.TabStop = false;
		this.grpActions.Text = "Actions";
		this.btnMultiAction.BackgroundImage = TGMacro.Properties.Resources.multi_16;
		this.btnMultiAction.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnMultiAction.FlatAppearance.BorderSize = 0;
		this.btnMultiAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnMultiAction.HoverColor = null;
		this.btnMultiAction.Location = new System.Drawing.Point(431, 170);
		this.btnMultiAction.Name = "btnMultiAction";
		this.btnMultiAction.Size = new System.Drawing.Size(25, 25);
		this.btnMultiAction.TabIndex = 16;
		this.btnMultiAction.UseVisualStyleBackColor = true;
		this.btnAdjustTiming.BackgroundImage = TGMacro.Properties.Resources.wait_16;
		this.btnAdjustTiming.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnAdjustTiming.FlatAppearance.BorderSize = 0;
		this.btnAdjustTiming.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdjustTiming.HoverColor = null;
		this.btnAdjustTiming.Location = new System.Drawing.Point(431, 139);
		this.btnAdjustTiming.Name = "btnAdjustTiming";
		this.btnAdjustTiming.Size = new System.Drawing.Size(25, 25);
		this.btnAdjustTiming.TabIndex = 15;
		this.btnAdjustTiming.UseVisualStyleBackColor = true;
		this.btnClear.BackgroundImage = TGMacro.Properties.Resources.clear_16;
		this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnClear.FlatAppearance.BorderSize = 0;
		this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnClear.HoverColor = null;
		this.btnClear.Location = new System.Drawing.Point(431, 199);
		this.btnClear.Name = "btnClear";
		this.btnClear.Size = new System.Drawing.Size(25, 25);
		this.btnClear.TabIndex = 14;
		this.btnClear.UseVisualStyleBackColor = true;
		this.btnImportScript.BackgroundImage = TGMacro.Properties.Resources.import_16;
		this.btnImportScript.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnImportScript.FlatAppearance.BorderSize = 0;
		this.btnImportScript.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnImportScript.HoverColor = null;
		this.btnImportScript.Location = new System.Drawing.Point(431, 504);
		this.btnImportScript.Name = "btnImportScript";
		this.btnImportScript.Size = new System.Drawing.Size(25, 25);
		this.btnImportScript.TabIndex = 12;
		this.btnImportScript.UseVisualStyleBackColor = true;
		this.btnClone.BackgroundImage = TGMacro.Properties.Resources.clone_16;
		this.btnClone.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnClone.FlatAppearance.BorderSize = 0;
		this.btnClone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnClone.HoverColor = null;
		this.btnClone.Location = new System.Drawing.Point(431, 77);
		this.btnClone.Name = "btnClone";
		this.btnClone.Size = new System.Drawing.Size(25, 25);
		this.btnClone.TabIndex = 11;
		this.btnClone.UseVisualStyleBackColor = true;
		this.dtActions.AllowUserToAddRows = false;
		this.dtActions.AllowUserToDeleteRows = false;
		this.dtActions.AllowUserToResizeColumns = false;
		this.dtActions.AllowUserToResizeRows = false;
		this.dtActions.BackgroundColor = System.Drawing.SystemColors.Control;
		this.dtActions.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
		this.dtActions.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
		this.dtActions.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.ControlDark;
		dataGridViewCellStyle.Font = new System.Drawing.Font("Century Gothic", 9f);
		dataGridViewCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.ControlDark;
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dtActions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
		this.dtActions.ColumnHeadersHeight = 30;
		this.dtActions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("Century Gothic", 9f);
		dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Transparent;
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(210, 210, 210);
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		this.dtActions.DefaultCellStyle = dataGridViewCellStyle2;
		this.dtActions.Dock = System.Windows.Forms.DockStyle.Left;
		this.dtActions.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
		this.dtActions.EnableHeadersVisualStyles = false;
		this.dtActions.GridColor = System.Drawing.SystemColors.Window;
		this.dtActions.Location = new System.Drawing.Point(3, 18);
		this.dtActions.Name = "dtActions";
		this.dtActions.ReadOnly = true;
		this.dtActions.RowHeadersVisible = false;
		this.dtActions.RowHeadersWidth = 51;
		this.dtActions.RowTemplate.Height = 29;
		this.dtActions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		this.dtActions.Size = new System.Drawing.Size(422, 491);
		this.dtActions.TabIndex = 8;
		this.dtActions.TabStop = false;
		this.lblActionInfo.AutoSize = true;
		this.lblActionInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.lblActionInfo.Location = new System.Drawing.Point(3, 509);
		this.lblActionInfo.Name = "lblActionInfo";
		this.lblActionInfo.Padding = new System.Windows.Forms.Padding(3);
		this.lblActionInfo.Size = new System.Drawing.Size(235, 23);
		this.lblActionInfo.TabIndex = 17;
		this.lblActionInfo.Text = "Actions : 27             Total Wait : 2600ms";
		this.btnDeleteAction.BackgroundImage = TGMacro.Properties.Resources.del_16;
		this.btnDeleteAction.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnDeleteAction.FlatAppearance.BorderSize = 0;
		this.btnDeleteAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDeleteAction.HoverColor = null;
		this.btnDeleteAction.Location = new System.Drawing.Point(431, 108);
		this.btnDeleteAction.Name = "btnDeleteAction";
		this.btnDeleteAction.Size = new System.Drawing.Size(25, 25);
		this.btnDeleteAction.TabIndex = 10;
		this.btnDeleteAction.UseVisualStyleBackColor = true;
		this.btnEditAction.BackgroundImage = TGMacro.Properties.Resources.edit_16;
		this.btnEditAction.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnEditAction.FlatAppearance.BorderSize = 0;
		this.btnEditAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnEditAction.HoverColor = null;
		this.btnEditAction.Location = new System.Drawing.Point(431, 48);
		this.btnEditAction.Name = "btnEditAction";
		this.btnEditAction.Size = new System.Drawing.Size(25, 25);
		this.btnEditAction.TabIndex = 9;
		this.btnEditAction.UseVisualStyleBackColor = true;
		this.btnActionMoveUp.BackgroundImage = TGMacro.Properties.Resources.up_arrow_16;
		this.btnActionMoveUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btnActionMoveUp.FlatAppearance.BorderSize = 0;
		this.btnActionMoveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnActionMoveUp.HoverColor = null;
		this.btnActionMoveUp.Location = new System.Drawing.Point(431, 273);
		this.btnActionMoveUp.Name = "btnActionMoveUp";
		this.btnActionMoveUp.Size = new System.Drawing.Size(20, 23);
		this.btnActionMoveUp.TabIndex = 3;
		this.btnActionMoveUp.UseVisualStyleBackColor = true;
		this.btnAddAction.BackgroundImage = TGMacro.Properties.Resources.add_16;
		this.btnAddAction.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnAddAction.FlatAppearance.BorderSize = 0;
		this.btnAddAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddAction.HoverColor = null;
		this.btnAddAction.Location = new System.Drawing.Point(431, 19);
		this.btnAddAction.Name = "btnAddAction";
		this.btnAddAction.Size = new System.Drawing.Size(25, 25);
		this.btnAddAction.TabIndex = 8;
		this.btnAddAction.UseVisualStyleBackColor = true;
		this.btnActionMoveDown.BackgroundImage = TGMacro.Properties.Resources.down_arrow_16;
		this.btnActionMoveDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btnActionMoveDown.FlatAppearance.BorderSize = 0;
		this.btnActionMoveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnActionMoveDown.HoverColor = null;
		this.btnActionMoveDown.Location = new System.Drawing.Point(431, 302);
		this.btnActionMoveDown.Name = "btnActionMoveDown";
		this.btnActionMoveDown.Size = new System.Drawing.Size(20, 20);
		this.btnActionMoveDown.TabIndex = 4;
		this.btnActionMoveDown.UseVisualStyleBackColor = true;
		this.txtMacroName.Location = new System.Drawing.Point(17, 31);
		this.txtMacroName.Name = "txtMacroName";
		this.txtMacroName.Size = new System.Drawing.Size(169, 22);
		this.txtMacroName.TabIndex = 0;
		this.txtMacroName.Text = "Macro 01";
		this.colorDialog1.FullOpen = true;
		this.grpName.Controls.Add(this.txtMacroName);
		this.grpName.Location = new System.Drawing.Point(16, 42);
		this.grpName.Name = "grpName";
		this.grpName.Size = new System.Drawing.Size(459, 79);
		this.grpName.TabIndex = 8;
		this.grpName.TabStop = false;
		this.grpName.Text = "Macro name";
		this.pnlMain.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
		this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnlMain.Controls.Add(this.progressBar1);
		this.pnlMain.Controls.Add(this.pnlTop);
		this.pnlMain.Controls.Add(this.grpRecord);
		this.pnlMain.Controls.Add(this.btnTest);
		this.pnlMain.Controls.Add(this.grpName);
		this.pnlMain.Controls.Add(this.grpActions);
		this.pnlMain.Controls.Add(this.grpTrigger);
		this.pnlMain.Controls.Add(this.btnSave);
		this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnlMain.ForeColor = System.Drawing.Color.Transparent;
		this.pnlMain.Location = new System.Drawing.Point(0, 0);
		this.pnlMain.Name = "pnlMain";
		this.pnlMain.Size = new System.Drawing.Size(862, 674);
		this.pnlMain.TabIndex = 11;
		this.progressBar1.Location = new System.Drawing.Point(515, 653);
		this.progressBar1.Name = "progressBar1";
		this.progressBar1.Size = new System.Drawing.Size(135, 5);
		this.progressBar1.TabIndex = 14;
		this.progressBar1.Visible = false;
		this.pnlTop.BackColor = System.Drawing.SystemColors.ControlLight;
		this.pnlTop.Controls.Add(this.btnHelpMacroEditor);
		this.pnlTop.Controls.Add(this.btnMin);
		this.pnlTop.Controls.Add(this.btnClose);
		this.pnlTop.Controls.Add(this.lblTitle);
		this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnlTop.Font = new System.Drawing.Font("Century Gothic", 9f);
		this.pnlTop.Location = new System.Drawing.Point(0, 0);
		this.pnlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.pnlTop.MaximumSize = new System.Drawing.Size(0, 30);
		this.pnlTop.Name = "pnlTop";
		this.pnlTop.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
		this.pnlTop.Size = new System.Drawing.Size(860, 30);
		this.pnlTop.TabIndex = 13;
		this.btnHelpMacroEditor.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnHelpMacroEditor.Image = TGMacro.Properties.Resources.help_20;
		this.btnHelpMacroEditor.Location = new System.Drawing.Point(780, 5);
		this.btnHelpMacroEditor.Name = "btnHelpMacroEditor";
		this.btnHelpMacroEditor.Size = new System.Drawing.Size(20, 20);
		this.btnHelpMacroEditor.TabIndex = 12;
		this.btnHelpMacroEditor.TabStop = false;
		this.btnMin.BackgroundImage = TGMacro.Properties.Resources.min_16;
		this.btnMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnMin.HoverImage = TGMacro.Properties.Resources.min_16_hover;
		this.btnMin.Location = new System.Drawing.Point(806, 0);
		this.btnMin.Name = "btnMin";
		this.btnMin.Size = new System.Drawing.Size(20, 30);
		this.btnMin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.btnMin.TabIndex = 10;
		this.btnMin.TabStop = false;
		this.btnClose.BackgroundImage = TGMacro.Properties.Resources.close_16;
		this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnClose.HoverImage = TGMacro.Properties.Resources.close_16_hover;
		this.btnClose.Location = new System.Drawing.Point(830, 0);
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
		this.lblTitle.Size = new System.Drawing.Size(91, 30);
		this.lblTitle.TabIndex = 6;
		this.lblTitle.Text = "Macro Editor";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.grpRecord.Controls.Add(this.btnRecord);
		this.grpRecord.Controls.Add(this.cbRecordDelays);
		this.grpRecord.Controls.Add(this.cbRecordKeyboardKeys);
		this.grpRecord.Controls.Add(this.cbRecordMouseKeys);
		this.grpRecord.Controls.Add(this.cbRecordMouseMovements);
		this.grpRecord.Controls.Add(this.lblRecordThis);
		this.grpRecord.Controls.Add(this.lblMouseCapPos);
		this.grpRecord.Controls.Add(this.cbMousePositioning);
		this.grpRecord.Location = new System.Drawing.Point(491, 414);
		this.grpRecord.Name = "grpRecord";
		this.grpRecord.Size = new System.Drawing.Size(354, 198);
		this.grpRecord.TabIndex = 12;
		this.grpRecord.TabStop = false;
		this.grpRecord.Text = "Record Settings";
		this.btnRecord.BackColor = System.Drawing.Color.CornflowerBlue;
		this.btnRecord.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		this.btnRecord.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(91, 91, 91);
		this.btnRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRecord.HoverColor = null;
		this.btnRecord.Image = TGMacro.Properties.Resources.record_16;
		this.btnRecord.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.btnRecord.Location = new System.Drawing.Point(68, 157);
		this.btnRecord.Name = "btnRecord";
		this.btnRecord.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
		this.btnRecord.Size = new System.Drawing.Size(213, 28);
		this.btnRecord.TabIndex = 13;
		this.btnRecord.Text = "Record (Ctrl + F10)";
		this.btnRecord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnRecord.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
		this.btnRecord.UseVisualStyleBackColor = false;
		this.cbRecordDelays.AutoSize = true;
		this.cbRecordDelays.Checked = true;
		this.cbRecordDelays.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbRecordDelays.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cbRecordDelays.Location = new System.Drawing.Point(199, 130);
		this.cbRecordDelays.Name = "cbRecordDelays";
		this.cbRecordDelays.Size = new System.Drawing.Size(63, 21);
		this.cbRecordDelays.TabIndex = 12;
		this.cbRecordDelays.Text = "Delays";
		this.cbRecordDelays.UseVisualStyleBackColor = true;
		this.cbRecordKeyboardKeys.AutoSize = true;
		this.cbRecordKeyboardKeys.Checked = true;
		this.cbRecordKeyboardKeys.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbRecordKeyboardKeys.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cbRecordKeyboardKeys.Location = new System.Drawing.Point(199, 100);
		this.cbRecordKeyboardKeys.Name = "cbRecordKeyboardKeys";
		this.cbRecordKeyboardKeys.Size = new System.Drawing.Size(110, 21);
		this.cbRecordKeyboardKeys.TabIndex = 11;
		this.cbRecordKeyboardKeys.Text = "Keyboard Keys";
		this.cbRecordKeyboardKeys.UseVisualStyleBackColor = true;
		this.cbRecordMouseKeys.AutoSize = true;
		this.cbRecordMouseKeys.Checked = true;
		this.cbRecordMouseKeys.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbRecordMouseKeys.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cbRecordMouseKeys.Location = new System.Drawing.Point(21, 127);
		this.cbRecordMouseKeys.Name = "cbRecordMouseKeys";
		this.cbRecordMouseKeys.Size = new System.Drawing.Size(110, 21);
		this.cbRecordMouseKeys.TabIndex = 10;
		this.cbRecordMouseKeys.Text = "Mouse Buttons";
		this.cbRecordMouseKeys.UseVisualStyleBackColor = true;
		this.cbRecordMouseMovements.AutoSize = true;
		this.cbRecordMouseMovements.Checked = true;
		this.cbRecordMouseMovements.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbRecordMouseMovements.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cbRecordMouseMovements.Location = new System.Drawing.Point(20, 100);
		this.cbRecordMouseMovements.Name = "cbRecordMouseMovements";
		this.cbRecordMouseMovements.Size = new System.Drawing.Size(137, 21);
		this.cbRecordMouseMovements.TabIndex = 9;
		this.cbRecordMouseMovements.Text = "Mouse Movements";
		this.cbRecordMouseMovements.UseVisualStyleBackColor = true;
		this.lblRecordThis.AutoSize = true;
		this.lblRecordThis.Location = new System.Drawing.Point(16, 77);
		this.lblRecordThis.Name = "lblRecordThis";
		this.lblRecordThis.Size = new System.Drawing.Size(57, 17);
		this.lblRecordThis.TabIndex = 8;
		this.lblRecordThis.Text = "Record :";
		this.lblMouseCapPos.AutoSize = true;
		this.lblMouseCapPos.Location = new System.Drawing.Point(14, 25);
		this.lblMouseCapPos.Name = "lblMouseCapPos";
		this.lblMouseCapPos.Size = new System.Drawing.Size(128, 17);
		this.lblMouseCapPos.TabIndex = 7;
		this.lblMouseCapPos.Text = "Mouse's positioning :";
		this.cbMousePositioning.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
		this.cbMousePositioning.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.cbMousePositioning.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
		this.cbMousePositioning.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbMousePositioning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cbMousePositioning.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.cbMousePositioning.FormattingEnabled = true;
		this.cbMousePositioning.Location = new System.Drawing.Point(14, 47);
		this.cbMousePositioning.Name = "cbMousePositioning";
		this.cbMousePositioning.Size = new System.Drawing.Size(321, 23);
		this.cbMousePositioning.TabIndex = 6;
		this.btnTest.BackColor = System.Drawing.SystemColors.ControlLight;
		this.btnTest.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnTest.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(91, 91, 91);
		this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnTest.HoverColor = null;
		this.btnTest.Image = TGMacro.Properties.Resources.start_32;
		this.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnTest.Location = new System.Drawing.Point(514, 618);
		this.btnTest.Name = "btnTest";
		this.btnTest.Size = new System.Drawing.Size(137, 41);
		this.btnTest.TabIndex = 11;
		this.btnTest.Text = "Test";
		this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btnTest.UseVisualStyleBackColor = false;
		this.btnSave.BackColor = System.Drawing.SystemColors.ControlLight;
		this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(91, 91, 91);
		this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSave.HoverColor = null;
		this.btnSave.Image = TGMacro.Properties.Resources.save_32;
		this.btnSave.Location = new System.Drawing.Point(708, 618);
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(137, 41);
		this.btnSave.TabIndex = 6;
		this.btnSave.Text = "Save";
		this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
		this.btnSave.UseVisualStyleBackColor = false;
		this.lblLoading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblLoading.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblLoading.Font = new System.Drawing.Font("Century Gothic", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 162);
		this.lblLoading.Location = new System.Drawing.Point(0, 0);
		this.lblLoading.Name = "lblLoading";
		this.lblLoading.Size = new System.Drawing.Size(862, 674);
		this.lblLoading.TabIndex = 12;
		this.lblLoading.Text = "Loading..";
		this.lblLoading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 17f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(862, 674);
		base.Controls.Add(this.pnlMain);
		base.Controls.Add(this.lblLoading);
		this.Font = new System.Drawing.Font("Century Gothic", 9f);
		this.ForeColor = System.Drawing.Color.Black;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.Name = "frmMacroEditor";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "TGM Editor";
		this.grpTrigger.ResumeLayout(false);
		this.grpTrigger.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnHelpTriggers).EndInit();
		this.pnl_colors.ResumeLayout(false);
		this.pnl_colors.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.txtColorTolerance).EndInit();
		((System.ComponentModel.ISupportInitialize)this.mPosY).EndInit();
		((System.ComponentModel.ISupportInitialize)this.mPosX).EndInit();
		this.pnl_keys.ResumeLayout(false);
		this.pnl_keys.PerformLayout();
		this.grpActions.ResumeLayout(false);
		this.grpActions.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dtActions).EndInit();
		this.grpName.ResumeLayout(false);
		this.grpName.PerformLayout();
		this.pnlMain.ResumeLayout(false);
		this.pnlTop.ResumeLayout(false);
		this.pnlTop.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnHelpMacroEditor).EndInit();
		((System.ComponentModel.ISupportInitialize)this.btnMin).EndInit();
		((System.ComponentModel.ISupportInitialize)this.btnClose).EndInit();
		this.grpRecord.ResumeLayout(false);
		this.grpRecord.PerformLayout();
		base.ResumeLayout(false);
	}
}
