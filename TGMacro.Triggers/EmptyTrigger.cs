using TGMacro.Bases;
using TGMacro.Enums;

namespace TGMacro.Triggers;

public class EmptyTrigger : TriggerBase
{
	public TriggerKeyMethods Method { get; set; } = TriggerKeyMethods.Hold;

	public EmptyTrigger()
	{
		TriggerType = TriggerTypes.Keyboard;
	}

	public override string ToString()
	{
		return "None";
	}
}
