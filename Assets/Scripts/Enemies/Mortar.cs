using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : MonoBehaviour
{
    private float startTime;
    public float lifespan = 2f;
    public GameObject explosion;
    public GameObject ball;
    private bool hasExploded = false;
    public AudioClip explosionSound;
    public AudioClip fireSound;
    public AudioClip landSound;
    private AudioSource audioSource;
    private bool hasLandedYet = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(fireSound, StateManager.volume);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(StateManager.paused) return;
        if (startTime + lifespan < Time.time)
        {
            Explode();
        }
    }

    private void OnCollisionEnter(Collision collider)
    {
        Player player = collider.gameObject.GetComponent<Player>();

        if (player)
        {
            Explode();
            return;
        }

        if (!hasLandedYet)
        {
            hasLandedYet = true;
            audioSource.PlayOneShot(landSound, StateManager.volume);
        }
    }


    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        audioSource.PlayOneShot(explosionSound, StateManager.volume);

        Player player = GameObject.FindObjectOfType<Player>();

        if (player)
        {
            float explosionPower = 100f;
            float radius = 4.5f;

            if (Vector3.Distance(player.transform.position, transform.position) <= radius)
            {
                player.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, transform.position, radius, 1, ForceMode.Impulse);
                player.Damage(Random.Range(15, 30));
            }
        }

        explosion.SetActive(true);
        ball.SetActive(false);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        StartCoroutine(Cleanup());
    }

    private IEnumerator Cleanup()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
