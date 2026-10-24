using System;
using System.Collections.Generic;
using CSInputs.Enums;

namespace TGMacro.Statics;

public static class UIData
{
	private static MouseKeys[] getMouseKeys;

	private static KeyboardKeys[] getKeyboardKeys;

	private static PrimaryKeys[] getPrimaryKeys;

	public static MouseKeys[] GetMouseKeys
	{
		get
		{
			if (getMouseKeys == null)
			{
				List<MouseKeys> list = new List<MouseKeys>();
				CSInputs.Enums.MouseKeys[] array = (CSInputs.Enums.MouseKeys[])Enum.GetValues(typeof(CSInputs.Enums.MouseKeys));
				for (int i = 0; i < array.Length; i++)
				{
					CSInputs.Enums.MouseKeys key = array[i];
					if (!key.ToString().ToLower().Contains("up"))
					{
						list.Add(new MouseKeys(key));
					}
				}
				getMouseKeys = list.ToArray();
			}
			return getMouseKeys;
		}
	}

	public static KeyboardKeys[] GetKeyboardKeys
	{
		get
		{
			if (getMouseKeys == null)
			{
				List<KeyboardKeys> list = new List<KeyboardKeys>();
				CSInputs.Enums.KeyboardKeys[] array = (CSInputs.Enums.KeyboardKeys[])Enum.GetValues(typeof(CSInputs.Enums.KeyboardKeys));
				foreach (CSInputs.Enums.KeyboardKeys key in array)
				{
					list.Add(new KeyboardKeys(key));
				}
				getKeyboardKeys = list.ToArray();
			}
			return getKeyboardKeys;
		}
	}

	public static PrimaryKeys[] GetPrimaryKeys
	{
		get
		{
			if (getMouseKeys == null)
			{
				List<PrimaryKeys> list = new List<PrimaryKeys>();
				CSInputs.Enums.KeyboardKeys[] array = (CSInputs.Enums.KeyboardKeys[])Enum.GetValues(typeof(CSInputs.Enums.KeyboardKeys));
				foreach (CSInputs.Enums.KeyboardKeys key in array)
				{
					list.Add(new PrimaryKeys(key));
				}
				getPrimaryKeys = list.ToArray();
			}
			return getPrimaryKeys;
		}
	}
}
