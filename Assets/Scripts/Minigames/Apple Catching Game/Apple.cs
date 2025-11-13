using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Apple : MonoBehaviour
{
    public Action onCaught;
    public Action onMissed;

    Rigidbody2D rb;
    public float fallSpeed = 2.0f;
    public bool isCaughtOrMissed = false;
    public SpriteRenderer spriteRenderer;

    public float groundY = -4.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        // Make sure collider is NOT a trigger
        var col = GetComponent<Collider2D>();
        col.isTrigger = false;
    }

    public void Initialize(float speed, AppleSpawner spawner)
    {
        fallSpeed = speed;
        rb.gravityScale = 0f; // move manually
    }

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (!isCaughtOrMissed && transform.position.y <= groundY)
        {
            isCaughtOrMissed = true;
            onMissed?.Invoke();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCaughtOrMissed) return;

        if (other.CompareTag("Bucket"))
        {
            isCaughtOrMissed = true;
            onCaught?.Invoke();
            Destroy(gameObject);
        }
    }
}


