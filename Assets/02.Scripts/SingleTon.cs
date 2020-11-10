using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour {

	protected static T instance = null;
	public static T Instance {
		get {
            if (instance == null) {
				instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
				DontDestroyOnLoad(instance);
			}
			return instance;
		}
	}
}