using System.Windows.Forms;

namespace TGMacro.UI.CustomControls;

public class DoubleBufferForm : Form
{
	public DoubleBufferForm()
	{
		DoubleBuffered = true;
	}
}
