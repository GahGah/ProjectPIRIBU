using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
	public GameObject pagePrefab;
	public List<Sprite> sprites;
	public string nextSceneName;
	private List<PageCurl> pages;
	PageCurl page;
	bool disableNext;
	void Start()
	{
		pages = new List<PageCurl>();

		//첫번째 페이지가 가장 마지막에 Instantiate되어야 가장 위에 보이게 되므로 Reverse
		sprites.Reverse();
		foreach (Sprite sprite in sprites) {
			page = Instantiate(pagePrefab,transform).GetComponentInChildren<PageCurl>();
			pages.Add(page);
			page.SetSprite(sprite);
		}

		//첫 페이지가 마지막 item이므로 다시 Reverse시킴
		pages.Reverse();
		page = pages[0];
		disableNext = false;
	}

	void Update()
	{
		
		if (InputManager.Instance.buttonMouseLeft.wasPressedThisFrame && !disableNext) {
			
			if (!pages[0]) {
				pages.RemoveAt(0);
				page = pages[0];
			}

			if (pages.Count > 1)
				page.NextPage();
			else {
				SceneChanger.Instance.LoadScene(nextSceneName);
				disableNext = true;
			}
		}
	}
}
