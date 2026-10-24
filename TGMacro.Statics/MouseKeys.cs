using System;
using CSInputs.Enums;

namespace TGMacro.Statics;

public class MouseKeys
{
	public string Name { get; set; }

	public CSInputs.Enums.MouseKeys Key { get; set; }

	public override string ToString()
	{
		return Name;
	}

	public MouseKeys(CSInputs.Enums.MouseKeys key)
	{
		Name = Enum.GetName(key.GetType(), key);
	}
}
