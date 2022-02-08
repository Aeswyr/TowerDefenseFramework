using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour

{
    
    float shootSpeed = 1f;
    float lastshoot = 0f;
    public GameObject Tracer;
    public Target[] targets;
    void Start()
    {
       
    }

    void FixedUpdate()
    {
        if (Time.time - lastshoot > shootSpeed)//shoot per shootSpeed
        {
            Debug.Log("shoot");
            Vector3 toPos = getClosestTarget(transform.position).transform.position;
            Vector3 shootDir = (toPos - transform.position).normalized;
            ProjectilesRaycast.Shoot(transform.position, shootDir);
            //Debug.DrawLine(transform.position, toPos,Color.white,0.1f);
           // Debug.Log(transform.position.ToString());
            CreateWeaponTracer(transform.position, toPos,0.03f);
            lastshoot = Time.time;
        }
    }

    private Target getClosestTarget(Vector3 Position)
    {
        float closestDis = 1000f;
        Target closestTarget = targets[0];
        foreach( Target t in targets){
            if (Vector3.Distance(t.transform.position, Position) < closestDis)
            {
                closestDis = Vector3.Distance(t.transform.position, Position);
                closestTarget = t;
            }
        }
        return closestTarget;
    }
    
    void CreateWeaponTracer(Vector3 fromPos, Vector3 toPos, float width)
    {
        Vector3[] vt = new Vector3[4];
        Vector3 dir = (fromPos - toPos).normalized;
        vt[0] = fromPos + width * new Vector3(dir.y,-dir.x);
        vt[1] = toPos + width * new Vector3(dir.y, -dir.x);
        vt[2] = toPos + width * new Vector3(-dir.y, dir.x);
        vt[3] = fromPos + width * new Vector3(-dir.y, dir.x);
        /*
        Debug.Log(vt[0]);
        Debug.Log(vt[1]);
        Debug.Log(vt[2]);
        Debug.Log(vt[3]);
        */
        GameObject tracerObject = Instantiate(Tracer, new Vector3(0, 0, 0), Quaternion.identity);
        tracerObject.GetComponent<Trace>().duration = .1f;
        tracerObject.GetComponent<Trace>().vertices = vt;
    }
    
    


}
