using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TagNameItem : GameUnit
{
    [SerializeField] TMP_Text killerName;
    [SerializeField] TMP_Text victimName;
    //private IEnumerator _ieDelay;

    //private void Awake()
    //{
    //    //_ieDelay = CoSelfDestroy();
    //    StartCoroutine(CoSelfDestroy());
    //}

    public void OnInit()
    {
        StartCoroutine(CoSelfDestroy());
    }
    private void OnDisable()
    {
        StopCoroutine(CoSelfDestroy());
    }

    private void OnDestroy()
    {
        StopCoroutine(CoSelfDestroy());
    }

    private IEnumerator CoSelfDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        //Destroy(gameObject);
        //killerName.text = "";
        //victimName.text = "";
        SimplePool.Despawn(this);

    }

    public void SetNameWhenKill(string killer, string victim)
    {
        this.killerName.text = killer;
        this.victimName.text = victim;
    }
    public void OnDespawn()
    {
        StopCoroutine(CoSelfDestroy());
    }
}
