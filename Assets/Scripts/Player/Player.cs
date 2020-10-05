using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Player : MonoBehaviour
{
    Rigidbody rigidBody;
    public Bullet bulletPrefab;

    bool shotHand = false;
    public Transform leftHand;
    public Transform rightHand;
    public GlitchEffect glitchEffect;
    private TextMesh healthText;
    private TextMesh timeText;
    public int health = 100;
    public Color healthLowColor;
    private Color healthStartColor;
    public int healthLowThreshold = 50;
    private Light[] lights;
    public bool isAlive = true;
    private LayerMask hurtLayerMask;
    private LayerMask worldLayer;
    private float lastShotTime;
    public float shootDelay = 0.2f;
    private AudioSource audioSource;
    public AudioClip hurtSound;
    public AudioClip deathSound;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {

        healthText = GameObject.Find("Health Text").GetComponent<TextMesh>();
        timeText = GameObject.Find("Time Text").GetComponent<TextMesh>();
        startTime = Time.time;

        hurtLayerMask = LayerMask.NameToLayer("Ouchy Oof");
        worldLayer = LayerMask.NameToLayer("World");
        lights = GetComponentsInChildren<Light>();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(DisplayGlitchAffect());

        healthStartColor = healthText.color;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == hurtLayerMask)
        {
            Die();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (StateManager.paused) return;
        if (isAlive) DoInteraction();
        UpdateUI();
    }

    void FixedUpdate()
    {
        if (isAlive) DoMovement();
    }

    public void Damage(int damage)
    {
        if (!isAlive) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }else{
            audioSource.PlayOneShot(hurtSound, StateManager.volume);
            CameraShaker.Instance.ShakeOnce(3f, 4f, 0.1f, 0.3f);
        }
    }

    public void Die()
    {
        health = 0;
        isAlive = false;

        audioSource.PlayOneShot(deathSound, StateManager.volume);
        CameraShaker.Instance.ShakeOnce(13f, 10f, 0.01f, 0.5f);

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        GetComponent<Rigidbody>().AddForce(Random.rotation * Vector3.up * 500f);
        GetComponent<Rigidbody>().AddForce(Vector3.up * 300f);
        GetComponent<Rigidbody>().AddRelativeTorque((Random.rotation * Vector3.up).normalized * 40f);

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = 0f;
        }

        MeshCollider[] bodyParts = GetComponentsInChildren<MeshCollider>();
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].transform.SetParent(null);
            bodyParts[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        StartCoroutine(DisplayGlitchAffect());
        StartCoroutine(GotoDeathScene());

        Time.timeScale = 0.2f;
    }

    private void UpdateUI()
    {
        float distance = 2f;
        float frustumHeight = 2.0f * distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustumWidth = frustumHeight * Camera.main.aspect;

        healthText.text = $"HEALTH: {health}";
        healthText.transform.localPosition = ((Camera.main.transform.rotation * Camera.main.transform.up).normalized * distance) + new Vector3(-(frustumWidth / 2f) + 0.2f, (frustumHeight / 2f) - 0.1f, 0);

        timeText.transform.localPosition = ((Camera.main.transform.rotation * Camera.main.transform.up).normalized * distance) + new Vector3(-(frustumWidth / 2f) + 0.2f, (frustumHeight / 2f) - 0.23f, 0);

        if (health <= healthLowThreshold)
        {
            healthText.color = healthLowColor;
        }
        else
        {
            healthText.color = healthStartColor;
        }

        if(isAlive){
            timeText.text = $"Time: {System.Math.Round(Time.time - startTime, 2)}";
        }
    }

    private void DoInteraction()
    {
        DoRotation();
        DoShooting();
    }

    private void DoShooting()
    {
        if (Input.GetMouseButton(0))
        {

            if (lastShotTime + shootDelay > Time.time)
            {
                return;
            }

            lastShotTime = Time.time;

            Vector3 pos = leftHand.position;

            if (shotHand)
            {
                pos = rightHand.position;
            }

            shotHand = !shotHand;

            if (IsPointInWall(pos))
            {
                return;
            }

            Bullet bullet = Instantiate(bulletPrefab, pos, transform.rotation);

            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 2000f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(leftHand.position, 0.1f);
        Gizmos.DrawSphere(rightHand.position, 0.1f);
    }

    public bool IsPointInWall(Vector3 pos)
    {
        return Physics.CheckSphere(pos, 0.4f, 1 << worldLayer.value);
    }

    private void DoRotation()
    {
        Vector3 mousePos = Vector3.forward;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        Plane plane = new Plane(Vector3.up, 0);
        if (plane.Raycast(ray, out distance))
        {
            mousePos = ray.GetPoint(distance);
        }

        transform.LookAt(new Vector3(mousePos.x, transform.position.y, mousePos.z));
    }

    private void DoMovement()
    {
        float power = 2500;

        Vector2 movement = new Vector2();

        if (Input.GetKey("w"))
        {
            movement += new Vector2(0, 1);
        }
        if (Input.GetKey("s"))
        {
            movement += new Vector2(0, -1);
        }
        if (Input.GetKey("a"))
        {
            movement += new Vector2(-1, 0);
        }
        if (Input.GetKey("d"))
        {
            movement += new Vector2(1, 0);
        }

        rigidBody.AddForce(new Vector3(movement.x, 0, movement.y).normalized * power * Time.deltaTime);

        // transform.rotation = Quaternion.LookRotation(rigidBody.velocity.normalized);
    }

    IEnumerator DisplayGlitchAffect()
    {

        glitchEffect.intensity = 1;
        glitchEffect.flipIntensity = 1;
        glitchEffect.colorIntensity = 1;

        yield return new WaitForSeconds(0.3f);

        glitchEffect.colorIntensity = 0;
        glitchEffect.flipIntensity = 0;
        glitchEffect.intensity = 0;

        yield return null;
    }

    IEnumerator GotoDeathScene()
    {
        yield return new WaitForSeconds(0.3f);

        if (!StateManager.paused) Time.timeScale = 1f;
        GameObject.FindObjectOfType<GameSceneManager>().LoadDeathScene();

        yield return null;
    }
}
