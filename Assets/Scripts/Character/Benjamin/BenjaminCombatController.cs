using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenjaminCombatController : CharacterCombatController
{
    public GameObject shieldBurstPrefab;
    public Transform shieldBurstSpawnTransform;

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
        GameObject shieldBurstGameObject =
            Instantiate(shieldBurstPrefab, shieldBurstSpawnTransform.position, shieldBurstSpawnTransform.rotation);

        ShieldBurst shieldBurst = shieldBurstGameObject.GetComponent<ShieldBurst>();
        shieldBurst.owner = _characterModel;

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