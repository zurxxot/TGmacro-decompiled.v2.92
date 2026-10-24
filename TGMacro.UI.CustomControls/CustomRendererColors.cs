using System.Drawing;
using System.Windows.Forms;
using TGMacro.Statics;

namespace TGMacro.UI.CustomControls;

public class CustomRendererColors : ProfessionalColorTable
{
	public override Color MenuItemSelected => AppSession.Settings.ColorPalette.LightColor;

	public override Color ToolStripDropDownBackground => AppSession.Settings.ColorPalette.DarkColor;

	public override Color ImageMarginGradientBegin => AppSession.Settings.ColorPalette.DarkColor;

	public override Color ImageMarginGradientEnd => AppSession.Settings.ColorPalette.DarkColor;

	public override Color ImageMarginGradientMiddle => AppSession.Settings.ColorPalette.DarkColor;

	public override Color MenuItemBorder => Color.Transparent;
}
