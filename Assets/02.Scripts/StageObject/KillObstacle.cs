using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObstacle : MonoBehaviour
{

	private void Awake() {
		GetComponent<MeshRenderer>().enabled = false;
	}
	//피리부를 죽이기
	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.Equals(GameManager.Instance.hero.unit.foot.footcollider)) {
			GameManager.Instance.hero.stateMachine.SetState(States.Hero_Die);
		}
	}
}
