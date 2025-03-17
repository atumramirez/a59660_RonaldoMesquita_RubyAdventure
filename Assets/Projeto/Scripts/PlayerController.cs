using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : GameManager
{
    // ========= MOVEMENT =================
    public float speed = 4;
    public InputAction moveAction;

    // ======== HEALTH ==========
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public Transform respawnPosition;
    public ParticleSystem hitParticle;
    public Slider healthSlider;

    // ======== PROJECTILE ==========
    public GameObject projectilePrefab;
    public InputAction launchAction;

    // ======== AUDIO ==========
    public AudioClip hitSound;
    public AudioClip shootingSound;

    // ======== HEALTH ==========
    public int health
    {
        get { return currentHealth; }
    }

    // =========== MOVEMENT ==============
    Rigidbody2D rigidbody2d;
    Vector2 currentInput;

    // ======== HEALTH ==========
    int currentHealth;
    float invincibleTimer;
    bool isInvincible;

    // ==== ANIMATION =====
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    // ================= SOUNDS =======================
    AudioSource audioSource;

    // ========== MOVEMENT DISABLE ============
    bool canMove = true;

    // // ========== MOVEMENT DISABLE ============
    public Projectiles projectileType = Projectiles.Regular;

    void Start()
    {
        // =========== MOVEMENT ==============
        rigidbody2d = GetComponent<Rigidbody2D>();
        moveAction.Enable();

        // ======== HEALTH ==========
        invincibleTimer = -1.0f;
        currentHealth = maxHealth;

        // ==== ANIMATION =====
        animator = GetComponent<Animator>();

        // ==== AUDIO =====
        audioSource = GetComponent<AudioSource>();

        // ==== ACTIONS ====
        launchAction.Enable();

        launchAction.performed += LaunchProjectile;

        // Initialize health slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    void Update()
    {
        // ================= HEALTH ====================
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        // ============== MOVEMENT ======================
        if (canMove)
        {
            Vector2 move = moveAction.ReadValue<Vector2>();

            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }

            currentInput = move;

            // ============== ANIMATION =======================
            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
            animator.SetFloat("Speed", move.magnitude);
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Vector2 position = rigidbody2d.position;

            position = position + currentInput * speed * Time.deltaTime;

            rigidbody2d.MovePosition(position);
        }
    }

    // ===================== HEALTH ==================
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            animator.SetTrigger("Hit");
            audioSource.PlayOneShot(hitSound);

            Instantiate(hitParticle, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        // Update the health slider
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth == 0)
            Respawn();

        Debug.Log(currentHealth);
    }

    void Respawn()
    {
        ChangeHealth(maxHealth);
        transform.position = respawnPosition.position;
    }

    // =============== PROJECTILE ========================
    void LaunchProjectile(InputAction.CallbackContext context)
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300, projectileType);

        animator.SetTrigger("Launch");
        audioSource.PlayOneShot(shootingSound);

    }

    // Coroutine to re-enable movement after delay
    IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMove = true;
    }

    // =============== SOUND ==========================
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
