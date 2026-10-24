using System;
using System.Drawing;
using TGMacro.Enums;

namespace TGMacro.Interfaces;

public interface IMacroAction : ICloneable
{
	ActionTypes ActionType { get; }

	Image Icon { get; }

	string VisibleText { get; }

	string VisibleValue { get; }

	void DoJob();
}
