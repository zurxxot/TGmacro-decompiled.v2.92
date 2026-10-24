using System.Windows.Forms;
using TGMacro.Statics;

namespace TGMacro.UI.CustomControls;

public class CustomToolStripRenderer : ToolStripProfessionalRenderer
{
	public CustomToolStripRenderer()
		: base(new CustomRendererColors())
	{
	}

	protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
	{
		e.TextColor = AppSession.Settings.ColorPalette.TextColor;
		base.OnRenderItemText(e);
	}
}
