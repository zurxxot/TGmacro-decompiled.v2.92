using System;
using System.Drawing;
using CSInputs.Enums;
using CSInputs.SendInput;
using TGMacro.Enums;
using TGMacro.Interfaces;
using TGMacro.Properties;

namespace TGMacro.Actions;

public class MouseMoveAction : IMacroAction, ICloneable
{
	public ActionTypes ActionType => ActionTypes.MoveMouse;

	public MousePositioning MouseMoveMethod { get; set; } = MousePositioning.Absolute;

	public Point MousePos { get; set; }

	public string VisibleText => $"Move Mouse ({MouseMoveMethod.ToString()[0]})";

	public string VisibleValue => MousePos.ToString();

	public Image Icon => Resources.mouse_16px;

	public object Clone()
	{
		return MemberwiseClone();
	}

	public void DoJob()
	{
		Mouse.MoveTo(MousePos, MouseMoveMethod);
	}
}
