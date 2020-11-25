using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class CharacterFoot : MonoBehaviour
{
	[HideInInspector]
	public Vector2 size;
	[HideInInspector]
	public CapsuleCollider2D footcollider;
	[HideInInspector]
	public Collider2D footSensor;
	[HideInInspector]
	public List<LiftObject> adjacentLiftObjects;//인접해있는 모든 LiftObject
	void Awake() {
		footcollider = GetComponent<CapsuleCollider2D>();
		footSensor = GetComponent<BoxCollider2D>();
		size = new Vector2(footcollider.size.x * transform.localScale.x,
			footcollider.size.y* transform.localScale.y);

	   adjacentLiftObjects = new List<LiftObject>();
	}

	public void IgnoreWith(Collider2D collider,bool isIgnore = true) {
		Physics2D.IgnoreCollision(footcollider, collider, isIgnore);
		Physics2D.IgnoreCollision(footSensor, collider, isIgnore);
	}

	//인접 부모 관리는 별도의 Trigger가 함 (조금만 떨어져도 Deparent 현상 일어나는것 방지)
	private void OnTriggerEnter2D (Collider2D collision) {
		LiftObject element = collision.gameObject.GetComponent<LiftObject>();
		if (element != null)
			if (!adjacentLiftObjects.Contains(element))
				adjacentLiftObjects.Add(element);
	}

	private void OnTriggerExit2D(Collider2D collision) {
		LiftObject element = collision.gameObject.GetComponent<LiftObject>();
		if (element != null)
			adjacentLiftObjects.Remove(element);
	}
}
