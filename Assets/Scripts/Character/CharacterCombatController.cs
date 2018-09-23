using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterCombatController : MonoBehaviour
{
    public float maxSpirit = 100;
    public float spirit;
    public float totalLightAttackCooldownTime = 0.5f;

    public float specialAttackAnimDuration = 1f;

    public bool isBlocking = false;
    public bool isChargingSpirit = false;
    public bool isSpecialAttackActive = false;

    private CharacterMovementController _characterMovementController;
    private CharacterInputController _characterInputController;
    private Animator _animator;

    private float lightAttackCooldownTimer = 0;

    void InitIfNeeded()
    {
        if (!_characterInputController)
        {
            _characterInputController = GetComponentInChildren<CharacterInputController>();
        }

        if (!_characterMovementController)
        {
            _characterMovementController = GetComponentInChildren<CharacterMovementController>();
        }
    }

    void ResetController()
    {
        spirit = maxSpirit;
    }

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        ResetController();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        InitIfNeeded();

        UpdateCombat();
    }

    void UpdateCombat()
    {
        if (lightAttackCooldownTimer > 0)
        {
            lightAttackCooldownTimer -= Time.deltaTime;
            if (lightAttackCooldownTimer < 0)
            {
                lightAttackCooldownTimer = 0;
            }
        }

        if (!_characterInputController)
        {
            return;
        }

        CharacterInputController.CharacterInput characterInput = _characterInputController.characterInput;

        if (characterInput.block && !_characterMovementController.IsDodging && !isChargingSpirit &&
            !isSpecialAttackActive)
        {
            if (!isBlocking)
            {
                if (Block())
                {
                    isBlocking = true;
                    _animator.SetBool("isBlocking", true);
                }
            }
        }
        else
        {
            if (isBlocking)
            {
                Unblock();
                isBlocking = false;
                _animator.SetBool("isBlocking", false);
            }
        }

        if (!isBlocking && !_characterMovementController.IsDodging && !isSpecialAttackActive)
        {
            if (characterInput.specialAttack)
            {
                SpecialAttack();
                isSpecialAttackActive = true;

                _animator.SetTrigger("Special");
                StartCoroutine(OnSpecialAttackUnleashed());
            }

            if (characterInput.specialAttackCharge)
            {
                StartingSpiritCharge();
                isChargingSpirit = true;
                _animator.SetBool("isCharging", true);
            }
            else
            {
                if (isChargingSpirit)
                {
                    CancellingSpiritCharge();
                    isChargingSpirit = false;
                    _animator.SetBool("isCharging", false);
                }
            }

            if (characterInput.lightAttack)
            {
                if (!isChargingSpirit && lightAttackCooldownTimer <= 0)
                {
                    if (LightAttack())
                    {
                        _animator.SetTrigger("Light");
                        lightAttackCooldownTimer = totalLightAttackCooldownTime;
                    }
                }
            }
        }
        else
        {
            if (isChargingSpirit)
            {
                CancellingSpiritCharge();
                isChargingSpirit = false;
                _animator.SetBool("isCharging", false);
            }
        }
    }

    IEnumerator OnSpecialAttackUnleashed()
    {
        yield return new WaitForSeconds(specialAttackAnimDuration);
        isSpecialAttackActive = false;
    }

    public abstract bool Block();
    public abstract void Unblock();
    public abstract bool LightAttack();
    public abstract bool StartingSpiritCharge();
    public abstract void CancellingSpiritCharge();
    public abstract void SpecialAttack();
}