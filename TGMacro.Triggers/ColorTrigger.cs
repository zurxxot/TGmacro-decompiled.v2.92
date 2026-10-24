using System;
using System.Drawing;
using System.Threading;
using TGMacro.Bases;
using TGMacro.Enums;
using TGMacro.Statics;

namespace TGMacro.Triggers;

public class ColorTrigger : TriggerBase, IDisposable
{
	private bool Triggered = false;

	public ColorTriggerMethods Method { get; set; }

	public Color Color { get; set; }

	public Point Pos { get; set; }

	public int ColorTolerance { get; set; } = 5;

	public bool TriggerOnce { get; set; } = false;

	public ColorTrigger()
	{
		TriggerType = TriggerTypes.Pixel;
		TriggerThread = new Thread(watchColor);
		TriggerThread.Start();
	}

	private void watchColor()
	{
		while (true)
		{
			base.RunState = TriggerRunState.Stop;
			SpinWait.SpinUntil(() => AppSession.Enable && base.IsEnable);
			bool flag = ColorSimilarity(SharedMethods.GetColorAt(Pos), Color) + (float)ColorTolerance >= 100f;
			if (Method == ColorTriggerMethods.ColorMatches)
			{
				if (flag)
				{
					if (!Triggered || !TriggerOnce)
					{
						base.RunState = (TriggerOnce ? TriggerRunState.RunOnce : TriggerRunState.Run);
					}
					Triggered = true;
				}
				else
				{
					Triggered = false;
				}
			}
			else if (!flag)
			{
				if (!Triggered || !TriggerOnce)
				{
					base.RunState = (TriggerOnce ? TriggerRunState.RunOnce : TriggerRunState.Run);
				}
				Triggered = true;
			}
			else
			{
				Triggered = false;
			}
			Thread.Sleep(100);
		}
	}

	public float ColorSimilarity(Color color1, Color color2)
	{
		int num = Math.Abs(color1.R - color2.R);
		int num2 = Math.Abs(color1.G - color2.G);
		int num3 = Math.Abs(color1.B - color2.B);
		int num4 = num + num2 + num3;
		return (1f - (float)num4 / 765f) * 100f;
	}
}
