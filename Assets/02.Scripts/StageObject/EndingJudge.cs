using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EndingJudge : MonoBehaviour
{
	bool SceneChangeActive;
	private void Awake() {
		SceneChangeActive = false;
	}
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Player") && !SceneChangeActive) {
			
			//살아있는 아이들
			int childAlive = GameManager.Instance.GetChildNumber();
			Debug.Log("살아있는 아이들 : " + childAlive);

			//데려온 아이들
			int childFollow = GameManager.Instance.childs.Where(
				child => child.isActiveAndEnabled &&
				Vector3.Distance(child.transform.position, GameManager.Instance.hero.transform.position) <= 10
				).Count();
			Debug.Log("데려온 아이들 : " + childFollow);

			//엔딩분기
			int childMax = GameManager.Instance.maxChildNumber;
			int deadChild = childMax - childAlive;
			int missingChild = childMax - childFollow;
			
			//다 살려서 데려올시 해피엔딩
			if (deadChild == 0 && missingChild == 0) {
				SceneChanger.Instance.LoadScene("CutScene_GoodEnding");
				SceneChangeActive = true;
			}

			//아이들 2명 이하로 소모시 노말엔딩
			else if (deadChild <= 2) {
				SceneChanger.Instance.LoadScene("CutScene_NormalEnding");
				SceneChangeActive = true;
			} 
			//아이들 3명 이상 소모시 배드엔딩
			else {
				SceneChanger.Instance.LoadScene("CutScene_BadEnding");
				SceneChangeActive = true;
			}
		}
		
	}
}
