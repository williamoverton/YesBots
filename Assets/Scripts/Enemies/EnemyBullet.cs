using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float startTime;
    public float lifespan = 5f;
    private int bounceLimit = 3;
    public GameObject explosion;
    public GameObject ball;
    private AudioSource audioSource;
    public AudioClip explosionSound;
    public AudioClip fireSound;

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
            Remove(false);
        }
    }

    private void OnCollisionEnter(Collision collider)
    {
        Player player = collider.gameObject.GetComponent<Player>();

        if (player)
        {
            player.Damage(Random.Range(10, 15));
            audioSource.PlayOneShot(explosionSound, StateManager.volume);
            Remove(true);
        }
        else if (bounceLimit <= 0)
        {
            Remove(false);
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("World"))
        {
            bounceLimit--;
        }
    }

    private void Remove(bool explode)
    {
        if(explode) explosion.SetActive(true);
        ball.SetActive(false);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        StartCoroutine(Cleanup());
    }

    private IEnumerator Cleanup(){
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
