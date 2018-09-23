using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class CharacterInputController : MonoBehaviour
{
    public struct CharacterInput
    {
        public float horizontal;
        public float vertical;
        public Vector3 facingDir;
        public bool dodge;
        public bool block;
        public bool lightAttack;
        public bool specialAttackCharge;
        public bool specialAttack;
    }

    public CharacterInput characterInput = new CharacterInput();

    private CharacterModel _characterModel;

    void Awake()
    {
        _characterModel = GetComponent<CharacterModel>();
    }

    void Update()
    {
        UpdateCharacterInput();   
    }

    public abstract void UpdateCharacterInput();
}