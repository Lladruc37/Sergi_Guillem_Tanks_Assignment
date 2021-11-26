using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Framework;

[Condition("MyConditions/Has ammo?")]
[Help("Checks whether tank has ammo.")]

public class HasAmmo : ConditionBase
{
	[InParam("myself")]
	public GameObject myself;
    public override bool Check()
	{
		int ammo = myself.GetComponent<TankShooting>().ammo;
		return ammo <= 0;
	}
}
