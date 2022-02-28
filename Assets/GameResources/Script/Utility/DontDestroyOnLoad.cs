using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DontDestroyOnLoad: MonoBehaviour
{
	protected virtual void Awake()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		DontDestroyOnLoad(gameObject);
	}
}
