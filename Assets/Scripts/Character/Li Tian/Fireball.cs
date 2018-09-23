using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 5;
    public float maxLifetime = 2f;

    public float damage = 5f;

    public float volume = 1f;

    [HideInInspector]
    public CharacterModel owner;

    private float startTime;
    private ParticleSystem particleSystem;
    private Light light;

    void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        light = GetComponentInChildren<Light>();
    }

    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
        AudioManager.instance.PlayEffect(AudioManager.AudioData.MonkLightATK, null, volume);
    }

    // Update is called once per frame
    void Update()
    {
        MoveFireball();
    }

    void MoveFireball()
    {
        Vector3 finalPos = transform.position + (transform.forward * speed);
        transform.position = Vector3.Lerp(transform.position, finalPos, Time.deltaTime);

        if (Time.time - startTime > maxLifetime)
        {
            if (particleSystem.transform.localScale.magnitude > 0.05f)
            {
                particleSystem.transform.localScale =
                    Vector3.Lerp(particleSystem.transform.localScale, Vector3.zero, Time.deltaTime);
                light.range = Mathf.Lerp(light.range, 0, Time.deltaTime);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (owner == null || otherCollider.gameObject == owner.gameObject)
        {
            return;
        }

        CharacterHealthController characterHealthController =
            otherCollider.gameObject.GetComponent<CharacterHealthController>();
        if (characterHealthController != null)
        {
            characterHealthController.TakeDamage(damage, owner);
        }

        Destroy(gameObject);
    }
}