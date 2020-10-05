using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatoExpress : MonoBehaviour
{
    public Transform rotatingPart;
    public float rotationSpeed = 20f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        rotatingPart.Rotate(0, rotationSpeed * Time.deltaTime, 0); 
    }
}
