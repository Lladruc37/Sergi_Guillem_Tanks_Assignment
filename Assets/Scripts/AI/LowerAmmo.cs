using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using Pada1.BBCore.Framework;

[Action("MyActions/Lower Ammo")]
[Help("Decreases ammo by one")]

public class LowerAmmo : BasePrimitiveAction
{
    [InParam("myself")]
    public GameObject myself;
    public override TaskStatus OnUpdate()
    {
        myself.GetComponent<TankShooting>().ammo--;
        return TaskStatus.COMPLETED;
    }
}
