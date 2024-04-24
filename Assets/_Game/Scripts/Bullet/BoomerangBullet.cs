using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBullet : AbstractBullet
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

        float distance = speed * Time.deltaTime;

        if (attackRange > 0)
        {
            bulletTransform.position += bulletTransform.forward * distance;
            attackRange -= distance;
        }
        else if (attackRange <= 0)
        {
            float gap = Vector3.Distance(owner.transform.position, bulletTransform.position); 
            if (gap > 0.1f)
            {
                // Return faster than before 50%
                bulletTransform.position += (owner.transform.position - bulletTransform.position).normalized * distance * 1.5f;
                attackRange -= distance;
            } 
            else
            {
                Despawn();
            }
        }
    }
}
