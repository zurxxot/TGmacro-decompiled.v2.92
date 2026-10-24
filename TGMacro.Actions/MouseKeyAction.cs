using System;
using System.Drawing;
using CSInputs.Enums;
using CSInputs.SendInput;
using TGMacro.Enums;
using TGMacro.Interfaces;
using TGMacro.Properties;

namespace TGMacro.Actions;

public class MouseKeyAction : IMacroAction, ICloneable
{
	public short ScrollAmount = 60;

	public ActionTypes ActionType => ActionTypes.MouseButton;

	public KeyPressMethods KeyMethod { get; set; } = KeyPressMethods.Press;

	public MouseKeys MouseKey { get; set; } = MouseKeys.MouseLeft;

	public MousePositioning MouseMoveMethod { get; set; } = MousePositioning.Absolute;

	public Point MousePos { get; set; }

	public string VisibleText => "Mouse " + ((KeyMethod == KeyPressMethods.Press) ? "Click" : KeyMethod.ToString()) + $" ({MouseMoveMethod.ToString()[0]})";

	public string VisibleValue => MouseKey.ToString() + "  " + ((MousePos != Point.Empty) ? MousePos.ToString() : "");

	public Image Icon => Resources.mouse_16px;

	public object Clone()
	{
		return MemberwiseClone();
	}

	public void DoJob()
	{
		Point mousePos = MousePos;
		if (KeyMethod == KeyPressMethods.Press)
		{
			Mouse.Send(MouseKey, mousePos, MouseMoveMethod, ScrollAmount);
		}
		else if (KeyMethod == KeyPressMethods.Down)
		{
			Mouse.Send(MouseKey, KeyFlags.Down, mousePos, MouseMoveMethod, ScrollAmount);
		}
		else
		{
			Mouse.Send(MouseKey, KeyFlags.Up, mousePos, MouseMoveMethod, ScrollAmount);
		}
	}
}
