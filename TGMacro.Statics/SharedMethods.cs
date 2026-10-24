using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using CSInputs.Enums;
using CSInputs.ReadInput;
using TGMacro.SysModels;

namespace TGMacro.Statics;

public static class SharedMethods
{
	private static JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

	public static async Task<string> GetTextFromServer(string address)
	{
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
		string result = string.Empty;
		try
		{
			using (HttpClient client = new HttpClient())
			{
				result = await client.GetStringAsync(address);
			}
			return result;
		}
		catch
		{
			return result;
		}
	}

	public static void DragWindow(IntPtr handle)
	{
		NativeMethods.ReleaseCapture();
		NativeMethods.SendMessage(handle, 161, 2, 0);
	}

	public static CSInputs.Enums.KeyboardKeys ModifierKeyToKey(ModifierKey modifier)
	{
		if (modifier.Control)
		{
			return CSInputs.Enums.KeyboardKeys.Ctrl;
		}
		if (modifier.LShift)
		{
			return CSInputs.Enums.KeyboardKeys.LeftShift;
		}
		if (modifier.RShift)
		{
			return CSInputs.Enums.KeyboardKeys.RightShift;
		}
		if (modifier.Alt)
		{
			return CSInputs.Enums.KeyboardKeys.Alt;
		}
		return CSInputs.Enums.KeyboardKeys.None;
	}

	public static Color GetColorAt(Point location)
	{
		Bitmap bitmap = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
		try
		{
			using Graphics graphics = Graphics.FromImage(bitmap);
			using Graphics graphics2 = Graphics.FromHwnd(IntPtr.Zero);
			IntPtr hdc = graphics2.GetHdc();
			IntPtr hdc2 = graphics.GetHdc();
			int num = NativeMethods.BitBlt(hdc2, 0, 0, 1, 1, hdc, location.X, location.Y, 13369376);
			graphics.ReleaseHdc();
			graphics2.ReleaseHdc();
		}
		catch
		{
		}
		return bitmap.GetPixel(0, 0);
	}

	public static bool getPrimaryKeyStatus(CSInputs.Enums.KeyboardKeys key, ModifierKey modifierKey)
	{
		return key switch
		{
			CSInputs.Enums.KeyboardKeys.Ctrl => modifierKey.Control, 
			CSInputs.Enums.KeyboardKeys.Alt => modifierKey.Alt, 
			CSInputs.Enums.KeyboardKeys.LeftShift => modifierKey.LShift, 
			CSInputs.Enums.KeyboardKeys.RightShift => modifierKey.RShift, 
			_ => false, 
		};
	}

	public static bool SaveSettings()
	{
		try
		{
			string contents = javaScriptSerializer.Serialize(AppSession.Settings);
			File.WriteAllText("settings.json", contents, Encoding.UTF8);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static Settings LoadSettings()
	{
		if (File.Exists("settings.json"))
		{
			try
			{
				return javaScriptSerializer.Deserialize<Settings>(File.ReadAllText("settings.json"));
			}
			catch (Exception)
			{
				return new Settings();
			}
		}
		Process.Start("https://trksyln.net/projects/TGM-Gaming-Macro-4/installed");
		return new Settings();
	}
}
