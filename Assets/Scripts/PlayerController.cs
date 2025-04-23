using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private bool canTakeDamage = true;

    //Time in seconds to wait before taking damage again.
    public float damageCooldown = 1f; 

    //Movement
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private Transform mainCamera;
    public float speed = 5f;

    //Health
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        mainCamera = Camera.main.transform;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate()
    {
        if (mainCamera == null) return;

        //Get camera-relative directions
        Vector3 camForward = mainCamera.forward;
        Vector3 camRight = mainCamera.right;

        //Flatten on the Y axis
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        //Use input to calculate movement direction
        Vector3 moveDirection = (camRight * movementX + camForward * movementY).normalized;

        rb.AddForce(moveDirection * speed);

    }

    //Damage taken method
    public void TakeDamage(int damageAmount)
    {
        if (!canTakeDamage)
            return;

        canTakeDamage = false;
        currentHealth -= damageAmount;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log("Player health: " + currentHealth);

        if (IsDead())
        {
            Die();
        }

        StartCoroutine(DamageCooldown());
    }

    //Cooldown for damage taken
    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }


    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public void Die()
    {
        Debug.Log(gameObject.name + " has died.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Respawn
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(50);
        }
    }
}
