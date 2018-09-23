using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float volume = 0.5f;
    public float explosionForce = 1000f;
    public float explosionRadius = 15f;
    public float explosionDuration = 8f;

    public float damage = 15f;
    public float damageCooldown = 2f;

    public ParticleSystem explosionParticleSystem;
    public GameObject explosionProjectilePrefab;
    public ParticleSystem BurningGroundParticleSystem;

    public Transform explosionOrigin;
    public Transform[] projectileSpawnPoints;

    private CharacterModel owner;

    private bool growBurningGround = false;
    private Vector3 burningGroundTargetScale = Vector3.zero;

    private float spiritUsed = 0;

    private Dictionary<CharacterHealthController, float> lastDamagesTaken =
        new Dictionary<CharacterHealthController, float>();

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (growBurningGround && BurningGroundParticleSystem)
        {
            BurningGroundParticleSystem.transform.localScale = Vector3.Lerp(
                BurningGroundParticleSystem.transform.localScale, burningGroundTargetScale, Time.deltaTime);
        }
    }

    public void Explode(CharacterModel owner, float spiritUsed)
    {
        this.owner = owner;
        this.spiritUsed = spiritUsed;

        CharacterCombatController characterCombatController = owner.GetComponent<CharacterCombatController>();
        damage = HelperUtilities.Remap(this.spiritUsed, 0, characterCombatController.maxSpirit, 0, damage);

        AudioManager.instance.PlayEffect(AudioManager.AudioData.Explosion, transform, volume);
        StartCoroutine(PlaySpecialAttackSequence());
    }

    IEnumerator PlaySpecialAttackSequence()
    {
        explosionParticleSystem.Play();
//        yield return new WaitForSeconds(1f);

        CharacterCombatController characterCombatController = owner.GetComponent<CharacterCombatController>();

        float duration = HelperUtilities.Remap(spiritUsed, 0, characterCombatController.maxSpirit,
            0, explosionDuration);

        float force = HelperUtilities.Remap(spiritUsed, 0, characterCombatController.maxSpirit,
            0, explosionForce);

        float radius = HelperUtilities.Remap(spiritUsed, 0, characterCombatController.maxSpirit, 0, explosionRadius);


        ParticleSystem.MainModule psMain = BurningGroundParticleSystem.main;
        psMain.duration = duration;
        growBurningGround = true;
        burningGroundTargetScale = Vector3.one * radius;
        BurningGroundParticleSystem.gameObject.SetActive(true);

        for (int i = 0; i < projectileSpawnPoints.Length; i++)
        {
            ThrowExplosionProjectile(projectileSpawnPoints[i], force, radius);
            yield return new WaitForSeconds(0.1f);
        }

        while (BurningGroundParticleSystem && BurningGroundParticleSystem.isPlaying)
        {
            yield return new WaitForSeconds(duration);
        }

        Destroy(gameObject);
    }

    void ThrowExplosionProjectile(Transform spawnTransform, float force, float radius)
    {
        GameObject projectile =
            Instantiate(explosionProjectilePrefab, spawnTransform.position, spawnTransform.rotation);
        projectile.GetComponent<Fireball>().owner = owner;
        projectile.GetComponent<Rigidbody>()
            .AddExplosionForce(force, explosionOrigin.position, radius);
    }

    void OnContactWithObject(GameObject otherGameObject)
    {
        if (owner == null || otherGameObject == owner.gameObject)
        {
            return;
        }

        CharacterHealthController characterHealthController =
            otherGameObject.GetComponent<CharacterHealthController>();

        if (characterHealthController != null)
        {
            float lastDamageTaken = float.MinValue;
            if (lastDamagesTaken.ContainsKey(characterHealthController))
            {
                lastDamageTaken = lastDamagesTaken[characterHealthController];
            }

            if (Time.time - lastDamageTaken > damageCooldown)
            {
                characterHealthController.TakeDamage(damage, owner);
                lastDamagesTaken[characterHealthController] = Time.time;
            }
        }
    }

    void OnTriggerStay(Collider otherCollider)
    {
        OnContactWithObject(otherCollider.gameObject);
    }
}