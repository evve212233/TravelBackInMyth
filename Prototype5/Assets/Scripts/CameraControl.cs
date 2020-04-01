using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject rabbit;
    public Transform target;
    private Vector3 offset;
    // Use this for initialization
    void Start() {
        target = rabbit.transform;
        offset = target.position - this.transform.position;
    }
    // Update is called once per frame
    void Update() {
        target = rabbit.transform;
        this.transform.position = target.position - offset;
    }
}
