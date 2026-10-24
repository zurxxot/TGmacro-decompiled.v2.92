using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CSInputs.Enums;
using TGMacro.Actions;
using TGMacro.Enums;
using TGMacro.Interfaces;

namespace TGMacro.Statics;

public static class ScriptParser
{
	private static readonly OpenFileDialog openFileDialog1 = new OpenFileDialog();

	public static void LoadScriptTo(DataTable actionsTable)
	{
		openFileDialog1.Filter = "AMC Script |*.amc";
		if (openFileDialog1.ShowDialog() == DialogResult.OK)
		{
			FileInfo fileInfo = new FileInfo(openFileDialog1.FileName);
			if (fileInfo.Extension.ToLower() == ".amc")
			{
				importAMC(actionsTable, openFileDialog1.FileName);
			}
			else
			{
				Messenger.Show(AppSession.ActiveLanguage.error_unkown_script, "Error");
			}
		}
	}

	private static void importAMC(DataTable actionsTalbe, string filePath)
	{
		frmTimeAdjuster frmTimeAdjuster = new frmTimeAdjuster();
		if (frmTimeAdjuster.ShowDialog() == DialogResult.OK)
		{
			IMacroAction[] array = amcToActions(filePath, frmTimeAdjuster.SleepAdjustValue);
			foreach (IMacroAction macroAction in array)
			{
				actionsTalbe.Rows.Add(macroAction, macroAction.Icon, macroAction.VisibleText, macroAction.VisibleValue);
			}
		}
	}

	private static IMacroAction[] amcToActions(string filePath, int waitAdjustAmount = 0)
	{
		List<IMacroAction> list = new List<IMacroAction>();
		try
		{
			string input = File.ReadAllText(filePath);
			input = Regex.Replace(input, "<(.*?)>", "");
			string[] array = (from t in input.ToLower().Split('\n')
				where t.Trim().Length > 3
				select t).ToArray();
			string[] array2 = array;
			foreach (string text in array2)
			{
				string[] array3 = text.Trim().Split(' ');
				if (array3[0] == "mover" && array3.Length == 3)
				{
					list.Add(new MouseMoveAction
					{
						MouseMoveMethod = MousePositioning.Relative,
						MousePos = new Point(int.Parse(array3[1]), int.Parse(array3[2]))
					});
				}
				if (array3[0] == "leftdown" && array3.Length == 2)
				{
					list.Add(new MouseKeyAction
					{
						KeyMethod = KeyPressMethods.Down,
						MouseKey = CSInputs.Enums.MouseKeys.MouseLeft,
						MouseMoveMethod = MousePositioning.Relative
					});
				}
				if (array3[0] == "leftup" && array3.Length == 2)
				{
					list.Add(new MouseKeyAction
					{
						KeyMethod = KeyPressMethods.Up,
						MouseKey = CSInputs.Enums.MouseKeys.MouseLeft,
						MouseMoveMethod = MousePositioning.Relative
					});
				}
				if (array3[0] == "delay" && array3.Length == 3)
				{
					list.Add(new WaitAction
					{
						SleepAmount = int.Parse(array3[1]) + waitAdjustAmount
					});
				}
			}
		}
		catch
		{
			Messenger.Show(AppSession.ActiveLanguage.error_import_script, "Error");
		}
		return list.ToArray();
	}
}
