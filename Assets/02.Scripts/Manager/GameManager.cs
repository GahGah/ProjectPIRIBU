using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

internal static class YieldInstructionCache
{
    class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y)
        {
            return x == y;
        }
        int IEqualityComparer<float>.GetHashCode(float obj)
        {
            return obj.GetHashCode();
        }
    }
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

}
public class GameManager : SingleTon<GameManager>
{
    /* 
	 * "기능 플랫폼, 그래픽 리소스"들의 Start 함수에서 isDebugMode 변수 체크.
	 * if: isDebugMode == true 일 시,
	 * [기능 플랫폼].Renderer.enabled = true; [그래픽 리소스].Renderer.enabled = false;
	 * else:
	 * [기능 플랫폼].Renderer.enabled = false; [그래픽 리소스].Rnderer.enabled = true;
	 */

    public bool isDebugMode = false;

	[HideInInspector] public bool isStageScene = false;
	[HideInInspector] public InputManager inputManager;
    [HideInInspector] public List<Child> childs;
    [HideInInspector] public Hero hero;
    [HideInInspector] public Rect childsRange;

	[HideInInspector] public int maxChildNumber;
	AudioSource bgm;
	float targetPitch;
	float currPitch;

    //피리 UI를 띄울 위치입니다.
	//씬 재로딩하면 Null 되어서 찾아가도록 합니다~
    public Transform piriUIPosition;

    //피리 이펙트를 나오게 할 위치...
    //PUP랑 동일하게 썼는데 오류 나려나 ㅋㅋㅋㅋㅋ
    public Transform piriEffectPosition;

    protected override void Awake()
    {
        base.Awake();//여기서 Init한다.
    }

    protected override void Init()
    {
        base.Init();
		inputManager = InputManager.Instance;
		childsRange = new Rect();
		
		//새 씬이 로딩될때마다 캐릭터 검색
		SceneManager.sceneLoaded += (x, y) => { FindObjectsInStage(); };
	}

	void FindObjectsInStage() {
		hero = null;
		childs.Clear();
		
		//Hard Coding : "피리부가 씬에 없으면 게임씬 아닌걸로 알겠다."
		hero = FindObjectOfType<Hero>();

		if (hero) {
			isStageScene = true;

			//피리UI 트랜스폼 검색
			foreach (Transform trans in hero.GetComponentInChildren<Transform>()) {
				if (trans.gameObject.name == "Piri UI Position") {
					piriUIPosition = trans;
					break;
				}

			}

            //피리UI 트랜스폼 검색
            foreach (Transform trans in hero.GetComponentInChildren<Transform>())
            {
                if (trans.gameObject.name == "Piri Effect Position")
                {
                    piriEffectPosition = trans;
                    break;
                }

            }


            //아이들 검색
            Child[] children = FindObjectsOfType<Child>();
			foreach (Child child in children) {
				if (!childs.Contains(child))
					childs.Add(child);
			}

			maxChildNumber = childs.Count();

		} else {
			isStageScene = false;
		}

	}

    //임시 배경음 재생
    private void Start()
    {
        //사운드 매니저 추가에 따라서 이거 주석처리
        //bgm = gameObject.GetComponent<AudioSource>();
        //     if (bgm!=null)
        //     {
        //         if (!bgm.isPlaying)
        //             bgm.Play();

        targetPitch = 1;
        currPitch = targetPitch;

        //     }

    }


    private void Update()
    {
        //ESC키 눌러 일시정지
        if (inputManager.buttonPause.wasPressedThisFrame)
        {
            UIManager.Instance.PauseToggle();
        }

		//스테이지 씬에서의 동작
		if (isStageScene) {
			UpdateChildRange();

			targetPitch = Mathf.Lerp(targetPitch, 1 + (float)(GetChildNumber() - maxChildNumber) / maxChildNumber * 0.4f, Time.deltaTime*2);
			currPitch = Mathf.Lerp(currPitch,targetPitch,Time.deltaTime*2);
            Debug.Log((float)(GetChildNumber() - maxChildNumber));
            if ((float)(GetChildNumber() - maxChildNumber)<0)
            {
                SoundManager.instance.audioSources[0].pitch = currPitch;
                if (SoundManager.instance.audioSources[0].isPlaying == false)
                {
                    SoundManager.instance.audioSources[0].Play();
                }
            }
            if ((float)(GetChildNumber() - maxChildNumber)== 0)
            {
                SoundManager.instance.audioSources[0].pitch = 1;
                if (SoundManager.instance.audioSources[0].isPlaying == false)
                {
                    SoundManager.instance.audioSources[0].Play();
                }
            }

			//bgm.pitch = currPitch;
		}
    }

    public void SetChildFollow(bool isFollow)
    {
        foreach (Child child in childs)
        {
            child.SetFollow(isFollow);
        }
    }
    public void DrawChildRange()
    {
        Debug.DrawLine(new Vector2(childsRange.xMin, childsRange.yMin), new Vector2(childsRange.xMax, childsRange.yMin));
        Debug.DrawLine(new Vector2(childsRange.xMin, childsRange.yMax), new Vector2(childsRange.xMax, childsRange.yMax));
        Debug.DrawLine(new Vector2(childsRange.xMin, childsRange.yMin), new Vector2(childsRange.xMin, childsRange.yMax));
        Debug.DrawLine(new Vector2(childsRange.xMax, childsRange.yMin), new Vector2(childsRange.xMax, childsRange.yMax));
    }
    public void UpdateChildRange()
    {
        Vector2 leftBottom = new Vector2(Mathf.Infinity, Mathf.Infinity);
        Vector2 rightTop = new Vector2(-Mathf.Infinity, -Mathf.Infinity);
        if (childs!=null)
        {
            foreach (Child child in childs)
            {
                Vector2 pos = child.unit.transform.position;
                if (pos.x < leftBottom.x)
                    leftBottom.x = pos.x;
                if (pos.y < leftBottom.y)
                    leftBottom.y = pos.y;
                if (pos.x > rightTop.x)
                    rightTop.x = pos.x;
                if (pos.y > rightTop.y)
                    rightTop.y = pos.y;

            }
            childsRange.xMin = leftBottom.x;
            childsRange.yMin = leftBottom.y;
            childsRange.xMax = rightTop.x;
            childsRange.yMax = rightTop.y;
        }

    }

    /// <summary>
    /// 아이들의 X범위 반환
    /// </summary>
    /// <returns>x : 왼쪽, y : 오른쪽 </returns>
    public Vector2 GetChildRangeX()
    {
        Vector2 ret = new Vector2(childsRange.xMin, childsRange.xMax);
        return ret;
    }

	public int GetChildNumber() {
		return childs.Where(x => x != null && x.enabled).Count();
	}

	public void UsePiri() {
		targetPitch = 1.4f;
	}

}
