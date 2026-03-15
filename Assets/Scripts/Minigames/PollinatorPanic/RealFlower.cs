using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealFlower : MonoBehaviour
{
    private bool used = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;

        if (other.CompareTag("Bee"))
        {
            used = true;
            FindObjectOfType<PollinatorPanic>().Pollinate();
            Destroy(gameObject);
        }
    }
}
