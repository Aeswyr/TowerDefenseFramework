using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesRaycast 
{
    public static void Shoot(Vector3 shootPos, Vector3 shootDir) {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(shootDir, shootPos);
        if (raycastHit2D.collider != null)
        {
            Target target = raycastHit2D.collider.GetComponent<Target>();
            if (target != null)
            {
                target.Damage();
            }
        }
            }
}
