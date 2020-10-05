using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SplodyEnemy : Enemy
{
    private NavMeshAgent agent;
    public float detectionDistance = 10f;
    public float boomDistance = 2f;
    private Transform currentRoamDest;
    public Transform[] roamTargets;
    private bool exploded = false;
    public GameObject explosion;
    public GameObject spawnEffect;
    public GameObject body;
    public AudioClip detectionSound;
    public AudioClip explosionSound;
    private AudioSource audioSource;
    private bool isAfterPlayer = false;
    public bool youHaveActivatedMyTrapCard = false;
    private bool trapCardActivated = false;
    private CapsuleCollider myCollider;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        myCollider = GetComponent<CapsuleCollider>();

        if (youHaveActivatedMyTrapCard)
        {
            Hide();
        }

        GetComponent<NavMeshAgent>().avoidancePriority = Random.Range(0, 99);
    }

    public void Hide()
    {
        body.SetActive(false);
        myCollider.isTrigger = true;
    }

    public void UnHide()
    {
        myCollider.isTrigger = false;
        trapCardActivated = true;
        body.SetActive(true);
        spawnEffect.SetActive(true);
        Debug.Log("Unhiding!");
    }

    void Update()
    {
        if(StateManager.paused) return;
        if (isAlive && player && !exploded)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if ((youHaveActivatedMyTrapCard && trapCardActivated) || !youHaveActivatedMyTrapCard)
            {
                if (distance < boomDistance)
                {
                    Explode();
                    audioSource.PlayOneShot(explosionSound, StateManager.volume);
                    return;
                }

                if (distance < detectionDistance)
                {
                    agent.SetDestination(player.transform.position);

                    if (!isAfterPlayer)
                    {
                        isAfterPlayer = true;
                        audioSource.PlayOneShot(detectionSound, StateManager.volume);
                    }

                }
                else
                {
                    isAfterPlayer = false;
                    Roam();
                }
            }else{
                if (distance < detectionDistance)
                {
                    UnHide();
                }
            }

        }
    }

    void Roam()
    {
        if (roamTargets != null && roamTargets.Length > 0)
        {
            // if is done moving to target
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                currentRoamDest = roamTargets[Mathf.FloorToInt(Random.Range(0, roamTargets.Length))];
                agent.SetDestination(currentRoamDest.position);
            }
        }
    }

    public void Explode()
    {
        if (exploded)
        {
            return;
        }

        exploded = true;
        explosion.SetActive(true);
        Destroy(GetComponent<NavMeshAgent>());
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<CapsuleCollider>());
        Destroy(body);

        if (player)
        {
            float explosionPower = 100f;
            float radius = 3f;

            if (Vector3.Distance(player.transform.position, transform.position) <= radius)
            {
                player.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, transform.position, radius, 1, ForceMode.Impulse);
                player.Damage(Random.Range(50, 70));
            }
        }
    }

    public override void Die(Vector3 from)
    {
        isAlive = false;
        Explode();
    }
}
