using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player player;
    public bool isAlive = true;
    public float wiggleAmount = 0.002f;
    public float wiggleSpeed = 0.05f;
    public int health = 100;
    public float wiggleOffset = 0f;
    public Light[] lights;
    public float shootSpeed = 1f;
    public float lastShotTime = 0;
    private LayerMask worldLayer;
    private LayerMask hurtLayerMask;

    public virtual void Start()
    {
        lastShotTime = Time.time + Random.Range(0, shootSpeed);
        player = GameObject.FindObjectOfType<Player>();

        worldLayer = LayerMask.NameToLayer("World");
        hurtLayerMask = LayerMask.NameToLayer("Ouchy Oof");

        lights = GetComponentsInChildren<Light>();
        
        wiggleOffset = Random.Range(0f, 9999f);
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == hurtLayerMask)
        {
            Die(transform.position);
        }
    }

    void FixedUpdate()
    {
        if(StateManager.paused) return;
        if (isAlive)
        {
            wiggleOffset += wiggleSpeed;
            transform.position = new Vector3(transform.position.x + Mathf.Cos(wiggleOffset) * wiggleAmount * 0.1f, transform.position.y + Mathf.Sin(wiggleOffset) * wiggleAmount * 0.1f, transform.position.z + Mathf.Sin(wiggleOffset + 0.5f) * wiggleAmount * 0.1f);
        }
    }

    public bool CanSeePlayer()
    {
        RaycastHit hit;

        if (Physics.Linecast(transform.position + (transform.rotation * Vector3.forward), player.transform.position, out hit))
        {
            if (hit.collider.gameObject == player.gameObject)
            {
                return true;
            }
        }

        return false;
    }

    public void Damage(int amount, Vector3 from)
    {
        health -= amount;

        if (health <= 0)
        {
            Die(from);
        }
        else
        {
            wiggleSpeed *= 1.25f;
            wiggleAmount *= 1.1f;
        }
    }

    public virtual void Die(Vector3 from)
    {
        isAlive = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        if (from != null && from != Vector3.zero)
        {
            GetComponent<Rigidbody>().AddForce((this.transform.position - from).normalized * 1000f);
            GetComponent<Rigidbody>().AddForce(Vector3.up * 800f);
            GetComponent<Rigidbody>().AddRelativeTorque((Random.rotation * Vector3.up).normalized * 1000f);
            GetComponent<Rigidbody>().drag = 0.2f;
        }

        if(GetComponentInChildren<ParticleSystem>()) GetComponentInChildren<ParticleSystem>().Stop();

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = 0f;
        }
    }

    public bool IsPointInWall(Vector3 pos)
    {
        return Physics.CheckSphere(pos, 0.3f, 1 << worldLayer.value);
    }
}
