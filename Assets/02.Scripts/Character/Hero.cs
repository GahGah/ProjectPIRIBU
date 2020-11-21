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
	public void Draw(Color color) {
		Debug.DrawLine(hero.transform.position, hero.transform.position + hero.transform.up, color);
	}
}

//지형 상태
public class HeroGround : HeroState {
	float tick;
	float rayDist;//지형 레이 거리
	Vector2 groundNormal;//지형 노말
	Vector2 groundForward;//GroundNormal의 직교벡터
	public override void Enter() {
		tick = 0f;
		charStat.verticalSpeed = 0;
		groundNormal = Vector2.up;
	}
	public override void Execute() {
		Draw(Color.green);

		tick += Time.deltaTime;

		//지형 부착
		if (hero.unit.RayAttachGround()) {
			groundForward = hero.unit.groundForward;
		} else {
			//땅과 거리차가 날시 공중상태
			sm.SetState(States.Hero_Air);
			return;
		}

		//좌우이동
		int moveDir = 0;
		if (input.buttonLeft.isPressed) moveDir = -1;
		if (input.buttonRight.isPressed) moveDir = 1;
		hero.unit.HandleMoveSpeed(moveDir, charStat.groundMoveSpeed);

		
		//이동호출
		Vector2 vel = groundForward * charStat.sideMoveSpeed;
		//Vector2 vel = new Vector2(charStat.sideMoveSpeed, 0);
		hero.unit.SetMovement(MovementType.SetVelocity, vel);

		if (input.buttonJump.isPressed) {
			sm.SetState(States.Hero_Jump);
			return;
		}

		//상호작용 오브젝트
		List<InteractionObject> interactions = hero.unit.sensor.InteractionObjects;
		foreach (InteractionObject interaction in interactions) {
			switch (interaction.type) {
				//사다리
				case InteractionType.Ladder:
					if ((input.buttonUp.isPressed || input.buttonDown.isPressed)//위아래 키를 누를때만
						&& hero.unit.GetDistanceToLadder(interaction) <= 1.5f //사다리 근처에 있을때만
						&& hero.unit.IsInLadder(0, interaction)//사다리 범위 내에 있을 시에만
						) {
						hero.unit.interactionObject = interaction;
						sm.SetState(States.Hero_Ladder);
						return;
					}
					break;

				//박스 밀기
				case InteractionType.PushBox:
					break;
			}
		}
	}

}

public class HeroJump : HeroState {
	public override void Enter() {
		hero.unit.ResetLiftParent();
		hero.unit.foot.adjacentlinearPlatforms.Clear();
		charStat.verticalSpeed = charStat.jumpSpeed;
		sm.SetState(States.Hero_Air);
	}
	public override void Execute() {
	}
}

//공중 상태
public class HeroAir : HeroState {
	public override void Enter() {
	}
	public override void Execute() {
		Draw(Color.yellow);
		//좌우이동
		int moveDir = 0;
		if (input.buttonLeft.isPressed) moveDir = -1;
		if (input.buttonRight.isPressed) moveDir = 1;
		hero.unit.HandleMoveSpeed(moveDir, charStat.airMoveSpeed);

		//추락
		charStat.verticalSpeed -= charStat.fallSpeed;

		//이동호출
		Vector2 vel = new Vector2(charStat.sideMoveSpeed, charStat.verticalSpeed);
		hero.unit.SetMovement(MovementType.SetVelocity, vel);

		//지형 부착
		float dist = hero.unit.RayGround(Vector2.down);
		float groundYSpeed = 0;
		if (hero.unit.raycastHitGround.rigidbody) {
			groundYSpeed = hero.unit.raycastHitGround.rigidbody.velocity.y;
		}

		if (dist < hero.unit.groundDist
			&& charStat.verticalSpeed-groundYSpeed < 0) {
			//추락할때만 땅에 붙게 (지형 속도도 고려)
			sm.SetState(States.Hero_Ground);
		}

		//상호작용 오브젝트
		List<InteractionObject> interactions = hero.unit.sensor.InteractionObjects;
		foreach (InteractionObject interaction in interactions) {
			switch (interaction.type) {
				//사다리
				case InteractionType.Ladder:
					if ((input.buttonUp.isPressed || input.buttonDown.isPressed)//위아래 키를 누를때만
						&& hero.unit.GetDistanceToLadder(interaction) <= 1.5f //사다리 근처에 있을때만
						&& hero.unit.IsInLadder(0,interaction)//사다리 범위 내에 있을 시에만
						) {
							hero.unit.interactionObject = interaction;
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

//사다리 액션
public class HeroLadder : HeroState {
	InteractionObject ladder;
	HingeJoint2D joint;
	float attachSpeed;//사다리에 들러붙는 가중치
	float time;//바로 점프 못하게 딜레이
	public override void Enter() {
		ladder = hero.unit.interactionObject;
		joint = ladder.gameObject.AddComponent<HingeJoint2D>();
		joint.connectedBody = hero.unit.rigid;
		joint.autoConfigureConnectedAnchor = false;
		
		//사다리 탈땐 OneWay 무시
		hero.unit.foot.gameObject.SetActive(false);

		charStat.sideMoveSpeed = 0;
		charStat.verticalSpeed = 0;

		attachSpeed = 0.1f;

		time = 0;
	}

	public override void Execute() {
		//사다리에 무게싣기 (Logic Error : 이렇게 구현하면 안된다)
		hero.unit.SetMovement(MovementType.AddVelocity, Vector2.down*charStat.fallSpeed*5);

		
		Vector3 newAnchor = joint.connectedAnchor;
		
		//사다리에 달라붙기
		Vector3 dirToLadder = hero.unit.GetDirectionToLadder();
		newAnchor -= dirToLadder*attachSpeed;
		attachSpeed += (1-attachSpeed)*0.1f;


		Vector3 ladderDir = ladder.transform.up;
		//위아래 이동
		if (input.buttonUp.isPressed) {
			newAnchor -= ladderDir*0.15f;
		}
		if (input.buttonDown.isPressed) {
			newAnchor += ladderDir*0.15f;
		}
		joint.connectedAnchor = newAnchor;

		//사다리를 벗어났는가?
		if (!hero.unit.IsInLadder(0.8f)) {
			sm.SetState(States.Hero_Air);
			return;
		}


		//점프로 끊기
		time += Time.deltaTime;
		if (input.buttonJump.isPressed && time > 0.5f) {
			sm.SetState(States.Hero_Jump);
			return;
		}
	}
	public override void Exit() {
		hero.unit.foot.gameObject.SetActive(true);
		BreakJoint();
	}

	void BreakJoint() {
		if (joint) {
			joint.breakForce = 0;//알아서 컴포넌트 삭제됨
			joint = null;
		}
	}
}
public class HeroBox : HeroState {
	public override void Enter() {
	}
}