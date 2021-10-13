using System.Collections;
using System.Collections.Generic;
using Gameplays.Platformer;
using UnityEngine;

public class HealObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //dont heal if detected is not player
        if (other.tag.Equals("Player") == false)
        {
            return;
        }

        //check if health entity is not null
        if (other.GetComponent<HealthEntity>())
        {
            other.GetComponent<HealthEntity>().Heal(1);
            other.GetComponent<CharacterController_Platformer>().PlayHealSound();
            Destroy(this.gameObject);
        }
    }
}
