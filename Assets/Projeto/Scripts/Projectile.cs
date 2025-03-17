using UnityEngine;

public class Projectile : GameManager
{
    Rigidbody2D rigidbody2d;
    Projectiles currentType;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.magnitude > 1000.0f)
            Destroy(gameObject);
    }

    public void Launch(Vector2 direction, float force, Projectiles type)
    {
        currentType = type;
        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController e = other.collider.GetComponent<EnemyController>();

        if (e != null)
        {
            e.Fix();
        }

        Destroy(gameObject);
    }
}