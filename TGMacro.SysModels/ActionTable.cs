using System.Data;
using TGMacro.Interfaces;

namespace TGMacro.SysModels;

public class ActionTable : DataTable
{
	public delegate void OnDataChanged();

	public enum TableRowDirection
	{
		Up,
		Down
	}

	private bool lineNumberUpdateEnable = true;

	public bool LineNumberUpdateEnabled
	{
		get
		{
			return lineNumberUpdateEnable;
		}
		set
		{
			lineNumberUpdateEnable = value;
			if (value)
			{
				base.RowDeleted += ActionTable_RowChanged;
				base.RowChanged += ActionTable_RowChanged;
				UpdateLineNumbers();
			}
			else
			{
				base.RowDeleted -= ActionTable_RowChanged;
				base.RowChanged -= ActionTable_RowChanged;
			}
		}
	}

	public event OnDataChanged DataChanged;

	public ActionTable()
	{
		base.RowChanged += ActionTable_RowChanged;
		base.RowDeleted += ActionTable_RowChanged;
	}

	private void ActionTable_RowChanged(object sender, DataRowChangeEventArgs e)
	{
		if (e.Action == DataRowAction.Delete || e.Action == DataRowAction.Add)
		{
			UpdateLineNumbers();
		}
	}

	public int FindIndexOf(IMacroAction action)
	{
		for (int i = 0; i < base.Rows.Count; i++)
		{
			if (action == base.Rows[i][0])
			{
				return i;
			}
		}
		return -1;
	}

	private void UpdateLineNumbers()
	{
		for (int i = 0; i < base.Rows.Count; i++)
		{
			base.Rows[i][1] = i + 1;
		}
		this.DataChanged?.Invoke();
	}

	public int MoveRow(DataRow row, TableRowDirection direction)
	{
		DataRow dataRow = NewRow();
		dataRow.ItemArray = row.ItemArray;
		int num = base.Rows.IndexOf(row);
		if (direction == TableRowDirection.Down)
		{
			int pos = num + 1;
			if (num < base.Rows.Count)
			{
				base.Rows.Remove(row);
				base.Rows.InsertAt(dataRow, pos);
				return base.Rows.IndexOf(dataRow);
			}
		}
		if (direction == TableRowDirection.Up)
		{
			int pos2 = num - 1;
			if (num > 0)
			{
				base.Rows.Remove(row);
				base.Rows.InsertAt(dataRow, pos2);
				return base.Rows.IndexOf(dataRow);
			}
		}
		return 0;
	}
}
