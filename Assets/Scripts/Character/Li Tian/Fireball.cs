using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 5;
    public float maxLifetime = 2f;

    public float damage = 5f;

    public float volume = 1f;

    public bool DestroyOnAnyContact = true;

    [HideInInspector]
    public CharacterModel owner;

    [HideInInspector]
    public bool travelForward = false;

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

        if (volume > 0)
        {
            AudioManager.instance.PlayEffect(AudioManager.AudioData.MonkLightATK, transform, volume);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (travelForward)
        {
            MoveFireball();
        }

        BurnOut();
    }

    void MoveFireball()
    {
        Vector3 finalPos = transform.position + (transform.forward * speed);
        transform.position = Vector3.Lerp(transform.position, finalPos, Time.deltaTime);
    }

    void BurnOut()
    {
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
            characterHealthController.TakeDamage(damage, owner);
            Destroy(gameObject);
        }
        else if (DestroyOnAnyContact)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (!otherCollider.isTrigger)
        {
            OnContactWithObject(otherCollider.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.isTrigger)
        {
            OnContactWithObject(collision.gameObject);
        }
    }
}