using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using TGMacro.Enums;
using TGMacro.Interfaces;
using TGMacro.Statics;

namespace TGMacro.Bases;

public abstract class TriggerBase : IDisposable
{
	public delegate void StatusUpdate();

	private TriggerRunState _runState;

	public TriggerTypes TriggerType;

	private bool _isEnable;

	public List<IMacroAction> Actions = new List<IMacroAction>();

	public Thread TriggerThread = null;

	private Thread baseTriggerThread;

	public virtual TriggerRunState RunState
	{
		get
		{
			return _runState;
		}
		set
		{
			if (!IsEnable || !AppSession.Enable)
			{
				_runState = TriggerRunState.Stop;
			}
			else
			{
				_runState = value;
			}
			this.TriggerRunningStatusChanged?.Invoke();
		}
	}

	public bool IsEnable
	{
		get
		{
			return _isEnable;
		}
		set
		{
			_isEnable = value;
			if (!value)
			{
				RunState = TriggerRunState.Stop;
			}
		}
	}

	public event StatusUpdate TriggerRunningStatusChanged;

	public TriggerBase()
	{
		InitiateThread();
	}

	private void InitiateThread()
	{
		baseTriggerThread = new Thread(Watch);
		baseTriggerThread.Start();
		if (TriggerThread != null && TriggerThread.IsAlive)
		{
			TriggerThread.Abort();
			TriggerThread = null;
		}
	}

	public void Start()
	{
		if (baseTriggerThread != null && baseTriggerThread.IsAlive)
		{
			Restart();
		}
		else
		{
			InitiateThread();
		}
	}

	public void Stop()
	{
		if (this.TriggerRunningStatusChanged != null)
		{
			Delegate[] invocationList = this.TriggerRunningStatusChanged.GetInvocationList();
			foreach (Delegate obj in invocationList)
			{
				TriggerRunningStatusChanged -= (StatusUpdate)obj;
			}
		}
		if (baseTriggerThread != null && baseTriggerThread.IsAlive)
		{
			baseTriggerThread.Abort();
		}
		if (TriggerThread != null && TriggerThread.IsAlive)
		{
			TriggerThread.Abort();
			TriggerThread = null;
		}
	}

	public void Restart()
	{
		Stop();
		InitiateThread();
	}

	public void Watch()
	{
		while (true)
		{
			SpinWait.SpinUntil(() => IsEnable && Actions.Count > 0 && AppSession.Enable && RunState != TriggerRunState.Stop);
			ReadOnlyCollection<IMacroAction> readOnlyCollection = Actions.AsReadOnly();
			for (int num = 0; num < readOnlyCollection.Count; num++)
			{
				readOnlyCollection[num].DoJob();
				if (RunState == TriggerRunState.Stop)
				{
					break;
				}
			}
			if (RunState != TriggerRunState.Run)
			{
				RunState = TriggerRunState.Stop;
			}
			Thread.Sleep(1);
		}
	}

	public void Dispose()
	{
		Stop();
	}
}
