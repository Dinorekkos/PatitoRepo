using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEntity : MonoBehaviour
{

    [SerializeField] private Renderer spritePlayer;
    Color colorSprite;

    public void MakeDamage(int damageAmount)
    {
        //Remove some health
        Health -= damageAmount;
        //Invulnerability for 3 seconds
        StartCoroutine(GetInvulnerable());

        //If healt is 0 or less than 0
        if (Health <= 0)
        {
            Debug.Log("Se reinicia nivel");
        }
    }

    IEnumerator GetInvulnerable()
    {
        Physics2D.IgnoreLayerCollision(17,18, true);
        colorSprite.a = 0.5f;
        spritePlayer.material.color = colorSprite;
        yield return new WaitForSeconds (3f);
        Physics2D.IgnoreLayerCollision(17,18,false);
        colorSprite.a = 1f;
        spritePlayer.material.color = colorSprite;
    }

    public void Heal(int healAmount)
    {
        Health += healAmount;

        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }

    public int Health
    {
        get;
        private set;
    }

    public int MaxHealth = 4;

    private void Start()
    {
        Health = MaxHealth;
        colorSprite = spritePlayer.material.color;
    }
}
