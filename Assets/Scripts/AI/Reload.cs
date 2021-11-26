using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using Pada1.BBCore.Framework;

[Action("MyActions/Reload")]
[Help("Reloads ammo")]

public class Reload : BasePrimitiveAction
{
    [InParam("myself")]
    public GameObject myself;
    public override TaskStatus OnUpdate()
    {
        myself.GetComponent<TankShooting>().ammo = 5;
        return TaskStatus.COMPLETED;
    }
}
