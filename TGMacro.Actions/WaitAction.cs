using System;
using System.Drawing;
using System.Threading;
using TGMacro.Enums;
using TGMacro.Interfaces;
using TGMacro.Properties;

namespace TGMacro.Actions;

public class WaitAction : IMacroAction, ICloneable
{
	public ActionTypes ActionType => ActionTypes.Wait;

	public int SleepAmount { get; set; }

	public string VisibleText => "Wait";

	public string VisibleValue => SleepAmount + "ms";

	public Image Icon => Resources.wait_16_green;

	public object Clone()
	{
		return MemberwiseClone();
	}

	public void DoJob()
	{
		Thread.Sleep(SleepAmount);
	}
}
