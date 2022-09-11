using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Henchman01_behavior : EnemyBehavior
{
    protected override void Attack()
    {
        base.Attack();
        ChangeAnimationState(HENCHMAN_MELEE_ATTACK);
    }
}
