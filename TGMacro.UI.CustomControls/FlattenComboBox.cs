using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TGMacro.Statics;

namespace TGMacro.UI.CustomControls;

public class FlattenComboBox : ComboBox
{
	private Brush BorderBrush = new SolidBrush(SystemColors.WindowFrame);

	private Brush ArrowBrush = new SolidBrush(SystemColors.ControlText);

	private Brush DropButtonBrush = new SolidBrush(SystemColors.Control);

	private Color _borderColor = AppSession.Settings.ColorPalette.LighterColor;

	private ButtonBorderStyle _borderStyle = ButtonBorderStyle.Solid;

	public override Color BackColor => AppSession.Settings.ColorPalette.LightColor;

	public override Color ForeColor => AppSession.Settings.ColorPalette.TextColor;

	[Category("Appearance")]
	public ButtonBorderStyle BorderStyle
	{
		get
		{
			return _borderStyle;
		}
		set
		{
			_borderStyle = value;
			Invalidate();
		}
	}

	public FlattenComboBox()
	{
		base.DrawMode = DrawMode.OwnerDrawFixed;
		BorderStyle = ButtonBorderStyle.Solid;
		DropButtonBrush = new SolidBrush(AppSession.Settings.ColorPalette.LighterColor);
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
		{
			e.Graphics.FillRectangle(new SolidBrush(AppSession.Settings.ColorPalette.LightColor), e.Bounds);
		}
		else
		{
			e.Graphics.FillRectangle(new SolidBrush(AppSession.Settings.ColorPalette.DarkColor), e.Bounds);
		}
		if (e.Index > -1)
		{
			e.Graphics.DrawString(base.Items[e.Index].ToString(), e.Font, new SolidBrush(AppSession.Settings.ColorPalette.TextColor), new Point(e.Bounds.X, e.Bounds.Y));
		}
	}

	protected override void WndProc(ref Message m)
	{
		base.WndProc(ref m);
		int msg = m.Msg;
		int num = msg;
		if (num == 15)
		{
			Graphics graphics = CreateGraphics();
			Rectangle bounds = new Rectangle(0, 0, base.Width, base.Height);
			ControlPaint.DrawBorder(graphics, bounds, _borderColor, _borderStyle);
			graphics.FillRectangle(rect: new Rectangle(base.Width - 18, 0, 18, base.Height), brush: DropButtonBrush);
			GraphicsPath graphicsPath = new GraphicsPath();
			PointF pt = new PointF(base.Width - 13, (base.Height - 5) / 2);
			PointF pointF = new PointF(base.Width - 6, (base.Height - 5) / 2);
			PointF pt2 = new PointF(base.Width - 9, (base.Height + 2) / 2);
			graphicsPath.AddLine(pt, pointF);
			graphicsPath.AddLine(pointF, pt2);
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			if (base.DroppedDown)
			{
				ArrowBrush = new SolidBrush(SystemColors.HighlightText);
			}
			else
			{
				ArrowBrush = new SolidBrush(SystemColors.ControlText);
			}
			graphics.FillPath(ArrowBrush, graphicsPath);
			graphics.Dispose();
		}
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		base.OnMouseEnter(e);
		Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);
		BorderBrush = new SolidBrush(SystemColors.WindowFrame);
		Invalidate();
	}

	protected override void OnLostFocus(EventArgs e)
	{
		base.OnLostFocus(e);
		Invalidate();
	}

	protected override void OnGotFocus(EventArgs e)
	{
		base.OnGotFocus(e);
		BorderBrush = new SolidBrush(SystemColors.WindowFrame);
		Invalidate();
	}

	protected override void OnMouseHover(EventArgs e)
	{
		base.OnMouseHover(e);
		BorderBrush = new SolidBrush(SystemColors.WindowFrame);
		Invalidate();
	}
}
