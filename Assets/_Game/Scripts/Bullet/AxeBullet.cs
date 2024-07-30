using HuySpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeBullet : AbstractBullet
{
    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void Throw()
    {
        base.Throw();

        // Spin bullet
        meshTransform.rotation = Quaternion.Euler(meshTransform.rotation.eulerAngles + Vector3.back * 180f * speed * Time.deltaTime);

        // Throw bullet
        float distance = speed * Time.deltaTime;

        bulletTransform.position += bulletTransform.forward * distance;
        attackRange -= distance;

        if (attackRange < 0)
        {
            SimplePool.Spawn<PickUpBullet>((PoolType)poolType + 79, bulletTransform.position, Quaternion.identity);
            SimplePool.Despawn(this);
        }
    }
}
