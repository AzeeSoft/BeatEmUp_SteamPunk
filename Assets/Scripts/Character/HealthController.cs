using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

	public delegate void OnHealthChanged (float previousHealth, float health);
	public event OnHealthChanged onHealthChanged = delegate {};

	public float maxHealth = 100f;
	public float health;
	Animator anim;

	void Awake ()
	{
		health = maxHealth;
	}
	
	public void TakeDamage (float damage)
	{
		float oldHealth = health;
		health -= damage;
		health = Mathf.Clamp (health, 0, maxHealth);
		onHealthChanged (oldHealth, health);
	}
	
}
