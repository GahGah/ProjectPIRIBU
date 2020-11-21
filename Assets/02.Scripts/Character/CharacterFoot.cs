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
	public List<LinearPlatform> adjacentlinearPlatforms;//붙어있는 땅들
	void Awake() {
		footcollider = GetComponent<CapsuleCollider2D>();
		size = footcollider.size;
		adjacentlinearPlatforms = new List<LinearPlatform>();
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("LinearPlatform")) {
			try {
				adjacentlinearPlatforms.Add(collision.gameObject.GetComponent<LinearPlatform>());
			} catch {}
		}
	}

	private void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("LinearPlatform")) {
			try {
				adjacentlinearPlatforms.Remove(collision.gameObject.GetComponent<LinearPlatform>());
			} catch {}
		}
	}
}
