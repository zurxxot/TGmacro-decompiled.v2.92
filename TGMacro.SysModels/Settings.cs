using System.Drawing;
using System.Linq;
using TGMacro.Bases;
using TGMacro.Statics;

namespace TGMacro.SysModels;

public class Settings
{
	public string Language { get; set; } = "english";

	public ColorPalette ColorPalette { get; set; } = new ColorPalette
	{
		LighterColor = Color.FromArgb(35, 35, 35),
		LightColor = Color.FromArgb(20, 20, 20),
		MidColor = Color.FromArgb(15, 15, 15),
		DarkColor = Color.FromArgb(0, 0, 0),
		TextColor = Color.White
	};

	public bool MoveToTrayWhenMinimized { get; set; } = false;

	public bool EnableOnStart { get; set; } = false;

	public bool LoadLastMacroOnLoad { get; set; } = false;

	public string LastFile { get; set; }

	public int ActivationKeyModifier { get; set; } = 0;

	public int ActivationKey { get; set; } = 36;

	public int RecordKeyModifier { get; set; } = 17;

	public int RecordKey { get; set; } = 121;

	public bool RecordKeyboardKeys { get; set; } = true;

	public bool RecordMouseKeys { get; set; } = true;

	public bool RecordMouseMovements { get; set; } = true;

	public bool RecordDelays { get; set; } = true;

	public void UpdateLanguage()
	{
		AppSession.ActiveLanguage = AppSession.Languages.Where((LanguageBase t) => t.language_name.ToLower().Contains(Language.ToLower())).FirstOrDefault();
	}
}
