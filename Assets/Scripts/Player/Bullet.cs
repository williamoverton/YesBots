using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float startTime;
    public float lifespan = 4f;
    private int bounceLimit = 1;
    
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
        if (startTime + lifespan < Time.time)
        {
            Remove(false);
        }
    }

    private void OnCollisionEnter(Collision collider)
    {
        Enemy enemy = collider.gameObject.GetComponent<Enemy>();

        if (enemy)
        {
            enemy.Damage(50, transform.position);
            audioSource.PlayOneShot(explosionSound, StateManager.volume);
            Remove(true);
        }
        else if (bounceLimit <= 0)
        {
            Remove(false);
        }

        if(collider.gameObject.layer == LayerMask.NameToLayer("World")){
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
