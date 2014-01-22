using UnityEngine;
using System.Collections;

public class CountDownTimer : Timer
{
	//Flag to check if we have started this timer
	[SerializeField] private bool	mStarted;

	#region Unity Function
	private void Update()
	{
		// if we started the timer, we will decrease the actual time, till it reaches 0;
		// we will then call the function we want to fire;
		if(mStarted)
		{
			ActualTimer-=Time.deltaTime;
			if(ActualTimer < 0)
			{
				if(TimerFunctionHook != null) TimerFunctionHook();
			}
		}
	}
	#endregion

	// Accessors for the private variables
	#region Class Function
	public bool Started
	{
		get { return mStarted;	}
		set { mStarted = value;	}
	}
	#endregion

	// Function that we are firing
	#region Delegate & Event Function
	public delegate void TimerFunctionDelegate();
	public event TimerFunctionDelegate TimerFunctionHook;
	#endregion
}

