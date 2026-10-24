using System;
using System.Drawing;

namespace TGMacro.SysModels;

[Serializable]
public class ColorModel
{
	public int R { get; set; }

	public int G { get; set; }

	public int B { get; set; }

	public static implicit operator Color(ColorModel c)
	{
		return Color.FromArgb(c.R, c.G, c.B);
	}

	public static implicit operator ColorModel(Color c)
	{
		return new ColorModel
		{
			R = c.R,
			B = c.B,
			G = c.B
		};
	}
}
