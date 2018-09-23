using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthController : MonoBehaviour {

	public delegate void OnHealthChanged (float previousHealth, float health);
	public event OnHealthChanged onHealthChanged = delegate {};

    public bool isDead = false;

	public float maxHealth = 100f;
	public float health;

    public float spiritReleasePerDamage = 1f;

    public float hitVolume = 0.5f;

    private CharacterCombatController _characterCombatController;
    private Animator _animator;

    void ResetController()
    {
        health = maxHealth;
    }

	void Awake ()
	{
	    _animator = GetComponentInChildren<Animator>();
	    _characterCombatController = GetComponent<CharacterCombatController>();
	    ResetController();
	}

    void Update()
    {

    }
	
	public void TakeDamage (float damage, CharacterModel attackedByCharacterModel = null)
	{
	    if (isDead)
	    {
	        return;
	    }

		float oldHealth = health;

	    if (_characterCombatController.isBlocking)
	    {
	        damage *= _characterCombatController.blockDamageReducer;
	    }

		health -= damage;
		health = Mathf.Clamp (health, 0, maxHealth);

	    if (attackedByCharacterModel != null)
	    {
	        CharacterCombatController otherCharacterCombatController =
	            attackedByCharacterModel.GetComponent<CharacterCombatController>();
            otherCharacterCombatController.AddSpirit(damage * spiritReleasePerDamage);
	    }

	    onHealthChanged(oldHealth, health);

        if (health <= 0)
	    {
	        OnPlayerDeath();
	    }
        
        AudioManager.instance.PlayEffect(AudioManager.AudioData.GettingHit, transform, hitVolume);
        _animator.SetTrigger("Hit");
	}

	public void OnPlayerDeath ()
	{
	    isDead = true;
        _animator.SetTrigger("Die");
//	    GameManager.instance.StartCoroutine("You Lose");
    }
}
