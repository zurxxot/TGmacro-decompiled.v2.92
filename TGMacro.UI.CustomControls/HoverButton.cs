using System;
using System.Drawing;
using System.Windows.Forms;
using TGMacro.Statics;

namespace TGMacro.UI.CustomControls;

public class HoverButton : Button
{
	private Color PrevColor;

	public Color? HoverColor { get; set; } = null;

	public HoverButton()
	{
		base.FlatStyle = FlatStyle.Flat;
		base.FlatAppearance.MouseDownBackColor = AppSession.Settings.ColorPalette.MidColor;
		base.FlatAppearance.MouseOverBackColor = AppSession.Settings.ColorPalette.LightColor;
		SetStyle(ControlStyles.Selectable, value: false);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		_ = PrevColor;
		if (true)
		{
			PrevColor = BackColor;
		}
		if (HoverColor.HasValue)
		{
			BackColor = HoverColor.Value;
		}
		else
		{
			BackColor = AppSession.Settings.ColorPalette.LightColor;
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		BackColor = PrevColor;
		base.OnMouseLeave(e);
	}
}
