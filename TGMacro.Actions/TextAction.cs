using System;
using System.Drawing;
using CSInputs.SendInput;
using TGMacro.Enums;
using TGMacro.Interfaces;
using TGMacro.Properties;

namespace TGMacro.Actions;

public class TextAction : IMacroAction, ICloneable
{
	public ActionTypes ActionType => ActionTypes.Text;

	public string Text { get; set; }

	public string VisibleText => "Text";

	public string VisibleValue => Text;

	public Image Icon => Resources.keyboard_16px;

	public object Clone()
	{
		return MemberwiseClone();
	}

	public void DoJob()
	{
		Keyboard.SendString(Text);
	}
}
