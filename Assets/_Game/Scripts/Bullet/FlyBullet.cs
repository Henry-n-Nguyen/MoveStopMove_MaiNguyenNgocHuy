using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBullet : Bullet
{
    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void Launch()
    {
        base.Launch();

        float distance = speed * Time.deltaTime;

        bulletTransform.position += bulletTransform.forward * distance;
        attackRange -= distance;

        if (attackRange <= 0) Despawn();
    }
}
