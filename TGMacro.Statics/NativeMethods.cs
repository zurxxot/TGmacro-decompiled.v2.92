using System;
using System.Runtime.InteropServices;

namespace TGMacro.Statics;

public static class NativeMethods
{
	[DllImport("user32.dll")]
	public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

	[DllImport("user32.dll")]
	public static extern bool ReleaseCapture();

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

	[DllImport("dnsapi.dll")]
	public static extern uint DnsFlushResolverCache();
}
