using System;
using CSInputs.Enums;

namespace TGMacro.Statics;

public class PrimaryKeys
{
	public string Name { get; set; }

	public CSInputs.Enums.KeyboardKeys Key { get; set; }

	public override string ToString()
	{
		return Name;
	}

	public PrimaryKeys(CSInputs.Enums.KeyboardKeys key)
	{
		Name = Enum.GetName(key.GetType(), key);
	}
}
