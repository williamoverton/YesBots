using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoiEnemy : Enemy
{
    public Mortar bulletPrefab;
    public Transform shootPoint;
    public AudioClip detectionSound;
    private AudioSource audioSource;
    public float detectionRadius = 8f;
    private bool detectedPlayer = false;
    public GameObject mortarTargetEffect;

    public override void Start()
    {
        base.Start();

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(StateManager.paused) return;
        if (isAlive && player)
        {
            if (detectedPlayer)
            {
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                DoShootAction();
            }
            else
            {
                if(Vector3.Distance(transform.position, player.transform.position) < detectionRadius){
                    detectedPlayer = true;
                    audioSource.PlayOneShot(detectionSound, StateManager.volume * 1.5f);
                    lastShotTime = Time.time - (shootSpeed / 2f);
                }
            }

        }

    }

    private void DoShootAction()
    {
        if (lastShotTime + shootSpeed < Time.time)
        {
            lastShotTime = Time.time;
            if (player.isAlive)
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        float launchAngle = 60f;

        Vector3 deltaPos = player.transform.position - shootPoint.position;
        Vector3 xzDelta = deltaPos;
        xzDelta.y = 0f;

        Vector3 shotDir = Quaternion.LookRotation(xzDelta) * Quaternion.AngleAxis(-launchAngle, Vector3.right) * Vector3.forward;
        float time = Mathf.Sqrt((shotDir.y * deltaPos.x / shotDir.x - deltaPos.y) / -Physics.gravity.y * 2);
        float vel = deltaPos.x / shotDir.x / time;

        if (float.IsNaN(vel))
        {
            Debug.Log("Impossible Trajectory");
        }

        Mortar bullet = Instantiate(bulletPrefab, shootPoint.position + (shootPoint.rotation * Vector3.forward), shootPoint.rotation);

        bullet.GetComponent<Rigidbody>().velocity = vel * shotDir;

        // Spawn Mortar target
        Instantiate(mortarTargetEffect, player.transform.position, Quaternion.identity);
    }
}
