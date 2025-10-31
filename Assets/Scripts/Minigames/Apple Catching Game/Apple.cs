using System;
using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Apple : MonoBehaviour, IPointerClickHandler
{
    public System.Action onCaught;
    public Action onMissed;

    Rigidbody2D rb;
    public float fallSpeed = 2.0f;
    public bool isCaughtOrMissed = false;
    public SpriteRenderer spriteRenderer;

    // groundY triggers a miss when apple crosses it
    public float groundY = -4.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(float speed, AppleSpawner spawner)
    {
        fallSpeed = speed;
        // zero gravity, we'll move manually for consistent speed
        if (rb != null) rb.gravityScale = 0f;
    }

    void Update()
    {
        // simple downward motion
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (!isCaughtOrMissed && transform.position.y <= groundY)
        {
            isCaughtOrMissed = true;
            onMissed?.Invoke();
            Destroy(gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isCaughtOrMissed) return;
        isCaughtOrMissed = true;
        onCaught?.Invoke();
        Destroy(gameObject);
    }
}

