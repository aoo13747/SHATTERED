using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Henchman02_behavior : EnemyBehavior
{
    protected override void Shoot()
    {
        base.Shoot();
        ChangeAnimationState(HENCHMAN_PISTOL_SHOOT);
    }
}
