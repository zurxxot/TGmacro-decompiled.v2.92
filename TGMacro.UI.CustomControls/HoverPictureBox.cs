using System;
using System.Drawing;
using System.Windows.Forms;

namespace TGMacro.UI.CustomControls;

public class HoverPictureBox : PictureBox
{
	private Image PrevImage;

	private Image _hoverImage;

	public Image HoverImage
	{
		get
		{
			return _hoverImage;
		}
		set
		{
			PrevImage = base.Image;
			_hoverImage = value;
		}
	}

	public HoverPictureBox()
	{
		base.SizeMode = PictureBoxSizeMode.Zoom;
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		base.Image = HoverImage;
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.Image = PrevImage;
	}
}
