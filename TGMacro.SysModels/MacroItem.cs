using System;
using System.Drawing;
using TGMacro.Bases;
using TGMacro.Enums;
using TGMacro.Properties;

namespace TGMacro.SysModels;

public class MacroItem : IDisposable
{
	public TriggerBase Trigger;

	public string Name { get; set; }

	public bool Enable
	{
		get
		{
			return Trigger != null && Trigger.IsEnable;
		}
		set
		{
			if (Trigger != null)
			{
				Trigger.IsEnable = value;
			}
		}
	}

	public Image Icon
	{
		get
		{
			if (Trigger != null)
			{
				return Trigger.TriggerType switch
				{
					TriggerTypes.Mouse => Resources.mouse_16px, 
					TriggerTypes.Keyboard => Resources.keyboard_16px, 
					TriggerTypes.Pixel => Resources.color_16, 
					_ => Resources.keyboard_16px, 
				};
			}
			return Resources.keyboard_16px;
		}
	}

	public void Dispose()
	{
		if (Trigger != null)
		{
			Trigger.Stop();
		}
	}
}
