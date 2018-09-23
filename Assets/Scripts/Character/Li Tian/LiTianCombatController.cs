﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiTianCombatController : CharacterCombatController
{
    public GameObject fireballPrefab;
    public Transform fireballSpawnTransform;

    public override bool Block()
    {
        return true;
    }

    public override void Unblock()
    {
    }

    public override bool LightAttack()
    {
        GameObject fireballGameObject =
            Instantiate(fireballPrefab, fireballSpawnTransform.position, fireballSpawnTransform.rotation);
        return true;
    }

    public override bool StartingSpiritCharge()
    {
        return true;
    }

    public override void CancellingSpiritCharge()
    {
    }

    public override void SpecialAttack()
    {
    }
}