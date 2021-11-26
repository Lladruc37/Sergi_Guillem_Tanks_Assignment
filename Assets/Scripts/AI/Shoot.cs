using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using Pada1.BBCore.Framework;

[Action("MyActions/shoot action")]
[Help("Shoots the target.")]
public class ShootAction : BasePrimitiveAction
{
	[InParam("myself")]
	public GameObject myself;
	public override TaskStatus OnUpdate()
	{
		if (myself.GetComponent<TankShooting>().ammo > 0)
		{
			myself.GetComponent<TankShooting>().Fire();
		}
		return TaskStatus.COMPLETED;
	}
}
