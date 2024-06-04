using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level : MonoBehaviour 
{
    [SerializeField] public Transform centerPosition;
    [SerializeField] public Transform minPoint;
    [SerializeField] public Transform maxPoint;
    public int realBot = 10;
    public int maxBot = 20;
    public int maxCoin = 200;
    public Vector3 RandomPoint()
    {
        Vector3 randPoint = Random.Range(minPoint.position.x, maxPoint.position.x) * Vector3.right + Random.Range(minPoint.position.z, maxPoint.position.z) * Vector3.forward;

        NavMeshHit hit;

        // dung de xac dinh vi tri random hop le cho navmesh. truong hop ma random point khong thuoc phan di duoc tren navmesh
        // sampleposition se tim diem hop le gan nhat voi diem randompoint
        NavMesh.SamplePosition(randPoint, out hit, float.PositiveInfinity, 1);

        return hit.position;
    }
    public int GetMaxCoin()
    {
        return maxCoin;
    }
}
