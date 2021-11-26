using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using Pada1.BBCore.Framework;

[Action("MyActions/Wander or Patrol")]
[Help("Default movement for the tank")]
public class WandoerOrPatrol : BasePrimitiveAction
{
	[InParam("myself")]
	public GameObject myself;
	public override TaskStatus OnUpdate()
	{
		TankMovement movement = myself.GetComponent<TankMovement>();
        switch (movement.m_PlayerNumber)
        {
            case 1:
                {
                    movement.Pursue(movement.patrolTarget);
                    break;
                }
            case 2:
                {
                    movement.Wander();
                    break;
                }
            default:
                break;
        }
        return TaskStatus.COMPLETED;
	}
}
