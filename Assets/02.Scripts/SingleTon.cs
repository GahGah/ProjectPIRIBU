using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : class, new() {
	protected static object _instanceLock = new object();
	protected static volatile T _instance;
	public static T Instance {
		get { return _instance; }
	}

	protected virtual void Awake() {
		if (_instance == null) {
			lock (_instanceLock) {
				if (null == _instance) {
					_instance = new T();
					DontDestroyOnLoad(this.gameObject);
				}
			}
		} else {
			Destroy(this);
		}
	}
}
