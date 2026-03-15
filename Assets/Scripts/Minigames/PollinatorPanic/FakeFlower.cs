using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FakeFlower : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bee"))
        {
            FindObjectOfType<PollinatorPanic>().HitFake();
        }
    }
}