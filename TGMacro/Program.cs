using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using TGMacro.Bases;
using TGMacro.Langs;
using TGMacro.Statics;

namespace TGMacro;

internal static class Program
{
	[STAThread]
	[Obsolete]
	private static void Main(string[] args)
	{
		Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
		AppDomain.CurrentDomain.UnhandledException += GlobalUnhandledException;
		Application.ThreadException += ThreadExceptions;
		Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
		AppDomain.CurrentDomain.AppendPrivatePath("Libs");
		new Mutex(initiallyOwned: true, "trksyln-TMACRO", out var createdNew);
		if (!createdNew)
		{
			MessageBox.Show("TGM Already running.", "TGM - Error");
			return;
		}
		if (!File.Exists("Libs\\CSInputs.dll"))
		{
			if (MessageBox.Show("Some important files are missing. Re-installing might fix the issue. Would like to download now?", "Opps!", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				Process.Start("https://trksyln.net/projects/TGM-Gaming-Macro-4/#download");
			}
			return;
		}
		string text = null;
		if (args.Length != 0)
		{
			FileInfo fileInfo = new FileInfo(args[0]);
			if (fileInfo.Exists && fileInfo.Extension.ToLower() == ".tmacroproj")
			{
				text = args[0];
			}
		}
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		AppSession.Languages.Add(new TUR());
		AppSession.Languages.Add(new ENG());
		ImportLanguages();
		AppSession.Settings = SharedMethods.LoadSettings();
		AppSession.Settings.UpdateLanguage();
		if (AppSession.Settings.LoadLastMacroOnLoad && string.IsNullOrWhiteSpace(text) && File.Exists(AppSession.Settings.LastFile))
		{
			text = AppSession.Settings.LastFile;
		}
		Application.Run(new frmMain(text));
	}

	private static void ThreadExceptions(object sender, ThreadExceptionEventArgs e)
	{
		Messenger.Show("Some error occurd TGMacro can not start.", "Error");
		Application.Exit();
	}

	private static void GlobalUnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		Messenger.Show("Some error occurd:\r\n" + (e.ExceptionObject as Exception).Message, "Error");
	}

	private static void ImportLanguages()
	{
		if (!Directory.Exists("Langs"))
		{
			return;
		}
		string[] files = Directory.GetFiles("Langs\\", "*.tglang");
		if (files.Count() < 1)
		{
			return;
		}
		JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
		for (int i = 0; i < files.Length; i++)
		{
			try
			{
				string input = File.ReadAllText(files[i]);
				LanguageBase newLang = javaScriptSerializer.Deserialize<LanguageBase>(input);
				if (AppSession.Languages.Where((LanguageBase t) => t.language_name.ToUpper() == newLang.language_name.ToUpper()).Count() <= 0)
				{
					AppSession.Languages.Add(newLang);
				}
			}
			catch
			{
				Messenger.Show(files[i] + " couldn't be added. Invalid file.", "Invalid language file");
			}
		}
	}
}
