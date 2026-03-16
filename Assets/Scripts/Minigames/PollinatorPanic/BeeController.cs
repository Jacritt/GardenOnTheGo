using UnityEngine;

public class BeeController : MonoBehaviour
{
    public float speed = 6f;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(x, y, 0f) * speed * Time.deltaTime);
    }
}
