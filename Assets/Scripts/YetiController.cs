using System.Collections;
using UnityEngine;

public class YetiController : MonoBehaviour
{
    //How far the yeti slides from its starting position
    public float slideDistance = 3f;

    //How fast the yeti moves
    public float slideSpeed = 2f;

    //Time to wait at each endpoint before sliding in the opposite direction
    public float waitTime = 1f;

    //Damage dealt to the player on contact
    public int damageAmount = 10;

    //Determines if the yeti starts moving to the right
    public bool startMovingRight = true;

    //Internal variables to track movement
    private Vector3 startPosition;
    private Vector3 leftTarget;
    private Vector3 rightTarget;
    private bool movingRight;

    void Start()
    {
        //Save the initial position of the yeti
        startPosition = transform.position;

        //Calculate the positions to slide between based on slide distance
        leftTarget = startPosition - transform.right * slideDistance;
        rightTarget = startPosition + transform.right * slideDistance;

        //Set initial movement direction
        movingRight = startMovingRight;

        //Start the slide coroutine
        StartCoroutine(SlideRoutine());
    }

    //Handles infinitely sliding between two points with pauses at each end
    IEnumerator SlideRoutine()
    {
        while (true)
        {
            Vector3 target = movingRight ? rightTarget : leftTarget;

            while (Vector3.Distance(transform.position, target) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, slideSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = target;
            yield return new WaitForSeconds(waitTime);
            movingRight = !movingRight;
        }
    }

    //Method for player damage
    private void OnCollisionEnter(Collision collision)
    {
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
