using System.Collections;
using System.Collections.Generic;
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

    //private static readonly Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(new FloatComparer());

    //public static WaitForSeconds WaitForSeconds(float _seconds)
    //{
    //    WaitForSeconds _waitForSeconds;
    //    if (!_timeInterval.TryGetValue(_seconds, out _waitForSeconds))
    //    {
    //        _timeInterval.Add(_seconds, _waitForSeconds = new WaitForSeconds(_seconds));
    //        return _waitForSeconds;
    //    }
    //    else
    //    {
    //        return null;
    //    }

    //}
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

	public float timer = 0f;

	public InputManager inputManager;
	public Hero hero;
	public List<Child> childs;
	[HideInInspector] public Rect childsRange;

	protected override void Awake() {
		base.Awake();//여기서 Init한다.
	}

	protected override void Init()
	{
		base.Init();
		inputManager = InputManager.Instance;

		//아이들 관련
		Child[] children = FindObjectsOfType<Child>();
		foreach (Child child in children) {
			if (!childs.Contains(child))
				childs.Add(child);
		}
		childsRange = new Rect();
	}


	private void Update() {
		//ESC키 누르면 임의 리셋
		if (inputManager.buttonPause.isPressed) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		UpdateChildRange();
		DrawChildRange();
	}

	public void DrawChildRange() {
		Debug.DrawLine(new Vector2(childsRange.xMin, childsRange.yMin), new Vector2(childsRange.xMax, childsRange.yMin));
		Debug.DrawLine(new Vector2(childsRange.xMin, childsRange.yMax), new Vector2(childsRange.xMax, childsRange.yMax));
		Debug.DrawLine(new Vector2(childsRange.xMin, childsRange.yMin), new Vector2(childsRange.xMin, childsRange.yMax));
		Debug.DrawLine(new Vector2(childsRange.xMax, childsRange.yMin), new Vector2(childsRange.xMax, childsRange.yMax));
	}
	public void UpdateChildRange() {
		Vector2 leftBottom = new Vector2(Mathf.Infinity, Mathf.Infinity);
		Vector2 rightTop = new Vector2(-Mathf.Infinity, -Mathf.Infinity);
		foreach (Child child in childs) {
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

	/// <summary>
	/// 아이들의 X범위 반환
	/// </summary>
	/// <returns>x : 왼쪽, y : 오른쪽 </returns>
	public Vector2 GetChildRangeX() {
		Vector2 ret = new Vector2(childsRange.xMin, childsRange.xMax);
		return ret;
	}

}
