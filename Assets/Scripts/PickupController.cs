using UnityEngine;

public class PickupManager : MonoBehaviour
{
    //Determines amount of points given to player
    public int scoreValue = 1;

    //spins the model around
    public Vector3 rotationSpeed = new Vector3(0f, 100f, 0f);

    void Update()
    {
        //Rotates the pickup object infinitely
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        //Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            //Adds points to score
            ScoreManager.instance.AddScore(scoreValue);

            //Destroy the pickup object
            Destroy(gameObject);
        }
    }
}

