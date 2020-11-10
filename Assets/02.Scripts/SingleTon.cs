using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour {

	protected static T instance = null;
	public static T Instance {
		get {
            if (instance == null) {
				//일반적인 Singleton 클래스는 게임오브젝트를 생성한다.
				instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
				DontDestroyOnLoad(instance);
			}
			return instance;
		}
	}
	//GameManager처럼 씬 게임오브젝트 컴포넌트로 이미 들어가있는 싱글턴들은
	//무조건 Awake에서 Init함수를 통해 자기 자신을 instance로 지정하도록 한다.
	protected virtual void Awake() {
		Init();
	}

	protected virtual void Init() {
		if (instance == null) {
			instance = this.gameObject.GetComponent<T>();
			DontDestroyOnLoad(this);
		} else if (instance != this) {
			Destroy(this);
		}
	}

	
}