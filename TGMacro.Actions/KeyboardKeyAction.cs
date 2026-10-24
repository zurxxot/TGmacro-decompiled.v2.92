using System;
using System.Drawing;
using CSInputs.Enums;
using CSInputs.SendInput;
using TGMacro.Enums;
using TGMacro.Interfaces;
using TGMacro.Properties;

namespace TGMacro.Actions;

public class KeyboardKeyAction : IMacroAction, ICloneable
{
	public ActionTypes ActionType => ActionTypes.KeyboardKey;

	public KeyboardKeys KeyboardKey { get; set; } = KeyboardKeys.Space;

	public KeyPressMethods KeyMethod { get; set; } = KeyPressMethods.Press;

	public string VisibleText => "Key " + KeyMethod;

	public string VisibleValue => KeyboardKey.ToString();

	public Image Icon => Resources.keyboard_16px;

	public object Clone()
	{
		return MemberwiseClone();
	}

	public void DoJob()
	{
		if (KeyMethod == KeyPressMethods.Press)
		{
			Keyboard.Send(KeyboardKey);
		}
		else if (KeyMethod == KeyPressMethods.Down)
		{
			Keyboard.Send(KeyboardKey, KeyFlags.Down);
		}
		else
		{
			Keyboard.Send(KeyboardKey, KeyFlags.Up);
		}
	}
}
