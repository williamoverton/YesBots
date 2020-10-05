using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBoiEnemy : Enemy
{
    public EnemyBullet bulletPrefab;

    void Update()
    {
        if (isAlive && player)
        {
            transform.LookAt(player.transform.position);
            DoShootAction();
        }

    }

    private void DoShootAction()
    {
        if (lastShotTime + shootSpeed < Time.time)
        {   
            lastShotTime = Time.time;
            if (player.isAlive && CanSeePlayer())
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        Vector3 bulletSpawnPos = transform.position + (transform.rotation * Vector3.forward);

        if(IsPointInWall(bulletSpawnPos)){
            return;
        }

        EnemyBullet bullet = Instantiate(bulletPrefab, bulletSpawnPos, transform.rotation);

        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1000f);
    }
}
