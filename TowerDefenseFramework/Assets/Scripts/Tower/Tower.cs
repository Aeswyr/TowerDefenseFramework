using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Tower : MonoBehaviour
{
    // The tower's state of readiness
    private enum State {
        Armed,
        Reloading
    }

    [SerializeField]
    private float shootSpeed = 1f;
    [SerializeField]
    private float radius = 3f;

    private float reloadTimer = 0f;
    private List<Oncomer> targets;
    private State currState;
    private CircleCollider2D m_collider;


    public GameObject Tracer;

    private void OnTriggerEnter2D(Collider2D collider) {
        // when an intruder enters this tower's range, add it to the list of targets
        if (collider.gameObject.tag == "target") {
            Oncomer target = collider.gameObject.GetComponent<Oncomer>();
            targets.Add(target);
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        // when an intruder enters this tower's range, add it to the list of targets
        if (collider.gameObject.tag == "target") {
            Oncomer target = collider.gameObject.GetComponent<Oncomer>();
            targets.Remove(target);
        }
    }

    private void Awake() {
        targets = new List<Oncomer>();
        currState = State.Armed;
        m_collider = GetComponent<CircleCollider2D>();
        m_collider.radius = this.radius;
    }

    void FixedUpdate()
    {
        // perform actions according to tower's state
        switch (currState) {
            case State.Reloading:
                // run down the reload timer
                reloadTimer -= Time.deltaTime;
                if (reloadTimer <= 0) {
                    // tower has re-armed itself
                    currState = State.Armed;
                }
                break;
            case State.Armed:
                // search for target to shoot at
                if (targets.Count == 0) {
                    // no targets means no shooting
                    return;
                }
                Shoot();
                break;
            default:
                break;
        }

    }

    private void Shoot() {
        Oncomer chosenTarget = getClosestTarget(transform.position);
        if (chosenTarget == null) {
            return;
        }

        // Create projectile
        Vector3 targetColliderOffset = new Vector3(0.5f, 0.5f, 0);
        Vector3 toPos = chosenTarget.transform.position + targetColliderOffset;
        Vector3 shootDir = (toPos - transform.position).normalized;

        // Produce launching sound
        AudioManager.instance.PlayOneShot("projectile1");
        
        // TODO: define each tower's effects within TowerData and pass to projectile
        ProjectilesRaycast.Shoot(transform.position, shootDir);

        // inserted here because ProjectilesRaycast needs tweaking to filter out tile layers
        chosenTarget.ApplyProjectileEffects();

        // Debug/Display projectile
        // Debug.DrawLine(transform.position, toPos,Color.white,0.1f);
        // Debug.Log(transform.position.ToString());
        Vector3 tracerOrigin = new Vector3(this.transform.position.x + m_collider.offset.x,
            this.transform.position.y + m_collider.offset.y,
            this.transform.position.z);
        CreateWeaponTracer(tracerOrigin, toPos, 0.03f);

        // TODO: Define what happens to target on hit

        // tower must now reload
        currState = State.Reloading;
        reloadTimer = shootSpeed;
    }

    // Note: currently gets the target which first entered the tower's radius
    private Oncomer getClosestTarget(Vector3 Position)
    {
        if (targets == null) {
            return null;
        }

        float closestDis = 1000f;
        Oncomer closestTarget = targets[0];
        foreach( Oncomer t in targets){
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
        GameObject tracerObject = Instantiate(Tracer, new Vector3(0, 0, -1), Quaternion.identity);
        tracerObject.GetComponent<Trace>().duration = .1f;
        tracerObject.GetComponent<Trace>().vertices = vt;
    }
    
    


}
