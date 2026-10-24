using System.Windows.Forms;

namespace TGMacro.Statics;

public static class Messenger
{
	public static DialogResult Show(string message, string title, string yesButtonText = "", string noButtonText = "", string cancelButtonText = "", int specialCode = 0)
	{
		return new frmMessage(message, title, yesButtonText, noButtonText, cancelButtonText, specialCode).ShowDialog();
	}
}
