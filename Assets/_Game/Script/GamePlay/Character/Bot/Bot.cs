using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : Character
{
    [SerializeField] public Renderer targetMark;

    public override void OnInit()
    {
        base.OnInit();
        isPlayer = false;
    }
}
