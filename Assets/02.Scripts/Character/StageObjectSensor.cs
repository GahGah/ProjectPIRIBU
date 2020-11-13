using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StageObjectSensor : MonoBehaviour
{
	[HideInInspector]
	public List<LinearPlatform> linearPlatforms;
    // Start is called before the first frame update
    void Awake()
    {
		linearPlatforms = new List<LinearPlatform>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("LinearPlatform")) {
			try {
				linearPlatforms.Add(collision.gameObject.GetComponent<LinearPlatform>());
			} catch {
				Debug.LogError("LinearPlatform Not Found!!", collision.gameObject);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("LinearPlatform")) {
			try {
				linearPlatforms.Remove(collision.gameObject.GetComponent<LinearPlatform>());
			} catch {
				Debug.LogError("LinearPlatform Not Found!!", collision.gameObject);
			}
		}
	}
}
