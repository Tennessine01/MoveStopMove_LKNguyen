using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackRange : Character
{
    public List<Character> characterList = new List<Character>();
    public Character owner;
    private Character _targetCharacter;
    public Character targetCharacter
    {
        get { return _targetCharacter; }
        set
        {
            if (_targetCharacter != value) // kiem tra gia tri moi duoc gan co khac gia tri hien tai khong
            {
                if (_targetCharacter != null)
                {
                    ControlTargetMark(_targetCharacter, false);
                }
                _targetCharacter = value;
                if (_targetCharacter != null)
                {
                    ControlTargetMark(_targetCharacter, true);
                }
            }
        }
    }
    public override void OnInit()
    {
        _targetCharacter = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            //Character character = other.GetComponent<Character>();
            //if (character != null && character != owner && !characterList.Contains(character))
            //{
            //    characterList.Add(character);
            //    DetectNearCharacter();
            //}
            Character character = other.GetComponent<Character>();
            if (character == null) return;
            if (character == owner) return;
            if (characterList.Contains(character)) return;
            characterList.Add(character);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            Character character = other.GetComponent<Character>();
            if (character != null)
            {
                characterList.Remove(character);
                if (character == targetCharacter)
                {
                    DetectNearCharacter();
                }
            }
        }
    }

    private void ControlTargetMark(Character character, bool state)
    {
        if (!owner.isPlayer) return;
        Bot bot = character as Bot;
        if (bot != null && bot.targetMark != null)
        {
            bot.targetMark.enabled = state;
        }
    }

    public void DetectNearCharacter()
    {
        float minDistance = float.MaxValue;
        Character nearestCharacter = null;


        foreach (Character character in characterList)
        {
            float distance = Vector3.Distance(transform.position, character.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCharacter = character;
            }
        }
        targetCharacter = nearestCharacter;

    }
}

