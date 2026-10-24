using System;
using CSInputs.Enums;

namespace TGMacro.Statics;

public class KeyboardKeys
{
	public string Name { get; set; }

	public CSInputs.Enums.KeyboardKeys Key { get; set; }

	public override string ToString()
	{
		return Name;
	}

	public KeyboardKeys(CSInputs.Enums.KeyboardKeys key)
	{
		Name = Enum.GetName(key.GetType(), key);
	}
}
