using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hero : Character {

	protected override void Start() {
		base.Start();
		stateMachine = new CharacterStateMachine(this, States.Hero_Ground);
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
	}

}

public class HeroState : CharacterState {
	public Hero hero;
	protected InputManager input;
	public override void Init() {
		base.Init();
		hero = character.GetChildClass<Hero>();
		input = InputManager.Instance;
	}
}

//지형 상태
public class HeroGround : HeroState {

	Vector2 groundForward;//GroundNormal의 직교벡터
	public override void Enter() {
		moveStat.verticalSpeed = 0;
	}
	public override void Execute() {

		//지형 부착
		if (unit.AttachGround()) {
			groundForward = unit.groundForward;
		} else {
			//땅이 아닐시 공중상태
			sm.SetState(States.Hero_Air);
			//Logic Error Colplete : 이 프레임에선 이동호출이 안되어서 끊기는 현상 발생했음
			unit.SetMovement(MovementType.SetVelocity, groundForward * moveStat.sideMoveSpeed);
			return;
		}

		//좌우이동
		unit.HandleMoveSpeed(unit.GetSideMoveDirection(), moveStat.groundMoveSpeed);

		
		//이동호출
		Vector2 vel = groundForward * moveStat.sideMoveSpeed;
		//Vector2 vel = new Vector2(charStat.sideMoveSpeed, 0);
		unit.SetMovement(MovementType.SetVelocity, vel);

		if (input.buttonJump.isPressed) {
			sm.SetState(States.Hero_Jump);
			return;
		}

		//상호작용 오브젝트
		List<InteractionObject> interactions = unit.sensor.InteractionObjects;
		foreach (InteractionObject interaction in interactions) {
			switch (interaction.type) {
				//사다리
				case InteractionType.Ladder:
					if ((input.buttonUp.isPressed || input.buttonDown.isPressed)//위아래 키를 누를때만
						&& unit.GetDistanceToLadder(interaction) <= 0.6f //사다리 근처에 있을때만
						&& unit.IsInLadder(unit.transform.position, 0, interaction)//사다리 범위 내에 있을 시에만
						) {
						unit.interactionObject = interaction;
						sm.SetState(States.Hero_Ladder);
						return;
					}
					break;

				//쉬프트 눌러 박스 밀기
				case InteractionType.PushBox:
					if (input.buttonCatch.isPressed) {
						//박스가 앞에 있을때만
						if (unit.WallCheck(moveStat.modelSide,interaction.gameObject)) {
							unit.interactionObject = interaction;
							sm.SetState(States.Hero_Box);
						}
					}
					break;
			}
		}
	}

}



//공중 상태
public class HeroAir : HeroState {
	public override void Enter() {
		//관성 구현
		unit.SetMovement(MovementType.AddVelocity, unit.deltaPosFromParent);

	}
	public override void Execute() {
		//좌우이동
		unit.HandleMoveSpeed(unit.GetSideMoveDirection(), moveStat.airMoveSpeed);

		//추락
		moveStat.verticalSpeed -= moveStat.fallSpeed;

		//이동호출
		Vector2 vel = new Vector2(moveStat.sideMoveSpeed, moveStat.verticalSpeed);
		unit.SetMovement(MovementType.SetVelocity, vel);

		//착지판정
		if (unit.GroundCheckFromAir()) {
			sm.SetState(States.Hero_Ground);
		}

		//상호작용 오브젝트
		List<InteractionObject> interactions = unit.sensor.InteractionObjects;
		foreach (InteractionObject interaction in interactions) {
			switch (interaction.type) {
				//사다리
				case InteractionType.Ladder:
					if ((input.buttonUp.isPressed || input.buttonDown.isPressed)//위아래 키를 누를때만
						&& unit.GetDistanceToLadder(interaction) <= 0.6f //사다리 근처에 있을때만
						&& unit.IsInLadder(unit.transform.position,0,interaction)//사다리 범위 내에 있을 시에만
						) {
							unit.interactionObject = interaction;
							sm.SetState(States.Hero_Ladder);
							return;
					}
					break;

				//공중에선 박스를 밀지 않음
				case InteractionType.PushBox:
					break;
			}
		}
	}
}

//점프
public class HeroJump : HeroState {
	public override void Enter() {
		unit.ResetLiftParent();
		unit.foot.adjacentLiftObjects.Clear();
		moveStat.verticalSpeed = moveStat.jumpSpeed;
		sm.SetState(States.Hero_Air);
	}
	public override void Execute() {
	}
}

//사다리 액션
public class HeroLadder : HeroState {
	InteractionObject ladder;
	HingeJoint2D joint;
	float attachSpeed;//사다리에 들러붙는 가중치
	float time;//바로 점프 못하게 딜레이
	public override void Enter() {
		ladder = unit.interactionObject;
		joint = ladder.gameObject.AddComponent<HingeJoint2D>();
		joint.connectedBody = unit.rigid;
		joint.autoConfigureConnectedAnchor = false;
		
		//사다리 탈땐 OneWay 무시
		unit.foot.gameObject.SetActive(false);

		moveStat.sideMoveSpeed = 0;
		moveStat.verticalSpeed = 0;

		attachSpeed = 0.1f;

		time = 0;
	}

	public override void Execute() {
		//사다리에 무게싣기 (Logic Error : 이렇게 구현하면 안된다)
		unit.SetMovement(MovementType.AddVelocity, Vector2.down*moveStat.fallSpeed*5);

		
		Vector3 newAnchor = joint.connectedAnchor;
		
		//사다리에 달라붙기
		Vector2 dirToLadder = unit.GetDirectionToLadder();
		newAnchor -= (Vector3)dirToLadder*attachSpeed;
		attachSpeed += (1-attachSpeed)*0.1f;


		Vector3 ladderDir = ladder.transform.up;
		//위아래 이동
		if (Vector3.Magnitude(dirToLadder) <= 0.2f) {//사다리에 붙어있을때만
			if (input.buttonUp.isPressed) {
				newAnchor -= ladderDir*0.15f;
			}
			if (input.buttonDown.isPressed) {
				newAnchor += ladderDir*0.15f;
			}
		}
		joint.connectedAnchor = newAnchor;

		//사다리를 벗어났는가?
		if (!unit.IsInLadder(ladder.transform.position-newAnchor,0.1f)) {
			sm.SetState(States.Hero_Air);
			return;
		}


		//점프로 끊기 (점프력을 낮게)
		time += Time.deltaTime;
		if (input.buttonJump.isPressed && time > 0.5f) {
			unit.ResetLiftParent();
			unit.foot.adjacentLiftObjects.Clear();
			moveStat.verticalSpeed = moveStat.jumpSpeed*0.5f;
			sm.SetState(States.Hero_Air);
			return;
		}
	}
	public override void Exit() {
		unit.foot.gameObject.SetActive(true);
		BreakJoint();
	}

	void BreakJoint() {
		if (joint) {
			joint.breakForce = 0;//알아서 컴포넌트 삭제됨
			joint = null;
		}
	}
}

//박스밀기
public class HeroBox : HeroState {
	PushBox box;
	int pushSide;
	public override void Enter() {
		box = unit.interactionObject.GetChildObject<PushBox>();
		box.isPushingMode = true;
		
		//미는방향
		pushSide = box.currPos.x-unit.currPos.x > 0 ? 1 : -1;

	}

	public override void Execute() {

		//박스가 앞에 없거나, 키를 뗐을 시 원상태로
		if (!unit.WallCheck(pushSide, box.gameObject, 0.25f) || !input.buttonCatch.isPressed) {
			sm.SetState(States.Hero_Ground);
			return;
		}

		//땅과 거리차가 날시 추락
		if (!unit.AttachGround()) {
			sm.SetState(States.Hero_Air);
			return;
		}

		//박스 밀기
		int moveDir = unit.GetSideMoveDirection();
		

		//움직이지 않는동안 박스 정지
		box.isPushingMode = Mathf.Abs(moveStat.sideMoveSpeed) <= 1 ? false : true;

		//밀때는 앞의 박스를 벽으로 인식하지 않고, 당길때는 뒤의 벽을 인식하게
		bool considerWall = moveDir == pushSide ? false : true;
		unit.HandleMoveSpeed(moveDir, moveStat.groundMoveSpeed, considerWall);


		//Logic Error : MovePosition을 사용하면 뒤에 다른 박스들이 수십개 있어도 똑같은 강도로 민다.
		Vector2 moveVec = unit.groundForward * moveStat.sideMoveSpeed * 0.5f*Time.deltaTime;
		box.SetMovement(MovementType.AddPos, moveVec);
		unit.SetMovement(MovementType.AddPos, moveVec);

	}

	public override void Exit() {
		box.isPushingMode = false;
	}
}