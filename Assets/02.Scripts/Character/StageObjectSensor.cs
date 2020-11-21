using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StageObjectSensor : MonoBehaviour
{
	[HideInInspector]
	public List<LinearPlatform> linearPlatforms;
	[HideInInspector]
	public List<InteractionObject> InteractionObjects;
	void Awake()
    {
		linearPlatforms = new List<LinearPlatform>();
		InteractionObjects = new List<InteractionObject>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("LinearPlatform")) {
			try {
				linearPlatforms.Add(collision.gameObject.GetComponent<LinearPlatform>());
			} catch {
				Debug.LogError("LinearPlatform Not Found!!", collision.gameObject);
			}
		} else if (collision.gameObject.CompareTag("InteractionObject")) {
			try {
				InteractionObjects.Add(collision.gameObject.GetComponent<InteractionObject>());
			} catch {
				Debug.LogError("InteractionObject Not Found!!", collision.gameObject);
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
		} else if (collision.gameObject.CompareTag("InteractionObject")) {
			try {
				InteractionObjects.Remove(collision.gameObject.GetComponent<InteractionObject>());
			} catch {
				Debug.LogError("InteractionObject Not Found!!", collision.gameObject);
			}
		}
	}
}
