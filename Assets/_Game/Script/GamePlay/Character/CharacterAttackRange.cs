using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.TextCore.Text;

public class CharacterAttackRange : MonoBehaviour
{
    //public List<Character> characterList = new List<Character>();
    public Character owner;
    //private Character _targetCharacter;

    //public float range ;

    
    
    //public override void Update()
    //{
    //    if (targetCharacter != null)
    //    {
    //        if (targetCharacter.isDespawn == true)
    //        {
    //            characterList.Remove(targetCharacter);
    //        }
    //    }
    //}

    //danh cho player de mark bot gan nhat
    //public Character targetCharacter
    //{
    //    get { return _targetCharacter; }
    //    set
    //    {
    //        if (_targetCharacter != value) // kiem tra gia tri moi duoc gan co khac gia tri hien tai khong
    //        {
    //            if (_targetCharacter != null)
    //            {
    //                ControlTargetMark(_targetCharacter, false);
    //            }
    //            _targetCharacter = value;
    //            if (_targetCharacter != null)
    //            {
    //                ControlTargetMark(_targetCharacter, true);
    //            }
    //        }
    //    }
    //}
    //public void OnInit()
    //{
    //    _targetCharacter = null;
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_CHARACTER))
        {
            Character character = Cache.GetCharacter(other);
            if (!character.IsDead && character != owner)
            {
                owner.AddTarget(character);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constant.TAG_CHARACTER))
        {
            if (other.CompareTag(Constant.TAG_CHARACTER))
            {
                Character character = Cache.GetCharacter(other);
                owner.RemoveTarget(character);
            }
        }
    }

    //private void ControlTargetMark(Character character, bool state)
    //{
    //    if (!owner.isPlayer) return;
    //    Bot bot = character as Bot;
    //    if (bot != null && bot.targetMark != null)
    //    {
    //        bot.targetMark.enabled = state;
    //    }
    //}

    //public void DetectNearCharacter()
    //{
    //    float minDistance = float.MaxValue;
    //    Character nearestCharacter = null;
    //    if (characterList != null)
    //    {
    //        for (int i = 0; i< characterList.Count; i++)
    //        {
    //            Character character = characterList[i];

    //            if (character.isDespawn == true)
    //            {
    //                //characterList.RemoveAt(i);
    //                continue;
    //            }
    //            if (character == null)
    //            {
    //                continue;
    //            }
    //            Debug.Log(character.name);
    //            float distance = Vector3.Distance(TF.position, character.TF.position);
    //            if (distance < minDistance)
    //            {
    //                minDistance = distance;
    //                nearestCharacter = character;
    //            }
    //        }
    //    }
    //    if(nearestCharacter != null)
    //    {
    //        if(nearestCharacter.isDespawn == true)
    //        {
    //            targetCharacter = null;
    //        }
    //        else
    //        {
    //            targetCharacter = nearestCharacter;
    //        }
    //    }
        
    //}
}

