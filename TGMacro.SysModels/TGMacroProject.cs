using System.Runtime.Serialization;
using TGMacro.Statics;

namespace TGMacro.SysModels;

[DataContract]
public class TGMacroProject
{
	[DataMember(Order = 0)]
	public string FileType { get; private set; } = "TGMacro Project";

	[DataMember(Order = 1)]
	public string Version { get; private set; } = AppSession.Version + " Build:" + AppSession.BuildNumber;

	[DataMember(Order = 2)]
	public TGMacroProjectMacro[] Macros { get; set; }
}
