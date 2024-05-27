using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PoolControler : MonoBehaviour
{
    [Header("---- POOL CONTROLER TO INIT POOL ----")]
    //[Header("Put object pool to list Pool or Resources/Pool")]
    //[Header("Preload: Init Poll")]
    //[Header("Spawn: Take object from pool")]
    //[Header("Despawn: return object to pool")]
    //[Header("Collect: return objects type to pool")]
    //[Header("CollectAll: return all objects to pool")]

    [Space]
    [Header("Pool")]
    public List<PoolAmount> Pool;

    [Header("Particle")]
    public ParticleAmount[] Particle;


    public void Awake()
    {
        for (int i = 0; i < Pool.Count; i++)
        {
            SimplePool.Preload(Pool[i].prefab, Pool[i].amount, Pool[i].root, Pool[i].collect);
        }

        for (int i = 0; i < Particle.Length; i++)
        {
            ParticlePool.Preload(Particle[i].prefab, Particle[i].amount, Particle[i].root);
            ParticlePool.Shortcut(Particle[i].particleType, Particle[i].prefab);
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(PoolControler))]
public class PoolControlerEditor : Editor
{
    PoolControler pool;

    private void OnEnable()
    {
        pool = (PoolControler)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create Quick Root"))
        {
            for (int i = 0; i < pool.Pool.Count; i++)
            {
                if (pool.Pool[i].root == null)
                {
                    Transform tf = new GameObject(pool.Pool[i].prefab.poolType.ToString()).transform;
                    tf.parent = pool.transform;
                    pool.Pool[i].root = tf; 
                }
            }
            
            for (int i = 0; i < pool.Particle.Length; i++)
            {
                if (pool.Particle[i].root == null)
                {
                    Transform tf = new GameObject(pool.Particle[i].particleType.ToString()).transform;
                    tf.parent = pool.transform;
                    pool.Particle[i].root = tf; 
                }
            }
        }

        if (GUILayout.Button("Get Prefab Resource"))
        {
            GameUnit[] resources = Resources.LoadAll<GameUnit>("Pool");

            for (int i = 0; i < resources.Length; i++)
            {
                bool isDuplicate = false;
                for (int j = 0; j < pool.Pool.Count; j++)
                {
                    if (resources[i].poolType == pool.Pool[j].prefab.poolType)
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                if (!isDuplicate)
                {
                    Transform root = new GameObject(resources[i].name).transform;

                    PoolAmount newPool = new PoolAmount(root, resources[i], SimplePool.DEFAULT_POOL_SIZE, true);

                    pool.Pool.Add(newPool);
                }
            }
        }
    }
}

#endif

[System.Serializable]
public class PoolAmount
{
    [Header("-- Pool Amount --")]
    public Transform root;
    public GameUnit prefab;
    public int amount;
    public bool collect;

    public PoolAmount (Transform root, GameUnit prefab, int amount, bool collect)
    {
        this.root = root;
        this.prefab = prefab;
        this.amount = amount;
        this.collect = collect;
    }
}


[System.Serializable]
public class ParticleAmount
{
    public Transform root;
    public ParticleType particleType;
    public ParticleSystem prefab;
    public int amount;
}


public enum ParticleType
{
    Hit
}

public enum PoolType
{
    None = 0,
    Bot = 1,
    W_Axe_1 = 2,
    W_Axe_2 = 3, 
    W_Axe_3 = 4,
    W_Axe_4 = 5 ,
    W_Candy_1 = 6,
    W_Candy_2 = 7,
    W_Candy_3 = 8,
    W_Candy_4 = 9,
    W_Candy_5 = 10,
    W_Candy_6 = 11,
    W_Candy_7 = 12,
    W_Candy_8 = 13,
    W_Boomerang_1 = 14,
    W_Boomerang_2 = 15,
    W_Hammer_1 = 16,
    W_Hammer_2 = 17,
    W_Knife_1 = 18,
    W_Knife_2 = 19,
    W_Uzi_1 = 20,
    W_Uzi_2 = 21,

    B_Axe_1 = 22,
    B_Axe_2 = 23,
    B_Axe_3 = 24,
    B_Axe_4 = 25,
    B_Candy_1 = 26,
    B_Candy_2 = 27,
    B_Candy_3 = 28,
    B_Candy_4 = 29,
    B_Candy_5 = 30,
    B_Candy_6 = 31,
    B_Candy_7 = 32,
    B_Candy_8 = 33,
    B_Boomerang_1 = 34,
    B_Boomerang_2 = 35,
    B_Hammer_1 = 36,
    B_Hammer_2 = 37,
    B_Knife_1 = 38,
    B_Knife_2 = 39,
    B_Uzi_1 = 40,
    B_Uzi_2 = 41,

    HAT_Cowboy = 42,
    HAT_Crown = 43,
    HAT_Ear = 44,
    HAT_Hat = 45,
    HAT_Cap = 46,
    HAT_StrawHat = 47,
    HAT_Headphone = 48,
    HAT_Horn = 49,

}


