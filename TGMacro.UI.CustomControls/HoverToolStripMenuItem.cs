using System;
using System.Drawing;
using System.Windows.Forms;
using TGMacro.Statics;

namespace TGMacro.UI.CustomControls;

public class HoverToolStripMenuItem : ToolStripMenuItem
{
	private Color PrevColor;

	public Color HoverColor { get; set; } = AppSession.Settings.ColorPalette.LightColor;

	protected override void OnMouseEnter(EventArgs e)
	{
		_ = PrevColor;
		if (true)
		{
			PrevColor = BackColor;
		}
		BackColor = HoverColor;
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		BackColor = PrevColor;
		base.OnMouseLeave(e);
	}
}
