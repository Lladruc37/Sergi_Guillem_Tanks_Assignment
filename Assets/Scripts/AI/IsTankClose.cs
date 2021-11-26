using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Framework;

[Condition("MyConditions/Is Tank Close?")]
[Help("Checks wether the tank is close to spawn or not")]

public class IsTankClose : ConditionBase
{
    [InParam("myself")]
    public GameObject myself;
    [InParam("spawn")]
    public Vector3 spawn;
    public override bool Check()
    {
        return 1 > Vector3.Distance(myself.transform.position, spawn);
    }
}
