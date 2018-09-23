using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInputController : CharacterInputController
{
    public float preferredRange = 5f;
    public float preferredRangeThreshold = 2f;

    public float defenceVsOffenseRatio = 0.35f;
    public float blockVsDodgeRatio = 0.25f;
    public float lightVsSpecialAttackRatio = 0.8f;

    public float blockDuration = 1f;
    public float blockDurationThreshold = 0.5f;

    public float specialChargeDuration = 2f;
    public float specialChargeDurationThreshold = 1f;


    private CharacterModel opponent;

    private float blockTill = 0;
    private float specialChargeTill = 0;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        CheckForOpponents();
    }

    void OnDrawGizmos()
    {
    }

    void CheckForOpponents()
    {
        if (!opponent)
        {
            for (int i = 0; i < ArenaManager.Instance.characterModels.Count; i++)
            {
                CharacterModel otherCharacterModel = ArenaManager.Instance.characterModels[i];
                Debug.Log(otherCharacterModel);
                if (otherCharacterModel.gameObject != _characterModel.gameObject)
                {
                    opponent = otherCharacterModel;
                    return;
                }
            }
        }
    }

    public override void UpdateCharacterInput()
    {
        if (!opponent)
        {
            return;
        }

        AdjustPosition();
        LookAtOpponent();

        ChooseOffenseOrDefence();
    }

    void AdjustPosition()
    {
        Vector3 toOpponent = opponent.transform.position - transform.position;
        toOpponent.y = 0;

        Vector3 toOpponentWorld = transform.TransformDirection(toOpponent);

        Vector3 cameraMoveDir = ArenaManager.Instance.arenaCamera.transform.InverseTransformDirection(toOpponentWorld);

        characterInput.horizontal = 0;
        characterInput.vertical = 0;

        Debug.Log("Mag: " + toOpponent.magnitude);

        if (toOpponent.magnitude < (preferredRange + preferredRangeThreshold) &&
            toOpponent.magnitude > (preferredRange - preferredRangeThreshold))
        {
            characterInput.horizontal = 0;
            characterInput.vertical = 0;
        }
        else if (toOpponent.magnitude > preferredRange)
        {
            Debug.Log("Moving Closer");
            cameraMoveDir.Normalize();
            characterInput.horizontal = cameraMoveDir.x;
            characterInput.vertical = cameraMoveDir.z;
        }
        else if (toOpponent.magnitude < preferredRange)
        {
            Debug.Log("Moving Farther");

            cameraMoveDir.Normalize();
            characterInput.horizontal = cameraMoveDir.x;
            characterInput.vertical = cameraMoveDir.z;
        }
    }

    void LookAtOpponent()
    {
        Vector3 toOpponent = opponent.transform.position - transform.position;
        toOpponent.y = transform.position.y;

        characterInput.facingDir = toOpponent.normalized;
    }

    void ChooseOffenseOrDefence()
    {
        if (Time.time < blockTill)
        {
            characterInput.block = true;
            return;
        }

        if (Time.time < specialChargeTill)
        {
            characterInput.specialAttackCharge = true;
            return;
        }
        else if(characterInput.specialAttackCharge)
        {
            characterInput.specialAttackCharge = false;
            characterInput.specialAttack = true;
            return;
        }

        characterInput.block = false;
        characterInput.dodge = false;

        characterInput.lightAttack = false;
        characterInput.specialAttackCharge = false;
        characterInput.specialAttack = false;

        float rnd = Random.value;
        if (rnd < defenceVsOffenseRatio)
        {
            MakeADefensiveMove();
        }
        else
        {
            MakeAnOffensiveMove();
        }
    }

    void MakeAnOffensiveMove()
    {
        CharacterCombatController characterCombatController = _characterModel.GetComponent<CharacterCombatController>();

        if (characterCombatController.spirit >= characterCombatController.minSpiritConsumption)
        {
            float rnd = Random.value;
            if (rnd < lightVsSpecialAttackRatio)
            {
                characterInput.lightAttack = true;
            }
            else
            {
                characterInput.specialAttackCharge = true;
                specialChargeTill = Time.time + specialChargeDuration +
                                    Random.Range(-specialChargeDurationThreshold, specialChargeDurationThreshold);
            }
        }
        else
        {
            characterInput.lightAttack = true;
        }
    }

    void MakeADefensiveMove()
    {
        float rnd = Random.value;

        if (rnd < blockVsDodgeRatio)
        {
            characterInput.block = true;
            blockTill = Time.time + blockDuration + Random.Range(-blockDurationThreshold, blockDurationThreshold);
        }
        else
        {
            characterInput.dodge = true;
        }
    }
}