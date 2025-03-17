using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Public variables
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    public RobotManager robotManager;

    Animator animator;

    // Private variables
    Rigidbody2D rigidbody2d;
    float timer;
    int direction = 1;

    //Audio
    public AudioClip hitSound;
    public AudioClip fixedSound;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;

        animator = GetComponent<Animator>();
    }

    // Update is called every frame
    void Update()
    {
        timer -= Time.deltaTime;


        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;

        if (vertical)
        {
            position.y = position.y + speed * direction * Time.deltaTime;
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;
        }


        rigidbody2d.MovePosition(position);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                Debug.Log("Tocaste");
                player.ChangeHealth(-1); 
            }
        }
    }

    public void Fix()
    {
        speed = 0; 
        robotManager.AddRobots(1);

        animator.SetTrigger("Fixed");
        GetComponent<Collider2D>().enabled = false; 
        StartCoroutine(FlashAndDestroy());
    }

    IEnumerator FlashAndDestroy()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        for (int i = 0; i < 6; i++) 
        {
            sprite.enabled = !sprite.enabled; 
            yield return new WaitForSeconds(0.2f); 
        }

        Destroy(gameObject); 
    }
}