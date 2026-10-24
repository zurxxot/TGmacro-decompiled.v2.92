using System.Windows.Forms;

namespace TGMacro.UI.CustomControls;

public class DatagridDoubleBuffer : DataGridView
{
	public DatagridDoubleBuffer()
	{
		DoubleBuffered = true;
	}

	protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
	{
		base.OnColumnAdded(e);
		e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
	}
}
