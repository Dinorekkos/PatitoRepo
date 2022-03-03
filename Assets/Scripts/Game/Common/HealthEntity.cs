using System.Collections;
using System.Collections.Generic;
using Gameplays.Platformer;
using UnityEngine;

public class HealthEntity : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private CharacterController_Platformer scriptPatito;
    [SerializeField] private GameObject playerPatito;
    private Animator playerAnimator;
    [SerializeField] private Renderer spritePlayer;
    Color colorSprite;

    public void MakeDamage(int damageAmount)
    {
        playerAnimator.SetTrigger("getDamage");
        scriptPatito.PlayDamagePatito();
        
        //Remove some health
        Health -= damageAmount;
        //Invulnerability for 3 seconds
        StartCoroutine(GetInvulnerable());
        

        //If health is 0 or less than 0
        if (Health <= 0)
        {
            Debug.Log("Patito se murió");
            gameController.MakeGameFinished();
        }
    }
    

    IEnumerator GetInvulnerable()
    {
        Physics2D.IgnoreLayerCollision(17,18, true);
        colorSprite.a = 0.5f;
        spritePlayer.material.color = colorSprite;
        yield return new WaitForSeconds (0.1f);
        colorSprite.a = 1f;
        spritePlayer.material.color = colorSprite;
        yield return new WaitForSeconds(0.1f);
        colorSprite.a = 0.5f;
        spritePlayer.material.color = colorSprite;
        yield return new WaitForSeconds (0.1f);
        colorSprite.a = 1f;
        spritePlayer.material.color = colorSprite;
        yield return new WaitForSeconds(0.1f); 
        colorSprite.a = 0.5f;
        spritePlayer.material.color = colorSprite;
        yield return new WaitForSeconds(0.1f);
        colorSprite.a = 1f;
        spritePlayer.material.color = colorSprite;
        yield return new WaitForSeconds (2f);
        Physics2D.IgnoreLayerCollision(17,18,false);
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
        playerAnimator = playerPatito.GetComponent<Animator>();
        Physics2D.IgnoreLayerCollision(17,18, false);
    }
}
