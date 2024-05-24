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
        meshTransform.rotation = Quaternion.Euler(meshTransform.rotation.eulerAngles + Vector3.down * Time.deltaTime * speed * 180f);

        float distance = speed * Time.deltaTime;

        if (attackRange > 0)
        {
            bulletTransform.position += bulletTransform.forward * distance;
            attackRange -= distance;
        }
        else if (attackRange <= 0)
        {
            float gap = Vector3.Distance(owner.characterTransform.position, bulletTransform.position); 
            if (gap > 0.1f)
            {
                // Return faster than before 50%
                bulletTransform.position += (owner.characterTransform.position - bulletTransform.position).normalized * distance * 1.5f;
                attackRange -= distance;
            } 
            else
            {
                Despawn();
            }
        }
    }
}
