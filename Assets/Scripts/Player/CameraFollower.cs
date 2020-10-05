using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform followTransform;

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, followTransform.position, 0.03f);
    }
}
