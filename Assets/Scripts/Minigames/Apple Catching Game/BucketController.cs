using UnityEngine;

public class BucketController : MonoBehaviour
{
    public float moveSpeed = 5f; // (still used for testing in editor)
    Vector3 touchOffset;

    void Update()
    {
#if UNITY_EDITOR
        // Editor fallback: move with arrows
        float h = Input.GetAxis("Horizontal");
        transform.position += Vector3.right * h * moveSpeed * Time.deltaTime;
#endif

        // Touch or mouse drag (works on mobile)
        if (Input.GetMouseButtonDown(0))
        {
            touchOffset = transform.position - GetWorldPos();
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 newPos = GetWorldPos() + touchOffset;
            transform.position = new Vector3(newPos.x, transform.position.y, transform.position.z);
        }
    }

    Vector3 GetWorldPos()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        return Camera.main.ScreenToWorldPoint(screenPos);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Apple apple = other.GetComponent<Apple>();
        if (apple != null && !apple.isCaughtOrMissed)
        {
            apple.isCaughtOrMissed = true;
            apple.onCaught?.Invoke();
            Destroy(other.gameObject);
        }
    }
}


