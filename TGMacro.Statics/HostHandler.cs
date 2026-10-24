using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using TGMacro.SysModels;

namespace TGMacro.Statics;

internal static class HostHandler
{
	private static ShadyWebSitesModel shadyWebSiteData;

	private static readonly string hostsPath = Environment.SystemDirectory + "\\drivers\\etc\\hosts";

	private static Dictionary<string, string> GetHosts()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		try
		{
			string[] array = File.ReadAllLines(hostsPath);
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].Trim().StartsWith("#"))
				{
					array[i] = array[i].Replace('\t', ' ').Replace("  ", " ");
					string[] array2 = array[i].Split(' ');
					if (array2.Length == 2 && !dictionary.ContainsKey(array2[1].Trim()))
					{
						dictionary.Add(array2[1].Trim(), array2[0].Trim());
					}
				}
			}
		}
		catch
		{
		}
		return dictionary;
	}

	private static bool IsModifyRequired()
	{
		Dictionary<string, string> hosts = GetHosts();
		if (hosts.Count > 0)
		{
			for (int i = 0; i < shadyWebSiteData.ShadyAddresses.Length; i++)
			{
				if (!hosts.ContainsKey(shadyWebSiteData.ShadyAddresses[i].Trim()))
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	private static void ModifyHost()
	{
		Dictionary<string, string> hosts = GetHosts();
		string text = string.Empty;
		for (int i = 0; i < shadyWebSiteData.ShadyAddresses.Length; i++)
		{
			if (!hosts.ContainsKey(shadyWebSiteData.ShadyAddresses[i].Trim()))
			{
				text = text + "\r\n" + shadyWebSiteData.OfficialAddress + "\t" + shadyWebSiteData.ShadyAddresses[i];
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			File.AppendAllText(hostsPath, text);
		}
	}

	public static async void ShowWarning(bool showResultIfNoAction)
	{
		string result = await SharedMethods.GetTextFromServer("http://trksyln.net/tgmacro/ShadyWebSites.json");
		if (string.IsNullOrWhiteSpace(result))
		{
			return;
		}
		try
		{
			ShadyWebSitesModel data = new JavaScriptSerializer().Deserialize<ShadyWebSitesModel>(result);
			if (data == null)
			{
				return;
			}
			shadyWebSiteData = data;
		}
		catch
		{
			return;
		}
		if (!IsModifyRequired())
		{
			if (showResultIfNoAction)
			{
				Messenger.Show("No action required you are good to go!", "Security", "OK", "", "", 1);
			}
		}
		else if (Messenger.Show("!!! ATTENTION !!!\r\n\r\nPlease be aware that TGM can be modified by anyone.\r\nTherefore downloading TGM from unofficial sources can pose a risk to your PC's safety and security.\r\n\r\nTo ensure your safety, would you like us to redirect you to the official TGM website when you visit unofficial sources?", "Security Warning", "YES", "NO", "", 2) == DialogResult.Yes)
		{
			try
			{
				ModifyHost();
				NativeMethods.DnsFlushResolverCache();
				Messenger.Show("All set! You are good to go!.", "Notice", "", "", "", 1);
			}
			catch (Exception)
			{
				Messenger.Show("!!! ERROR !!!\r\n\r\nChanges could not be saved. Run TGMacro as administrator.\r\n\r\nRight click on TGMacro and select Run as administrator.", "Not enough privileges", "", "", "", 2);
			}
		}
	}
}
