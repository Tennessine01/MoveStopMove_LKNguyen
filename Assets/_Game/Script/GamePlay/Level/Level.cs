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
    //public int maxCoin = 200;
    public Vector3 RandomPoint()
    {
        // Tao mot goc ngau nhien tu 0 den 360 do
        float angle = Random.Range(0f, 360f);

        // Tao mot ban kinh ngau nhien tu 10 don vi den ban kinh toi da cho phep trong vung minPoint den maxPoint
        float minRadius = 10f;
        float maxRadius = Mathf.Min(
            Vector3.Distance(centerPosition.position, minPoint.position),
            Vector3.Distance(centerPosition.position, maxPoint.position)
        );
        float radius = Random.Range(minRadius, maxRadius);

        // Chuyen doi tu toa do cuc sang toa do Cartesian
        float x = centerPosition.position.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = centerPosition.position.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);

        Vector3 randPoint = new Vector3(x, centerPosition.position.y, z);

        // Xac dinh vi tri hop le tren NavMesh
        NavMeshHit hit;
        NavMesh.SamplePosition(randPoint, out hit, float.PositiveInfinity, 1);

        return hit.position;
    }

    //public int GetMaxCoin()
    //{
    //    return maxCoin;
    //}
}
