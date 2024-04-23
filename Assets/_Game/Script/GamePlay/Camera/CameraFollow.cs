
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float speed = 20;
    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        //target = player.transform;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
    }
}

