using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Singletone<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance = null;
	public static T Instance
	{
		get { return instance; }
		set { }
	}

	protected virtual void Awake()
	{
		if(instance == null)
		{
			instance = GetComponent<T>();
		}
		else
		{
			DestroyImmediate (this);
		}
	}
}
