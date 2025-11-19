using UnityEngine;

public class MissZone : MonoBehaviour
{
    public AppleSpawner spawner;

    void OnTriggerEnter2D(Collider2D other)
    {
        Apple apple = other.GetComponent<Apple>();
        if (apple != null && !apple.isCaughtOrMissed)
        {
            apple.isCaughtOrMissed = true;
            spawner?.onAppleMissed?.Invoke();
            Destroy(apple.gameObject);
        }
    }
}

