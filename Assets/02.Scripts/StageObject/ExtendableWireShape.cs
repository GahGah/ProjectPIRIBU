using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class ExtendableWireShape : MonoBehaviour
{

	public Transform fixTransform;
	public SpriteShapeController shapeController;
	Spline spline;

	private void Awake() {
		shapeController = GetComponent<SpriteShapeController>();
	}
	
	void Update()
	{
		UpdateSpline();
	}

	void UpdateSpline() {
		if (fixTransform && shapeController) {
			spline = shapeController.spline;
			Vector3 pos = fixTransform.position - transform.position;
			pos.x = 0;
			pos.z = 0;
			spline.SetPosition(1, pos);
		}
	}
}
