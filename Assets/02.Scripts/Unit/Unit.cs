using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//물리 + 다른 객체들과의 상호작용 담당
public class Unit : MonoBehaviour
{
	public T GetChildClass<T>() where T : Unit {
		return (T)this;
	}
}
