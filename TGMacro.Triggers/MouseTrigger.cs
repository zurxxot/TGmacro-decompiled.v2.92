using System;
using CSInputs.Enums;
using CSInputs.ReadInput;
using CSInputs.Structs;
using TGMacro.Bases;
using TGMacro.Enums;
using TGMacro.Statics;

namespace TGMacro.Triggers;

public class MouseTrigger : TriggerBase, IDisposable
{
	public TriggerKeyMethods Method { get; set; }

	public bool HasPrimary => PrimaryKey != CSInputs.Enums.KeyboardKeys.None;

	public CSInputs.Enums.KeyboardKeys PrimaryKey { get; set; }

	public CSInputs.Enums.MouseKeys SecondaryKey { get; set; }

	public MouseTrigger()
	{
		TriggerType = TriggerTypes.Mouse;
		AppSession.InputListener.MouseInputs += Hook_MouseInputs;
	}

	private void Hook_MouseInputs(MouseData data, ref ModifierKey modifierKey)
	{
		if (!AppSession.Enable || !base.IsEnable)
		{
			base.RunState = TriggerRunState.Stop;
			return;
		}
		bool flag = (HasPrimary && SharedMethods.getPrimaryKeyStatus(PrimaryKey, modifierKey)) || !HasPrimary;
		bool flag2 = data.Key == SecondaryKey;
		if (flag && flag2 && Method == TriggerKeyMethods.Press && data.Flags == KeyFlags.Down)
		{
			base.RunState = TriggerRunState.RunOnce;
		}
		if (flag && flag2 && Method == TriggerKeyMethods.Release && data.Flags == KeyFlags.Up)
		{
			base.RunState = TriggerRunState.RunOnce;
		}
		if (Method == TriggerKeyMethods.Hold)
		{
			if (data.Flags == KeyFlags.Down && flag && flag2)
			{
				base.RunState = TriggerRunState.Run;
			}
			else if (flag2 && data.Flags == KeyFlags.Up)
			{
				base.RunState = TriggerRunState.Stop;
			}
		}
		if (flag && flag2 && Method == TriggerKeyMethods.Toggle && data.Flags == KeyFlags.Down)
		{
			if (base.RunState == TriggerRunState.Run)
			{
				base.RunState = TriggerRunState.Stop;
			}
			else
			{
				base.RunState = TriggerRunState.Run;
			}
		}
	}

	public override string ToString()
	{
		return "Mouse";
	}
}
