using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBullet : AbstractBullet
{
    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void Launch()
    {
        base.Launch();

        // Spin bullet
        meshTransform.rotation = Quaternion.Euler(meshTransform.rotation.eulerAngles + Vector3.down * 180f * speed * Time.deltaTime);

        // Throw bullet
        float distance = speed * Time.deltaTime;

        bulletTransform.position += bulletTransform.forward * distance;
        attackRange -= distance;

        if (attackRange <= 0) Despawn();
    }
}
