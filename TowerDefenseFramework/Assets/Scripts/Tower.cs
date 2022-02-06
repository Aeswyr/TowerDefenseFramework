using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour

{
    [SerializeField] private Material WeaponTracer;
    float shootSpeed = 1f;
    float lastshoot = 0f;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (Time.time - lastshoot > shootSpeed)//shoot per shootSpeed
        {
            Debug.Log("shoot");
            Vector3 toPos = new Vector3(0, 10f, 1);
            Vector3 shootDir = new Vector3(0, 1f, 0);
            ProjectilesRaycast.Shoot(transform.position, shootDir);
            Debug.DrawLine(transform.position, toPos,Color.white,1f);
            Debug.Log(transform.position.ToString());
            //CreateWeaponTracer(transform.position, toPos);
            lastshoot = Time.time;
        }
    }

    /*
    void CreateWeaponTracer(Vector3 fromPos, Vector3 toPos)
    {
        Vector3 shootDir = (toPos - fromPos).normalized;
        float eulerZ = Vector3.Angle(shootDir,new Vector3(0,1,0));
        float distance = Vector3.Distance(toPos, fromPos);
        
    }
    */


}
