using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiTianCombatController : CharacterCombatController
{
    public GameObject fireballPrefab;
    public Transform fireballSpawnTransform;

    public ParticleSystem spiritChargingParticleSystem;
    public GameObject explosionPrefab;

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

        Fireball fireball = fireballGameObject.GetComponent<Fireball>();
        fireball.owner = _characterModel;
        fireball.travelForward = true;

        return true;
    }

    public override bool StartingSpiritCharge()
    {
        spiritChargingParticleSystem.Play();
        return true;
    }

    public override void CancellingSpiritCharge()
    {
        spiritChargingParticleSystem.Stop();
    }

    public override void SpecialAttack(float chargedSpirit)
    {
        Debug.Log("Charged Spirit: " + chargedSpirit);
        GameObject explosionGameObject = Instantiate(explosionPrefab, transform.position, transform.rotation);
        explosionGameObject.GetComponent<Explosion>().Explode(_characterModel, chargedSpirit);
    }
}