using System.Drawing;
using System.Runtime.Serialization;
using CSInputs.Enums;
using TGMacro.Actions;
using TGMacro.Enums;
using TGMacro.Interfaces;

namespace TGMacro.SysModels;

[DataContract]
public class TGMacroProjectAction
{
	[DataMember(EmitDefaultValue = true)]
	public ActionTypes ActionType { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public KeyPressMethods? KeyPressMethod { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public KeyboardKeys? KeyboardKeyCode { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public MouseKeys? MouseKeyCode { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public MousePositioning? MousePositioning { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public Point? PixelPosition { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public int? WaitFor { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public string Text { get; set; }

	public TGMacroProjectAction(IMacroAction act)
	{
		switch (act.ActionType)
		{
		case ActionTypes.MoveMouse:
			ActionType = (act as MouseMoveAction).ActionType;
			PixelPosition = (act as MouseMoveAction).MousePos;
			MousePositioning = (act as MouseMoveAction).MouseMoveMethod;
			break;
		case ActionTypes.MouseButton:
			ActionType = (act as MouseKeyAction).ActionType;
			PixelPosition = (act as MouseKeyAction).MousePos;
			MousePositioning = (act as MouseKeyAction).MouseMoveMethod;
			KeyPressMethod = (act as MouseKeyAction).KeyMethod;
			MouseKeyCode = (act as MouseKeyAction).MouseKey;
			break;
		case ActionTypes.KeyboardKey:
			ActionType = (act as KeyboardKeyAction).ActionType;
			KeyboardKeyCode = (act as KeyboardKeyAction).KeyboardKey;
			KeyPressMethod = (act as KeyboardKeyAction).KeyMethod;
			break;
		case ActionTypes.Wait:
			ActionType = (act as WaitAction).ActionType;
			WaitFor = (act as WaitAction).SleepAmount;
			break;
		case ActionTypes.Text:
			ActionType = (act as TextAction).ActionType;
			Text = (act as TextAction).Text;
			break;
		}
	}

	public IMacroAction GetAction()
	{
		return ActionType switch
		{
			ActionTypes.MoveMouse => new MouseMoveAction
			{
				MouseMoveMethod = MousePositioning.GetValueOrDefault(),
				MousePos = PixelPosition.GetValueOrDefault()
			}, 
			ActionTypes.MouseButton => new MouseKeyAction
			{
				MouseMoveMethod = MousePositioning.GetValueOrDefault(),
				MousePos = PixelPosition.GetValueOrDefault(),
				KeyMethod = KeyPressMethod.GetValueOrDefault(),
				MouseKey = MouseKeyCode.GetValueOrDefault()
			}, 
			ActionTypes.KeyboardKey => new KeyboardKeyAction
			{
				KeyboardKey = KeyboardKeyCode.GetValueOrDefault(),
				KeyMethod = KeyPressMethod.GetValueOrDefault()
			}, 
			ActionTypes.Wait => new WaitAction
			{
				SleepAmount = WaitFor.GetValueOrDefault()
			}, 
			ActionTypes.Text => new TextAction
			{
				Text = (Text ?? null)
			}, 
			_ => new WaitAction(), 
		};
	}
}
