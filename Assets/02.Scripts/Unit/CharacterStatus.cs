using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterStatus
{
	public string name;
	public float hp;
	public float walkSpeed = 2;
	public MoveSpeed groundMoveSpeed = new MoveSpeed();//공중 이동속도 관련값
	public MoveSpeed airMoveSpeed = new MoveSpeed();//지형 이동속도 관련값
	public float sideMoveSpeed = 0;//좌우 이동속력
	public float verticalSpeed = 0;//상하 이동속력

	public float jumpSpeed = 17;//점프력
	public float fallSpeed = 1.2f;//추락속도

}

[Serializable]
public struct MoveSpeed {
	public float accel;
	public float brake;
	public float max;
	public MoveSpeed(float _accel = 2, float _brake = 3, float _max = 15) {
		accel = _accel;
		brake = _brake;
		max = _max;
	}
}