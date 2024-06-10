
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    public Transform target;
    public Vector3 offset;
    public Vector3 offsetRotation;
    public float speed = 20;
    public Camera Camera;
    //void Start()
    //{
    //    OnInit();
    //}
    private void Awake()
    {
        Camera = Camera.main;
    }

    public void OnInit()
    {
        transform.position = target.position + offset;
        transform.rotation = Quaternion.Euler( offsetRotation);
        
    }
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
        transform.rotation = Quaternion.Euler( offsetRotation); 
    }
    public void SetOffset(float x, float y, float z)
    {
        Vector3 offset = new Vector3 (x, y, z);
        this.offset = offset;
    }
    public void SetRotation(float x, float y, float z)
    {
        Vector3 offsetR = new Vector3 (x, y, z);
        this.offsetRotation = offsetR;
    }
}

