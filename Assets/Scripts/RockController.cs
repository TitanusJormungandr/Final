using UnityEngine;

public class RockController : MonoBehaviour
{
    //The amount of damage this rock deals to player
    public int damageAmount = 10;

    private void OnCollisionEnter(Collision collision)
    {
        //Check if the collided object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
            }
        }
    }
}
