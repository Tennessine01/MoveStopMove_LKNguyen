using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class Character : MonoBehaviour
{
   
    //Anim
    [SerializeField] public Animator animm;
    private string currentAnim;
    //Rigidbody
    [SerializeField] public Rigidbody rb;
    //Range
    [SerializeField] public float range;
    //Movespeed
    [SerializeField] public float moveSpeed;
    //AttackSpeed
    [SerializeField] public float speed;
    //Size
    [SerializeField] public float size;
    //Skin

    //check death
    bool isDead = false;


    public void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
    }
    // Update is called once per frame
    public virtual void Update()
    {


    }


    //-----------------------------------------------------------

    public void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            animm.ResetTrigger(currentAnim);
            animm.SetTrigger(animName);
            
        }
        currentAnim = animName;
    }

}
