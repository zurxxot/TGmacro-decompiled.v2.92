using System.Drawing;
using System.Runtime.Serialization;
using CSInputs.Enums;
using TGMacro.Bases;
using TGMacro.Enums;
using TGMacro.Triggers;

namespace TGMacro.SysModels;

[DataContract]
public class TGMacroProjectTrigger
{
	[DataMember(EmitDefaultValue = true)]
	public TriggerTypes TriggerType { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public KeyboardKeys? MasterKey { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public KeyboardKeys? KeyboardKey { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public MouseKeys? MouseKey { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public Point? PixelPosition { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public RGB? PixelColor { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public ColorTriggerMethods? IfMatches { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public TriggerKeyMethods? KeyPressMethod { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public bool? TriggerOnce { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public int? ColorTolerance { get; set; }

	public TGMacroProjectTrigger()
	{
	}

	public TGMacroProjectTrigger(TriggerBase trg)
	{
		switch (trg.TriggerType)
		{
		case TriggerTypes.Mouse:
			TriggerType = TriggerTypes.Mouse;
			MasterKey = (trg as MouseTrigger).PrimaryKey;
			KeyPressMethod = (trg as MouseTrigger).Method;
			MouseKey = (trg as MouseTrigger).SecondaryKey;
			break;
		case TriggerTypes.Keyboard:
			TriggerType = TriggerTypes.Keyboard;
			MasterKey = (trg as KeyboardTrigger).PrimaryKey;
			KeyPressMethod = (trg as KeyboardTrigger).Method;
			KeyboardKey = (trg as KeyboardTrigger).SecondaryKey;
			break;
		case TriggerTypes.Pixel:
			TriggerType = TriggerTypes.Pixel;
			PixelPosition = (trg as ColorTrigger).Pos;
			PixelColor = new RGB((trg as ColorTrigger).Color.R, (trg as ColorTrigger).Color.G, (trg as ColorTrigger).Color.B);
			IfMatches = (trg as ColorTrigger).Method;
			ColorTolerance = (trg as ColorTrigger).ColorTolerance;
			TriggerOnce = (trg as ColorTrigger).TriggerOnce;
			break;
		}
	}

	public TriggerBase GetTrigger()
	{
		return TriggerType switch
		{
			TriggerTypes.Mouse => new MouseTrigger
			{
				Method = KeyPressMethod.GetValueOrDefault(),
				PrimaryKey = MasterKey.GetValueOrDefault(),
				SecondaryKey = MouseKey.GetValueOrDefault()
			}, 
			TriggerTypes.Keyboard => new KeyboardTrigger
			{
				Method = KeyPressMethod.GetValueOrDefault(),
				PrimaryKey = MasterKey.GetValueOrDefault(),
				SecondaryKey = KeyboardKey.GetValueOrDefault()
			}, 
			TriggerTypes.Pixel => new ColorTrigger
			{
				Method = IfMatches.GetValueOrDefault(),
				Color = Color.FromArgb(PixelColor.Value.R, PixelColor.Value.G, PixelColor.Value.B),
				Pos = PixelPosition.GetValueOrDefault(),
				ColorTolerance = ((ColorTolerance > 10) ? 10 : (((ColorTolerance < 1) ? new int?(1) : ColorTolerance) ?? 10)),
				TriggerOnce = (TriggerOnce == true)
			}, 
			_ => new EmptyTrigger(), 
		};
	}
}
