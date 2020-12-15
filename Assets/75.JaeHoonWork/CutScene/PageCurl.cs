using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class PageCurl : MonoBehaviour {

	public Transform _Front;
	public Transform _Mask;
	public Transform _GradOutter;
	public Vector3 _Pos = new Vector3(-240.0f, -470.0f, 0.0f) * 0.01f;
	private Sprite sprite;
	void LateUpdate() {
		transform.position = _Pos;
		transform.eulerAngles = Vector3.zero;

		Vector3 pos = _Front.localPosition;
		float theta = Mathf.Atan2(pos.y, pos.x) * 180.0f / Mathf.PI;

		//if (theta <= 0.0f || theta >= 90.0f) return;

		float deg = -(90.0f - theta) * 2.0f;
		_Front.eulerAngles = new Vector3(0.0f, 0.0f, deg);

		_Mask.position = (transform.position + _Front.position) * 0.5f;
		_Mask.eulerAngles = new Vector3(0.0f, 0.0f, deg * 0.5f);

		_GradOutter.position = _Mask.position;
		_GradOutter.eulerAngles = new Vector3(0.0f, 0.0f, deg * 0.5f + 90.0f);

		transform.position = _Pos;
		transform.eulerAngles = Vector3.zero;
	}
	public void SetSprite(Sprite _sprite) {
		sprite = _sprite;
		GetComponent<Image>().sprite = sprite;
		_Front.GetComponent<Image>().sprite = sprite;
	}

	bool isStarted = false;
	public void NextPage() {
		if (!isStarted) {
			isStarted = true;
			SoundManager.Instance.PlaySFXOneShot(SoundManager.Instance.audioClips[2]);
			_Front.gameObject.GetComponent<Animator>().SetTrigger("NextPage");
			Destroy(transform.parent.gameObject, 2f);
		}
	}



}
