using System.Runtime.Serialization;

namespace TGMacro.SysModels;

[DataContract]
public class TGMacroProjectMacro
{
	[DataMember(Order = 0)]
	public string Name { get; set; }

	[DataMember(Order = 1)]
	public TGMacroProjectTrigger Trigger { get; set; }

	[DataMember(Order = 2)]
	public TGMacroProjectAction[] Actions { get; set; }

	public TGMacroProjectMacro(string name)
	{
		Name = name;
	}
}
