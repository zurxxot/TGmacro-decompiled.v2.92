using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using CSInputs.ReadInput;
using TGMacro.Bases;
using TGMacro.SysModels;

namespace TGMacro.Statics;

public static class AppSession
{
	public const string VersionInfoUrl = "http://trksyln.net/tgmacro/VersionInfo.json";

	public const string ShadyWebSitesUrl = "http://trksyln.net/tgmacro/ShadyWebSites.json";

	public static string LastSavePath = string.Empty;

	public static VersionInfoModel NewVersionInfo;

	public static Settings Settings = new Settings();

	private static ColorPalette SupporterColorPalette = new ColorPalette
	{
		LighterColor = Color.FromArgb(35, 35, 35),
		LightColor = Color.FromArgb(20, 20, 20),
		MidColor = Color.FromArgb(15, 15, 15),
		DarkColor = Color.FromArgb(0, 0, 0),
		TextColor = Color.White
	};

	private static ColorPalette MainColorPallette = new ColorPalette
	{
		LighterColor = Color.FromArgb(255, 255, 255),
		LightColor = Color.FromArgb(240, 240, 240),
		MidColor = Color.FromArgb(227, 227, 227),
		DarkColor = Color.FromArgb(227, 227, 227),
		TextColor = Color.Black
	};

	public static List<LanguageBase> Languages = new List<LanguageBase>();

	public static LanguageBase _activeLanguage;

	private static bool _enable = false;

	public static string Version = Assembly.GetExecutingAssembly().GetName().Version.Major + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor;

	public static float BuildNumber = Assembly.GetExecutingAssembly().GetName().Version.Revision;

	private static bool _topMost = false;

	public static InputListener InputListener;

	public static List<MacroItem> Macros = new List<MacroItem>();

	public static LanguageBase ActiveLanguage
	{
		get
		{
			return _activeLanguage;
		}
		set
		{
			_activeLanguage = value;
			AppSession.LanguageChanged?.Invoke(null, null);
		}
	}

	public static bool Enable
	{
		get
		{
			return _enable;
		}
		set
		{
			_enable = value;
			AppSession.EnableStatusChanged?.Invoke(null, null);
		}
	}

	public static bool TopMost
	{
		get
		{
			return _topMost;
		}
		set
		{
			_topMost = value;
			AppSession.TopMostStatusChanged?.Invoke(null, null);
		}
	}

	public static event EventHandler LanguageChanged;

	public static event EventHandler EnableStatusChanged;

	public static event EventHandler TopMostStatusChanged;
}
