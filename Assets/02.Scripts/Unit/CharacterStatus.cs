﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterStatus
{
	public MoveSpeed groundMoveSpeed = new MoveSpeed(1,2,10);//공중 이동속도 관련값
	public MoveSpeed airMoveSpeed = new MoveSpeed(1,2,10);//지형 이동속도 관련값
	[HideInInspector] public float sideMoveSpeed = 0;//좌우 이동속력
	[HideInInspector] public float verticalSpeed = 0;//상하 이동속력
	[HideInInspector] public int modelSide = 1;//바라보는 방향
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