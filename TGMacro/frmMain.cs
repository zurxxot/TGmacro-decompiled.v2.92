using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using CSInputs.Enums;
using CSInputs.ReadInput;
using CSInputs.Structs;
using TGMacro.Interfaces;
using TGMacro.Properties;
using TGMacro.Statics;
using TGMacro.SysModels;
using TGMacro.UI.CustomControls;

namespace TGMacro;

public class frmMain : DoubleBufferForm
{
	private string loadProjectOnStart = string.Empty;

	private bool isSaved = true;

	private IContainer components = null;

	private Panel pnlMain;

	private FlowLayoutPanel pnlMacroList;

	private Label lblNoMacroMessage;

	private SaveFileDialog saveFileDialog1;

	private OpenFileDialog openFileDialog1;

	private HoverButton btnEnable;

	private HoverButton btnAdd;

	public Panel pnlTop;

	public HoverPictureBox btnStatus;

	public HoverButton btnHelp;

	public HoverButton btnFile;

	public Label lblTitle;

	private ContextMenuStrip ctHelpMenu;

	public ToolStripMenuItem btnUpdates;

	public ToolStripMenuItem btnAbout;

	private ContextMenuStrip ctFileMenu;

	public ToolStripMenuItem btnLoadProject;

	public ToolStripMenuItem btnSaveProject;

	public ToolStripMenuItem btnImportProject;

	public ToolStripMenuItem btnImportScript;

	private ToolTip toolTip1;

	private Panel pnlContent;

	public HoverPictureBox btnMin;

	public HoverPictureBox btnClose;

	public HoverPictureBox btnTopMost;

	private ToolStripMenuItem btnClear;

	private ToolStripMenuItem btnSaveProjectAs;

	private Timer tmrUpdate;

	private Label lblLoading;

	private ToolStripMenuItem btnHowToUse;

	private ToolStripMenuItem btnSecurity;

	private PictureBox btnHelpMainMenu;

	public HoverPictureBox btnSettings;

	private NotifyIcon notifyAppIcon;

	public frmMain(string param)
	{
		InitializeComponent();
		notifyAppIcon.Text = "TGMacro " + AppSession.Version;
		lblLoading.BringToFront();
		base.TopMost = AppSession.TopMost;
		loadProjectOnStart = param;
		base.Shown += FrmMain_Shown;
		UpdateColors();
	}

	private void UpdateColors()
	{
		pnlTop.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		BackColor = AppSession.Settings.ColorPalette.LightColor;
		lblNoMacroMessage.BackColor = AppSession.Settings.ColorPalette.MidColor;
		lblNoMacroMessage.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		lblLoading.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		pnlMacroList.BackColor = AppSession.Settings.ColorPalette.MidColor;
		pnlMacroList.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		pnlMacroList.BackColor = AppSession.Settings.ColorPalette.MidColor;
		pnlMacroList.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		btnAdd.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		btnEnable.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		btnEnable.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		btnEnable.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		btnAdd.BackColor = AppSession.Settings.ColorPalette.DarkColor;
		btnAdd.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		ctFileMenu.Renderer = new CustomToolStripRenderer();
		ctHelpMenu.Renderer = new CustomToolStripRenderer();
		btnAdd.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		btnFile.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		btnHelp.ForeColor = AppSession.Settings.ColorPalette.TextColor;
		lblTitle.ForeColor = AppSession.Settings.ColorPalette.TextColor;
	}

	private void LoadLanguage()
	{
		btnClear.Text = AppSession.ActiveLanguage.clearProjects;
		btnFile.Text = AppSession.ActiveLanguage.btnFile;
		lblNoMacroMessage.Text = AppSession.ActiveLanguage.info_no_macro;
		btnHelp.Text = AppSession.ActiveLanguage.btnHelp;
		btnAbout.Text = AppSession.ActiveLanguage.btnAbout;
		btnAdd.Text = AppSession.ActiveLanguage.btnAdd;
		UpdateEnableButtonText();
		btnImportProject.Text = AppSession.ActiveLanguage.btnImportProject;
		btnImportScript.Text = AppSession.ActiveLanguage.btnImportScript;
		btnLoadProject.Text = AppSession.ActiveLanguage.btnLoadProject;
		btnSaveProject.Text = AppSession.ActiveLanguage.btnSaveProject;
		btnSaveProjectAs.Text = AppSession.ActiveLanguage.btnSaveProjectAs;
		btnUpdates.Text = AppSession.ActiveLanguage.btnUpdates;
		btnHowToUse.Text = AppSession.ActiveLanguage.howToUse;
		toolTip1.SetToolTip(btnStatus, AppSession.ActiveLanguage.text_macro_status);
		toolTip1.SetToolTip(btnTopMost, AppSession.ActiveLanguage.btnTopMost);
		toolTip1.SetToolTip(btnHelpMainMenu, AppSession.ActiveLanguage.btnHelp + "?");
	}

	private void FrmMain_Shown(object sender, EventArgs e)
	{
		Application.DoEvents();
		AppSession.InputListener = new InputListener();
		LoadLanguage();
		checkTopMostButton();
		registerEvents();
		AppSession.InputListener.KeyboardInputs += Hook_KeyboardInputs;
		CheckForUpdates();
		if (!string.IsNullOrEmpty(loadProjectOnStart))
		{
			LoadProject(loadProjectOnStart);
		}
		lblLoading.Visible = false;
		AppSession.Enable = AppSession.Settings.EnableOnStart;
		SharedMethods.SaveSettings();
	}

	private async Task CheckForUpdates()
	{
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
		try
		{
			string result = await SharedMethods.GetTextFromServer("http://trksyln.net/tgmacro/VersionInfo.json");
			if (!string.IsNullOrWhiteSpace(result))
			{
				VersionInfoModel data = new JavaScriptSerializer().Deserialize<VersionInfoModel>(result);
				if (!data.FinalVersion.Trim().Equals(AppSession.Version))
				{
					AppSession.NewVersionInfo = data;
					tmrUpdate.Start();
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private void Hook_KeyboardInputs(KeyboardData data, ref ModifierKey modifierKey)
	{
		if ((int)data.Key == AppSession.Settings.ActivationKey && (int)SharedMethods.ModifierKeyToKey(modifierKey) == AppSession.Settings.ActivationKeyModifier && data.Flags == KeyFlags.Up)
		{
			AppSession.Enable = !AppSession.Enable;
		}
		if (Form.ActiveForm == this)
		{
			if (modifierKey.Control && data.Key == CSInputs.Enums.KeyboardKeys.S && data.Flags == KeyFlags.Up)
			{
				ShowSaveDialog(AppSession.LastSavePath);
			}
			if (modifierKey.Control && data.Key == CSInputs.Enums.KeyboardKeys.O && data.Flags == KeyFlags.Up)
			{
				ShowLoadDialog();
			}
			if (modifierKey.Control && data.Key == CSInputs.Enums.KeyboardKeys.I && data.Flags == KeyFlags.Up)
			{
				ShowImportDialog();
			}
		}
	}

	private void checkTopMostButton()
	{
		if (AppSession.TopMost)
		{
			btnTopMost.Image = Resources.lock_16;
			btnTopMost.HoverImage = Resources.unlock_16_hover;
		}
		else
		{
			btnTopMost.Image = Resources.unlock_16;
			btnTopMost.HoverImage = Resources.lock_16_hover;
		}
	}

	private void UpdateEnableButtonText()
	{
		btnEnable.Text = (AppSession.Enable ? AppSession.ActiveLanguage.btnEnable_on : AppSession.ActiveLanguage.btnEnable_off) + " { " + ((AppSession.Settings.ActivationKeyModifier != 0) ? (((CSInputs.Enums.KeyboardKeys)AppSession.Settings.ActivationKeyModifier/*cast due to .constrained prefix*/).ToString() + " + " + (CSInputs.Enums.KeyboardKeys)AppSession.Settings.ActivationKey/*cast due to .constrained prefix*/) : ((CSInputs.Enums.KeyboardKeys)AppSession.Settings.ActivationKey/*cast due to .constrained prefix*/).ToString()) + " }";
	}

	private void registerEvents()
	{
		AppSession.LanguageChanged += delegate
		{
			LoadLanguage();
		};
		btnSettings.Click += delegate
		{
			new frmSettings().ShowDialog();
			UpdateEnableButtonText();
			UpdateColors();
		};
		btnFile.Click += delegate
		{
			Point screenLocation = PointToScreen(btnFile.Location);
			screenLocation.Y += btnFile.Height;
			ctFileMenu.Show(screenLocation);
		};
		btnHelp.Click += delegate
		{
			Point screenLocation = PointToScreen(btnHelp.Location);
			screenLocation.Y += btnHelp.Height;
			ctHelpMenu.Show(screenLocation);
		};
		btnUpdates.Click += delegate
		{
			try
			{
				Process.Start("https://trksyln.net/projects/TGM-Gaming-Macro-(TGMacro)-4/");
			}
			catch
			{
			}
		};
		btnHowToUse.Click += delegate
		{
			try
			{
				Process.Start("https://trksyln.net/projects/TGM-Gaming-Macro-(TGMacro)-4/How-to-use-5");
			}
			catch
			{
			}
		};
		btnHelpMainMenu.Click += delegate
		{
			try
			{
				Process.Start("https://trksyln.net/projects/TGM-Gaming-Macro-(TGMacro)-4/How-to-use-5#mainmenu");
			}
			catch
			{
			}
		};
		btnClose.Click += delegate
		{
			Close();
		};
		btnMin.Click += delegate
		{
			if (AppSession.Settings.MoveToTrayWhenMinimized)
			{
				base.Visible = false;
			}
			else
			{
				base.WindowState = FormWindowState.Minimized;
			}
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
			checkTopMostButton();
			base.TopMost = AppSession.TopMost;
		};
		btnEnable.Click += delegate
		{
			AppSession.Enable = !AppSession.Enable;
		};
		btnStatus.Click += delegate
		{
			AppSession.Enable = !AppSession.Enable;
		};
		btnTopMost.Click += delegate
		{
			AppSession.TopMost = !AppSession.TopMost;
		};
		AppSession.EnableStatusChanged += delegate
		{
			if (AppSession.Enable)
			{
				btnStatus.BackgroundImage = Resources.playing_16;
				btnEnable.Image = Resources.stop_32;
				if (notifyAppIcon != null)
				{
					notifyAppIcon.Icon = Resources.tgm_active;
				}
			}
			else
			{
				btnStatus.BackgroundImage = Resources.paused_16;
				btnEnable.Image = Resources.start_32;
				if (notifyAppIcon != null)
				{
					notifyAppIcon.Icon = Resources.tgm;
				}
			}
			UpdateEnableButtonText();
		};
		btnAdd.Click += delegate
		{
			frmMacroEditor frmMacroEditor2 = new frmMacroEditor();
			if (frmMacroEditor2.ShowDialog() == DialogResult.OK)
			{
				MacroItem macro = frmMacroEditor2.Macro;
				ucMacroItem value = new ucMacroItem(macro, pnlMacroList);
				pnlMacroList.Controls.Add(value);
			}
		};
		pnlMacroList.ControlAdded += delegate(object s, ControlEventArgs e)
		{
			lblNoMacroMessage.Visible = pnlMacroList.Controls.Count < 1;
			ucMacroItem ucMacroItem = e.Control as ucMacroItem;
			AppSession.Macros.Add(ucMacroItem.Macro);
			ResizeMacroItems();
			isSaved = false;
		};
		pnlMacroList.ControlRemoved += delegate(object s, ControlEventArgs e)
		{
			lblNoMacroMessage.Visible = pnlMacroList.Controls.Count < 1;
			AppSession.Macros.Remove((e.Control as ucMacroItem).Macro);
			ResizeMacroItems();
			isSaved = false;
		};
		btnSaveProject.Click += delegate
		{
			ShowSaveDialog(AppSession.LastSavePath);
		};
		btnSaveProjectAs.Click += delegate
		{
			ShowSaveDialog();
		};
		btnClear.Click += delegate
		{
			if (AppSession.Macros.Count >= 1 && (isSaved || Messenger.Show("All unsaved data will be lost?", "Warning", "OK", "CANCEL") == DialogResult.Yes))
			{
				AppSession.LastSavePath = null;
				isSaved = true;
				AppSession.Macros.Clear();
				pnlMacroList.Controls.Clear();
			}
		};
		base.FormClosing += delegate(object s, FormClosingEventArgs e)
		{
			if (AppSession.Macros.Count >= 1 && !isSaved)
			{
				DialogResult dialogResult = Messenger.Show(AppSession.ActiveLanguage.someUnsavedChanges, "Warning", "CONTINE", "CANCEL");
				if (dialogResult != DialogResult.Yes)
				{
					e.Cancel = true;
				}
			}
		};
		base.FormClosed += delegate
		{
			notifyAppIcon.Visible = false;
			notifyAppIcon.Dispose();
			foreach (MacroItem macro2 in AppSession.Macros)
			{
				macro2.Dispose();
			}
			Environment.Exit(0);
		};
		btnSecurity.Click += delegate
		{
			HostHandler.ShowWarning(showResultIfNoAction: true);
		};
		btnLoadProject.Click += delegate
		{
			ShowLoadDialog();
		};
		btnImportProject.Click += delegate
		{
			ShowImportDialog();
		};
		btnImportScript.Click += delegate
		{
			frmMacroEditor frmMacroEditor2 = new frmMacroEditor(null, loadScript: true);
			if (frmMacroEditor2.ShowDialog() == DialogResult.OK)
			{
				MacroItem macro = frmMacroEditor2.Macro;
				ucMacroItem value = new ucMacroItem(macro, pnlMacroList);
				pnlMacroList.Controls.Add(value);
			}
		};
		btnAbout.Click += delegate
		{
			new frmAbout().ShowDialog();
		};
		notifyAppIcon.DoubleClick += delegate
		{
			base.Visible = true;
			base.WindowState = FormWindowState.Normal;
			Activate();
		};
	}

	private void ShowSaveDialog(string path = "")
	{
		if (AppSession.Macros.Count < 1)
		{
			Messenger.Show(AppSession.ActiveLanguage.info_nothing_to_save, "Error");
			return;
		}
		saveFileDialog1.Title = "Save Project";
		if (string.IsNullOrEmpty(path))
		{
			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				SaveProject(saveFileDialog1.FileName);
				AppSession.Settings.LastFile = saveFileDialog1.FileName;
				isSaved = true;
			}
		}
		else
		{
			SaveProject(path);
			AppSession.Settings.LastFile = path;
		}
	}

	private void ShowLoadDialog()
	{
		openFileDialog1.Title = "Open Project";
		if (openFileDialog1.ShowDialog() == DialogResult.OK)
		{
			LoadProject(openFileDialog1.FileName);
			AppSession.Settings.LastFile = openFileDialog1.FileName;
		}
	}

	private void ShowImportDialog()
	{
		openFileDialog1.Title = "Import Project";
		if (openFileDialog1.ShowDialog() == DialogResult.OK)
		{
			ImportProject(openFileDialog1.FileName);
			AppSession.Settings.LastFile = openFileDialog1.FileName;
		}
	}

	private void SaveProject(string filePath)
	{
		try
		{
			TGMacroProject tGMacroProject = new TGMacroProject();
			List<TGMacroProjectMacro> list = new List<TGMacroProjectMacro>();
			foreach (MacroItem macro in AppSession.Macros)
			{
				TGMacroProjectMacro tGMacroProjectMacro = new TGMacroProjectMacro(macro.Name)
				{
					Trigger = new TGMacroProjectTrigger(macro.Trigger)
				};
				List<TGMacroProjectAction> list2 = new List<TGMacroProjectAction>();
				foreach (IMacroAction action in macro.Trigger.Actions)
				{
					list2.Add(new TGMacroProjectAction(action));
				}
				tGMacroProjectMacro.Actions = list2.ToArray();
				list.Add(tGMacroProjectMacro);
			}
			tGMacroProject.Macros = list.ToArray();
			SaveProjectFile(tGMacroProject, filePath);
			isSaved = true;
			AppSession.LastSavePath = filePath;
		}
		catch
		{
			Messenger.Show(AppSession.ActiveLanguage.error_saving_project, "Error");
		}
	}

	private void LoadProject(string filePath)
	{
		try
		{
			foreach (MacroItem macro in AppSession.Macros)
			{
				macro.Trigger.Dispose();
			}
			AppSession.Macros.Clear();
			pnlMacroList.Controls.Clear();
			TGMacroProject tGMacroProject = LoadProjectFile(filePath);
			TGMacroProjectMacro[] macros = tGMacroProject.Macros;
			foreach (TGMacroProjectMacro tGMacroProjectMacro in macros)
			{
				MacroItem macroItem = new MacroItem
				{
					Name = tGMacroProjectMacro.Name,
					Trigger = tGMacroProjectMacro.Trigger.GetTrigger()
				};
				List<IMacroAction> list = new List<IMacroAction>();
				TGMacroProjectAction[] actions = tGMacroProjectMacro.Actions;
				foreach (TGMacroProjectAction tGMacroProjectAction in actions)
				{
					list.Add(tGMacroProjectAction.GetAction());
				}
				macroItem.Trigger.Actions = list;
				ucMacroItem value = new ucMacroItem(macroItem, pnlMacroList);
				pnlMacroList.Controls.Add(value);
			}
			AppSession.LastSavePath = filePath;
			isSaved = true;
		}
		catch
		{
			Messenger.Show(AppSession.ActiveLanguage.error_loding_project, "Error");
		}
	}

	private void ImportProject(string filePath)
	{
		try
		{
			TGMacroProject tGMacroProject = LoadProjectFile(filePath);
			TGMacroProjectMacro[] macros = tGMacroProject.Macros;
			foreach (TGMacroProjectMacro tGMacroProjectMacro in macros)
			{
				MacroItem macroItem = new MacroItem
				{
					Name = tGMacroProjectMacro.Name,
					Trigger = tGMacroProjectMacro.Trigger.GetTrigger()
				};
				List<IMacroAction> list = new List<IMacroAction>();
				TGMacroProjectAction[] actions = tGMacroProjectMacro.Actions;
				foreach (TGMacroProjectAction tGMacroProjectAction in actions)
				{
					list.Add(tGMacroProjectAction.GetAction());
				}
				macroItem.Trigger.Actions = list;
				ucMacroItem value = new ucMacroItem(macroItem, pnlMacroList);
				pnlMacroList.Controls.Add(value);
			}
		}
		catch
		{
			Messenger.Show(AppSession.ActiveLanguage.error_importing_project, "Error");
		}
	}

	private void SaveProjectFile(TGMacroProject obj, string path)
	{
		if (File.Exists(path))
		{
			try
			{
				File.Delete(path);
			}
			catch
			{
			}
		}
		DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(obj.GetType());
		using FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
		dataContractJsonSerializer.WriteObject((Stream)stream, (object)obj);
	}

	private TGMacroProject LoadProjectFile(string path)
	{
		DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(TGMacroProject));
		using FileStream stream = new FileStream(path, FileMode.Open);
		return (TGMacroProject)dataContractJsonSerializer.ReadObject((Stream)stream);
	}

	private void ResizeMacroItems()
	{
		foreach (ucMacroItem control in pnlMacroList.Controls)
		{
			control.Width = (pnlMacroList.VerticalScroll.Visible ? (pnlMacroList.Width - 30) : (pnlMacroList.Width - 6));
		}
	}

	private void tmrUpdate_Tick(object sender, EventArgs e)
	{
		if (!AppSession.Enable)
		{
			tmrUpdate.Stop();
			if (Messenger.Show(AppSession.NewVersionInfo.WhatsNew ?? "", AppSession.NewVersionInfo.Title, AppSession.NewVersionInfo.YesText, AppSession.NewVersionInfo.NoText, "", 1) == DialogResult.Yes)
			{
				Process.Start(AppSession.NewVersionInfo.DownloadUrl);
			}
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TGMacro.frmMain));
		this.pnlMain = new System.Windows.Forms.Panel();
		this.btnAdd = new TGMacro.UI.CustomControls.HoverButton();
		this.btnEnable = new TGMacro.UI.CustomControls.HoverButton();
		this.lblNoMacroMessage = new System.Windows.Forms.Label();
		this.pnlMacroList = new System.Windows.Forms.FlowLayoutPanel();
		this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
		this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
		this.pnlTop = new System.Windows.Forms.Panel();
		this.btnSettings = new TGMacro.UI.CustomControls.HoverPictureBox();
		this.btnHelpMainMenu = new System.Windows.Forms.PictureBox();
		this.btnMin = new TGMacro.UI.CustomControls.HoverPictureBox();
		this.btnClose = new TGMacro.UI.CustomControls.HoverPictureBox();
		this.btnStatus = new TGMacro.UI.CustomControls.HoverPictureBox();
		this.btnTopMost = new TGMacro.UI.CustomControls.HoverPictureBox();
		this.btnHelp = new TGMacro.UI.CustomControls.HoverButton();
		this.btnFile = new TGMacro.UI.CustomControls.HoverButton();
		this.lblTitle = new System.Windows.Forms.Label();
		this.ctHelpMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.btnSecurity = new System.Windows.Forms.ToolStripMenuItem();
		this.btnHowToUse = new System.Windows.Forms.ToolStripMenuItem();
		this.btnUpdates = new System.Windows.Forms.ToolStripMenuItem();
		this.btnAbout = new System.Windows.Forms.ToolStripMenuItem();
		this.ctFileMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.btnLoadProject = new System.Windows.Forms.ToolStripMenuItem();
		this.btnSaveProject = new System.Windows.Forms.ToolStripMenuItem();
		this.btnSaveProjectAs = new System.Windows.Forms.ToolStripMenuItem();
		this.btnImportProject = new System.Windows.Forms.ToolStripMenuItem();
		this.btnImportScript = new System.Windows.Forms.ToolStripMenuItem();
		this.btnClear = new System.Windows.Forms.ToolStripMenuItem();
		this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
		this.pnlContent = new System.Windows.Forms.Panel();
		this.lblLoading = new System.Windows.Forms.Label();
		this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
		this.notifyAppIcon = new System.Windows.Forms.NotifyIcon(this.components);
		this.pnlMain.SuspendLayout();
		this.pnlTop.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnSettings).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.btnHelpMainMenu).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.btnMin).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.btnClose).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.btnStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.btnTopMost).BeginInit();
		this.ctHelpMenu.SuspendLayout();
		this.ctFileMenu.SuspendLayout();
		this.pnlContent.SuspendLayout();
		base.SuspendLayout();
		this.pnlMain.Controls.Add(this.btnAdd);
		this.pnlMain.Controls.Add(this.btnEnable);
		this.pnlMain.Controls.Add(this.lblNoMacroMessage);
		this.pnlMain.Controls.Add(this.pnlMacroList);
		this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnlMain.Location = new System.Drawing.Point(0, 30);
		this.pnlMain.Name = "pnlMain";
		this.pnlMain.Padding = new System.Windows.Forms.Padding(10, 10, 10, 60);
		this.pnlMain.Size = new System.Drawing.Size(462, 542);
		this.pnlMain.TabIndex = 0;
		this.btnAdd.BackColor = System.Drawing.SystemColors.ControlLight;
		this.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(91, 91, 91);
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.HoverColor = null;
		this.btnAdd.Image = TGMacro.Properties.Resources.add_32;
		this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btnAdd.Location = new System.Drawing.Point(261, 492);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
		this.btnAdd.Size = new System.Drawing.Size(193, 41);
		this.btnAdd.TabIndex = 16;
		this.btnAdd.Text = "Add New Macro";
		this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnEnable.BackColor = System.Drawing.SystemColors.ControlLight;
		this.btnEnable.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnEnable.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(91, 91, 91);
		this.btnEnable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnEnable.HoverColor = null;
		this.btnEnable.Image = TGMacro.Properties.Resources.start_32;
		this.btnEnable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnEnable.Location = new System.Drawing.Point(10, 492);
		this.btnEnable.Name = "btnEnable";
		this.btnEnable.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
		this.btnEnable.Size = new System.Drawing.Size(193, 41);
		this.btnEnable.TabIndex = 15;
		this.btnEnable.Text = "Enable";
		this.btnEnable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnEnable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btnEnable.UseVisualStyleBackColor = false;
		this.lblNoMacroMessage.BackColor = System.Drawing.SystemColors.ControlLight;
		this.lblNoMacroMessage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblNoMacroMessage.ForeColor = System.Drawing.SystemColors.WindowText;
		this.lblNoMacroMessage.Location = new System.Drawing.Point(10, 10);
		this.lblNoMacroMessage.Name = "lblNoMacroMessage";
		this.lblNoMacroMessage.Size = new System.Drawing.Size(442, 472);
		this.lblNoMacroMessage.TabIndex = 13;
		this.lblNoMacroMessage.Text = "No macro to show.\r\n\r\nClick \"Add New\" to create macro.";
		this.lblNoMacroMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.pnlMacroList.AutoScroll = true;
		this.pnlMacroList.BackColor = System.Drawing.SystemColors.ControlLight;
		this.pnlMacroList.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnlMacroList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
		this.pnlMacroList.Location = new System.Drawing.Point(10, 10);
		this.pnlMacroList.Name = "pnlMacroList";
		this.pnlMacroList.Size = new System.Drawing.Size(442, 472);
		this.pnlMacroList.TabIndex = 0;
		this.pnlMacroList.WrapContents = false;
		this.saveFileDialog1.Filter = "TGMacro Project |*.tmacroproj";
		this.saveFileDialog1.Title = "Save Project";
		this.openFileDialog1.Filter = "TGMacro Project |*.TMacroProj";
		this.openFileDialog1.Title = "Load Project";
		this.pnlTop.BackColor = System.Drawing.SystemColors.ControlLight;
		this.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnlTop.Controls.Add(this.btnSettings);
		this.pnlTop.Controls.Add(this.btnHelpMainMenu);
		this.pnlTop.Controls.Add(this.btnMin);
		this.pnlTop.Controls.Add(this.btnClose);
		this.pnlTop.Controls.Add(this.btnStatus);
		this.pnlTop.Controls.Add(this.btnTopMost);
		this.pnlTop.Controls.Add(this.btnHelp);
		this.pnlTop.Controls.Add(this.btnFile);
		this.pnlTop.Controls.Add(this.lblTitle);
		this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnlTop.Font = new System.Drawing.Font("Century Gothic", 9f);
		this.pnlTop.Location = new System.Drawing.Point(0, 0);
		this.pnlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.pnlTop.Name = "pnlTop";
		this.pnlTop.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
		this.pnlTop.Size = new System.Drawing.Size(462, 30);
		this.pnlTop.TabIndex = 6;
		this.btnSettings.BackColor = System.Drawing.Color.Transparent;
		this.btnSettings.BackgroundImage = TGMacro.Properties.Resources.settings_16;
		this.btnSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btnSettings.HoverImage = null;
		this.btnSettings.Location = new System.Drawing.Point(305, -1);
		this.btnSettings.Name = "btnSettings";
		this.btnSettings.Size = new System.Drawing.Size(20, 30);
		this.btnSettings.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.btnSettings.TabIndex = 24;
		this.btnSettings.TabStop = false;
		this.btnHelpMainMenu.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnHelpMainMenu.Image = TGMacro.Properties.Resources.help_20;
		this.btnHelpMainMenu.Location = new System.Drawing.Point(331, 4);
		this.btnHelpMainMenu.Name = "btnHelpMainMenu";
		this.btnHelpMainMenu.Size = new System.Drawing.Size(20, 20);
		this.btnHelpMainMenu.TabIndex = 22;
		this.btnHelpMainMenu.TabStop = false;
		this.btnMin.BackgroundImage = TGMacro.Properties.Resources.min_16;
		this.btnMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnMin.HoverImage = TGMacro.Properties.Resources.min_16_hover;
		this.btnMin.Location = new System.Drawing.Point(406, -1);
		this.btnMin.Name = "btnMin";
		this.btnMin.Size = new System.Drawing.Size(20, 30);
		this.btnMin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.btnMin.TabIndex = 21;
		this.btnMin.TabStop = false;
		this.btnClose.BackgroundImage = TGMacro.Properties.Resources.close_16;
		this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnClose.HoverImage = TGMacro.Properties.Resources.close_16_hover;
		this.btnClose.Location = new System.Drawing.Point(430, 0);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(20, 28);
		this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.btnClose.TabIndex = 20;
		this.btnClose.TabStop = false;
		this.btnStatus.BackColor = System.Drawing.Color.Transparent;
		this.btnStatus.BackgroundImage = TGMacro.Properties.Resources.paused_16;
		this.btnStatus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnStatus.HoverImage = null;
		this.btnStatus.Location = new System.Drawing.Point(356, 0);
		this.btnStatus.Name = "btnStatus";
		this.btnStatus.Size = new System.Drawing.Size(20, 30);
		this.btnStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.btnStatus.TabIndex = 11;
		this.btnStatus.TabStop = false;
		this.btnTopMost.BackColor = System.Drawing.Color.Transparent;
		this.btnTopMost.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.btnTopMost.HoverImage = TGMacro.Properties.Resources.unlock_16;
		this.btnTopMost.Image = TGMacro.Properties.Resources.unlock_16_hover;
		this.btnTopMost.Location = new System.Drawing.Point(381, -1);
		this.btnTopMost.Name = "btnTopMost";
		this.btnTopMost.Size = new System.Drawing.Size(21, 30);
		this.btnTopMost.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.btnTopMost.TabIndex = 10;
		this.btnTopMost.TabStop = false;
		this.btnHelp.AutoSize = true;
		this.btnHelp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.btnHelp.BackColor = System.Drawing.Color.Transparent;
		this.btnHelp.Dock = System.Windows.Forms.DockStyle.Left;
		this.btnHelp.FlatAppearance.BorderSize = 0;
		this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnHelp.HoverColor = null;
		this.btnHelp.Location = new System.Drawing.Point(120, 0);
		this.btnHelp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.btnHelp.Name = "btnHelp";
		this.btnHelp.Size = new System.Drawing.Size(45, 28);
		this.btnHelp.TabIndex = 8;
		this.btnHelp.Text = "&Help";
		this.btnHelp.UseVisualStyleBackColor = false;
		this.btnFile.AutoSize = true;
		this.btnFile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.btnFile.BackColor = System.Drawing.Color.Transparent;
		this.btnFile.Dock = System.Windows.Forms.DockStyle.Left;
		this.btnFile.FlatAppearance.BorderSize = 0;
		this.btnFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnFile.HoverColor = null;
		this.btnFile.Location = new System.Drawing.Point(82, 0);
		this.btnFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.btnFile.Name = "btnFile";
		this.btnFile.Size = new System.Drawing.Size(38, 28);
		this.btnFile.TabIndex = 7;
		this.btnFile.Text = "&File";
		this.btnFile.UseVisualStyleBackColor = false;
		this.lblTitle.AutoSize = true;
		this.lblTitle.BackColor = System.Drawing.Color.Transparent;
		this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
		this.lblTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblTitle.Font = new System.Drawing.Font("Century Gothic", 9f, System.Drawing.FontStyle.Bold);
		this.lblTitle.Location = new System.Drawing.Point(10, 0);
		this.lblTitle.MaximumSize = new System.Drawing.Size(0, 30);
		this.lblTitle.MinimumSize = new System.Drawing.Size(0, 30);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Padding = new System.Windows.Forms.Padding(0, 0, 10, 2);
		this.lblTitle.Size = new System.Drawing.Size(72, 30);
		this.lblTitle.TabIndex = 6;
		this.lblTitle.Text = "TGMacro";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.ctHelpMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.ctHelpMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnSecurity, this.btnHowToUse, this.btnUpdates, this.btnAbout });
		this.ctHelpMenu.Name = "ctFileMenu";
		this.ctHelpMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.ctHelpMenu.Size = new System.Drawing.Size(146, 108);
		this.btnSecurity.Image = TGMacro.Properties.Resources.shield_16;
		this.btnSecurity.Name = "btnSecurity";
		this.btnSecurity.Size = new System.Drawing.Size(145, 26);
		this.btnSecurity.Text = "Security";
		this.btnHowToUse.ForeColor = System.Drawing.Color.Blue;
		this.btnHowToUse.Image = TGMacro.Properties.Resources.about_16;
		this.btnHowToUse.Name = "btnHowToUse";
		this.btnHowToUse.Size = new System.Drawing.Size(145, 26);
		this.btnHowToUse.Text = "How To Use?";
		this.btnUpdates.Image = TGMacro.Properties.Resources.update_16;
		this.btnUpdates.Name = "btnUpdates";
		this.btnUpdates.Size = new System.Drawing.Size(145, 26);
		this.btnUpdates.Text = "Updates";
		this.btnAbout.Image = TGMacro.Properties.Resources.about_16;
		this.btnAbout.Name = "btnAbout";
		this.btnAbout.Size = new System.Drawing.Size(145, 26);
		this.btnAbout.Text = "About";
		this.ctFileMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.ctFileMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnLoadProject, this.btnSaveProject, this.btnSaveProjectAs, this.btnImportProject, this.btnImportScript, this.btnClear });
		this.ctFileMenu.Name = "ctFileMenu";
		this.ctFileMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.ctFileMenu.Size = new System.Drawing.Size(159, 160);
		this.btnLoadProject.Image = TGMacro.Properties.Resources.open_16;
		this.btnLoadProject.ImageTransparentColor = System.Drawing.Color.Transparent;
		this.btnLoadProject.Name = "btnLoadProject";
		this.btnLoadProject.Size = new System.Drawing.Size(158, 26);
		this.btnLoadProject.Text = "Load Project";
		this.btnSaveProject.Image = TGMacro.Properties.Resources.save_16;
		this.btnSaveProject.Name = "btnSaveProject";
		this.btnSaveProject.Size = new System.Drawing.Size(158, 26);
		this.btnSaveProject.Text = "Save Project";
		this.btnSaveProjectAs.Image = TGMacro.Properties.Resources.save_16;
		this.btnSaveProjectAs.Name = "btnSaveProjectAs";
		this.btnSaveProjectAs.Size = new System.Drawing.Size(158, 26);
		this.btnSaveProjectAs.Text = "Save Project As";
		this.btnImportProject.Image = TGMacro.Properties.Resources.import_16;
		this.btnImportProject.Name = "btnImportProject";
		this.btnImportProject.Size = new System.Drawing.Size(158, 26);
		this.btnImportProject.Text = "Import Project";
		this.btnImportScript.Image = TGMacro.Properties.Resources.import_sc_16;
		this.btnImportScript.Name = "btnImportScript";
		this.btnImportScript.Size = new System.Drawing.Size(158, 26);
		this.btnImportScript.Text = "Import Script";
		this.btnClear.Image = TGMacro.Properties.Resources.clear_16;
		this.btnClear.Name = "btnClear";
		this.btnClear.Size = new System.Drawing.Size(158, 26);
		this.btnClear.Text = "Clear Screen";
		this.pnlContent.BackColor = System.Drawing.Color.Transparent;
		this.pnlContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnlContent.Controls.Add(this.pnlMain);
		this.pnlContent.Controls.Add(this.pnlTop);
		this.pnlContent.Controls.Add(this.lblLoading);
		this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnlContent.Location = new System.Drawing.Point(0, 0);
		this.pnlContent.Name = "pnlContent";
		this.pnlContent.Size = new System.Drawing.Size(464, 574);
		this.pnlContent.TabIndex = 7;
		this.lblLoading.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblLoading.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblLoading.Font = new System.Drawing.Font("Century Gothic", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 162);
		this.lblLoading.Location = new System.Drawing.Point(0, 0);
		this.lblLoading.Name = "lblLoading";
		this.lblLoading.Size = new System.Drawing.Size(462, 572);
		this.lblLoading.TabIndex = 7;
		this.lblLoading.Text = "Loading..";
		this.lblLoading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.tmrUpdate.Interval = 5000;
		this.tmrUpdate.Tick += new System.EventHandler(tmrUpdate_Tick);
		this.notifyAppIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
		this.notifyAppIcon.Icon = (System.Drawing.Icon)resources.GetObject("notifyAppIcon.Icon");
		this.notifyAppIcon.Text = "TGMacro";
		this.notifyAppIcon.Visible = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 17f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(464, 574);
		base.Controls.Add(this.pnlContent);
		this.Font = new System.Drawing.Font("Century Gothic", 9f);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		base.MaximizeBox = false;
		base.Name = "frmMain";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "TGM";
		this.pnlMain.ResumeLayout(false);
		this.pnlTop.ResumeLayout(false);
		this.pnlTop.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnSettings).EndInit();
		((System.ComponentModel.ISupportInitialize)this.btnHelpMainMenu).EndInit();
		((System.ComponentModel.ISupportInitialize)this.btnMin).EndInit();
		((System.ComponentModel.ISupportInitialize)this.btnClose).EndInit();
		((System.ComponentModel.ISupportInitialize)this.btnStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.btnTopMost).EndInit();
		this.ctHelpMenu.ResumeLayout(false);
		this.ctFileMenu.ResumeLayout(false);
		this.pnlContent.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
