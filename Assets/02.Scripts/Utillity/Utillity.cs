using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// PreciseVector3
/// 그냥 벡터 자료형끼리 더하니 소수점 정밀도가 심히 낮다.
/// 어떻게 할지 몰라서 일단 구조체 새로 만듬
/// </summary>

public struct PreVector3 {
	public double x;
	public double y;
	public double z;

	public PreVector3(double _x = 0, double _y = 0, double _z = 0) {
		x = _x;
		y = _y;
		z = _z;
	}
	public PreVector3(Vector3 _vec) {
		this = new PreVector3(_vec.x, _vec.y, _vec.z);
	}

	public static PreVector3 operator + (PreVector3 a, PreVector3 b) {
		return new PreVector3(a.x + b.x, a.y + b.y, a.z + b.z);
	}
	public static PreVector3 operator -(PreVector3 a, PreVector3 b) {
		return new PreVector3(a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static PreVector3 operator *(PreVector3 a, float f) {
		return new PreVector3(a.x*f, a.y*f, a.z*f);
	}
	public static PreVector3 operator /(PreVector3 a, float f) {
		return new PreVector3(a.x / f, a.y / f, a.z / f);
	}

	public static Vector3 operator + (PreVector3 preVec, Vector3 vec) {
		return new Vector3((float)preVec.x+vec.x, (float)preVec.y+vec.y, (float)preVec.z+vec.z);
	}
	public static Vector3 operator +(Vector3 vec, PreVector3 preVec) {
		return new Vector3((float)preVec.x + vec.x, (float)preVec.y + vec.y, (float)preVec.z + vec.z);
	}
	public static Vector3 operator -(PreVector3 preVec, Vector3 vec) {
		return new Vector3((float)preVec.x - vec.x, (float)preVec.y - vec.y, (float)preVec.z - vec.z);
	}
	public static Vector3 operator -(Vector3 vec, PreVector3 preVec) {
		return new Vector3(vec.x - (float)preVec.x, vec.y - (float)preVec.y, vec.z - (float)preVec.z);
	}

	public override string ToString() {
		return "(" + x + ", " + y + ", " + z + ")";
	}
}

public static class Utility
{	
	delegate Vector3 Vector3_Operator(Vector3 a, Vector3 b);

	//벡터끼리 더하거나 빼면 소수점이 한자리가 되어버린다.. 어떻게 해결하지?
	public static PreVector3 GetVector3Minus(Vector3 a, Vector3 b) {
		return new PreVector3(a.x - b.x, a.y - b.y, a.z - b.z);
	}
	public static PreVector3 GetVector3Plus(Vector3 a, Vector3 b) {
		return new PreVector3(a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static void DrawDir(Vector3 Pos, Vector3 Dir, Color color) {
		Debug.DrawLine(Pos, Pos + Dir.normalized * 3, color);
	}
}
