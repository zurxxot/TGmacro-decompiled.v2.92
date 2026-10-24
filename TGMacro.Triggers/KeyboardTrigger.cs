using System;
using CSInputs.Enums;
using CSInputs.ReadInput;
using CSInputs.Structs;
using TGMacro.Bases;
using TGMacro.Enums;
using TGMacro.Statics;

namespace TGMacro.Triggers;

public class KeyboardTrigger : TriggerBase, IDisposable
{
	public TriggerKeyMethods Method { get; set; }

	public bool HasPrimary => PrimaryKey != CSInputs.Enums.KeyboardKeys.None;

	public CSInputs.Enums.KeyboardKeys PrimaryKey { get; set; }

	public CSInputs.Enums.KeyboardKeys SecondaryKey { get; set; }

	public KeyboardTrigger()
	{
		TriggerType = TriggerTypes.Keyboard;
		AppSession.InputListener.KeyboardInputs += Hook_KeyboardInputs;
	}

	private void Hook_KeyboardInputs(KeyboardData data, ref ModifierKey modifierKey)
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
			else if ((flag2 || PrimaryKey == data.Key) && data.Flags == KeyFlags.Up)
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
		return "Keyboard";
	}
}
