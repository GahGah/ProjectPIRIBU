using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType {
	Ladder,PushBox
}
public class InteractionObject : MonoBehaviour
{
	[HideInInspector] public InteractionType type;
	[HideInInspector] public HingeJoint2D joint;
}
