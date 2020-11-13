using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CharacterFoot : MonoBehaviour
{
	[HideInInspector]
	public Collider2D collider;
	[HideInInspector]
	public List<LinearPlatform> adjacentlinearPlatforms;//붙어있는 땅들
	// Start is called before the first frame update
	void Awake() {
		collider = GetComponent<Collider2D>();
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
