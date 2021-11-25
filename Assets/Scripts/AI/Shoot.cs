using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Framework;

[Condition("MyConditions/shoot action")]
[Help("Shoots the target.")]
public class ShootAction : BasePrimitiveAction
{
	[InParam("myself")]
	public GameObject myself;
	public override void OnStart()
	{
		myself.GetComponent<TankShooting>().Fire();
	}
}
