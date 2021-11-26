using UnityEngine;
using UnityEngine.AI;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using Pada1.BBCore.Framework;

[Action("MyActions/Move To Spawn")]
[Help("Moves Tank to Spawn")]

public class MoveToSpawn : BasePrimitiveAction
{
    [InParam("myself")]
    public GameObject myself;

    [InParam("spawn")]
    public Vector3 spawn;
    public override void OnStart()
    {
        myself.GetComponent<TankMovement>().Seek(spawn);
    }

    public override TaskStatus OnUpdate()
    {
        NavMeshAgent navAgent = myself.GetComponent<NavMeshAgent>();
        if (!navAgent.pathPending && Vector3.Distance(navAgent.nextPosition, navAgent.pathEndPosition) <= 1.0f)
        {
            return TaskStatus.COMPLETED;
        }

        return TaskStatus.RUNNING;
    }
}