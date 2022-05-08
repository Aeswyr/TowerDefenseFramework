using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhNarwahl.pH;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Tower : MonoBehaviour, HasPh {
    public enum TargetStrategy {
        LowestPH,
        HighestPH,
        First,
        Last,
    }

    // The tower's state of readiness
    private enum State {
        Armed,
        Reloading
    }

    [SerializeField] private TowerData m_towerData;
    [SerializeField] private Oncomer.Type[] m_oncomerTargetTypes;
    [SerializeField] private Tower.TargetStrategy m_targetStrategy;
    [SerializeField] private bool ignoreFullOncomers;
    [SerializeField] private float shootSpeed = 1f;
    [SerializeField] private float radius = 3f;
    [SerializeField] private string projectileSoundID = "projectile-default";
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectilePh;
    private float projectileVolume;

    [SerializeField] private CircleCollider2D m_radiusCollider;

    private int m_cost;

    private float reloadTimer = 0f;
    private List<GameObject> m_targets;
    private State currState;
    private AudioSource m_audioSrc;

    public GameObject Tracer;

    private AudioData m_audioData;

    private const float CELL_OFFSET = 0.5f;

    public float getPH() {
        return projectilePh;
    }

    // Handles triggers of this tower's radius collider
    public void HandleTriggerEnter2D(Collider2D collider) {
        // when an intruder enters this tower's range, add it to the list of targets if it is targetable by the tower
        if (collider.gameObject.tag == "target") {
            if (CanTarget(collider.gameObject)) {
                m_targets.Add(collider.gameObject);
            }
        }
    }

    public void HandleTriggerExit2D(Collider2D collider) {
        // when an intruder exits this tower's range, remove it from the list of targets
        if (collider.gameObject.tag == "target") {
            m_targets.Remove(collider.gameObject);
        }
    }

    private void Awake() {
        m_targets = new List<GameObject>();
        currState = State.Armed;
        m_radiusCollider.radius = this.radius;
        m_audioSrc = this.GetComponent<AudioSource>();
    }

    void FixedUpdate() {
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
                if (m_targets.Count == 0) {
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
        // TODO: check for nexus
        GameObject chosenTarget = ChooseTarget(transform.position);
        if (chosenTarget == null) {
            return;
        }

        // Create projectile
        Vector3 toPos = chosenTarget.transform.position;
        Vector3 shootDir = (toPos - transform.position).normalized;

        // Produce launching sound
        PlayLaunchSound();

        // Instantiate projectile
        GameObject projectileObj = Instantiate(projectilePrefab);
        projectileObj.transform.position = this.transform.position + new Vector3(CELL_OFFSET, CELL_OFFSET, 0);

        // Assign projectile target
        Projectile projectileComp = projectileObj.GetComponent<Projectile>();
        projectileComp.TargetObj = chosenTarget;
        projectileComp.Volume = projectileVolume;
        projectileComp.MolH = pH.getAcidMolarity(projectilePh) * projectileVolume;
        projectileComp.MolOH = pH.getBaseMolarity(projectilePh) * projectileVolume;
        
        Debug.Log("Projectile molOH " + projectileComp.MolOH);
        Debug.Log("Projectile molH " + projectileComp.MolH);

        // tower must now reload
        currState = State.Reloading;
        reloadTimer = shootSpeed;
    }

    private void PlayLaunchSound() {
        if(m_audioData) {
            AudioClip clip = m_audioData.Clip;
            m_audioSrc.volume = m_audioData.Volume;
            m_audioSrc.PlayOneShot(clip);
        }
    }


    private GameObject ChooseTarget(Vector3 Position) {
        // clear enemies which have been destroyed
        m_targets = m_targets.FindAll(t => t != null);

        if (m_targets.Count == 0) {
            return null;
        }
        
        switch (m_targetStrategy) {
            case TargetStrategy.First:
                int lowestSpawnId = int.MaxValue;
                GameObject bestTarget = null;
                foreach (GameObject t in m_targets) {
                    Oncomer oncomer = t.GetComponent<Oncomer>();
                    if (ignoreFullOncomers && oncomer != null && oncomer.IsFull()) {
                        continue;
                    }
                    if (oncomer != null && oncomer.SpawnId < lowestSpawnId) {
                        lowestSpawnId = oncomer.SpawnId;
                        bestTarget = t;
                    }
                }
                return bestTarget;
            case TargetStrategy.Last:
                int highestSpawnId = int.MinValue;
                bestTarget = null;
                foreach (GameObject t in m_targets) {
                    Oncomer oncomer = t.GetComponent<Oncomer>();
                    if (ignoreFullOncomers && oncomer != null && oncomer.IsFull()) {
                        continue;
                    }
                    if (oncomer != null && oncomer.SpawnId > highestSpawnId) {
                        highestSpawnId = oncomer.SpawnId;
                        bestTarget = t;
                    }
                }
                return bestTarget;
            case TargetStrategy.HighestPH:
                float highestPH = 0;
                bestTarget = null;
                foreach (GameObject t in m_targets) {
                    Oncomer oncomer = t.GetComponent<Oncomer>();
                    if (ignoreFullOncomers && oncomer != null && oncomer.IsFull()) {
                        continue;
                    }
                    if (oncomer != null && oncomer.getPH() > highestPH) {
                        highestPH = oncomer.getPH();
                        bestTarget = t;
                    }
                }
                return bestTarget;
            case TargetStrategy.LowestPH:
                float lowestPH = 0;
                bestTarget = null;
                foreach (GameObject t in m_targets) {
                    Oncomer oncomer = t.GetComponent<Oncomer>();
                    if (ignoreFullOncomers && oncomer != null && oncomer.IsFull()) {
                        continue;
                    }
                    if (oncomer != null && oncomer.getPH() < lowestPH) {
                        lowestPH = oncomer.getPH();
                        bestTarget = t;
                    }
                }
                return bestTarget;
        }
        return null;
    }

    void CreateWeaponTracer(Vector3 fromPos, Vector3 toPos, float width) {
        Vector3[] vt = new Vector3[4];
        Vector3 dir = (fromPos - toPos).normalized;
        vt[0] = fromPos + width * new Vector3(dir.y, -dir.x);
        vt[1] = toPos + width * new Vector3(dir.y, -dir.x);
        vt[2] = toPos + width * new Vector3(-dir.y, dir.x);
        vt[3] = fromPos + width * new Vector3(-dir.y, dir.x);

        GameObject tracerObject = Instantiate(Tracer, new Vector3(0, 0, -1), Quaternion.identity);
        tracerObject.GetComponent<Trace>().duration = .1f;
        tracerObject.GetComponent<Trace>().vertices = vt;
    }

    public void ApplyTowerData(TowerData data) {
        this.GetComponent<SpriteRenderer>().sprite = data.Sprite;
        m_targetStrategy = data.TargetStrategy;
        ignoreFullOncomers = data.IgnoreFullOncomers;
        m_oncomerTargetTypes = data.OncomerTargets;
        shootSpeed = data.ShootSpeed;
        radius = data.Radius;

        // TODO: set projectiles with their own data
        projectileSoundID = data.ProjectileSoundID;
        projectilePrefab = data.ProjectilePrefab;
        projectileVolume = data.ProjectileVolume;
        projectilePh = data.ProjectilePh;

        m_cost = data.Cost;

        if (GameDB.instance != null) {
            m_audioData = GameDB.instance.GetAudioData(projectileSoundID);
        }
    }

    private bool CanTarget(GameObject potentialTarget) {
        Oncomer oncomer = potentialTarget.GetComponent<Oncomer>();
        if (oncomer != null) {
            foreach (Oncomer.Type type in m_oncomerTargetTypes) {
                if (oncomer.GetOncomerType() == type) {
                    return true;
                }
            }
        }
        return false;
    }
}