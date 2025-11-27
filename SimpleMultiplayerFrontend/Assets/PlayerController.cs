using System.Runtime.ExceptionServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float dir = 0;
        if (Input.GetKey(KeyCode.D))
        {
            dir += 10;
        }
        if (Input.GetKey(KeyCode.A))
        {
            dir -= 10;
        }

        rb.AddForce(new Vector2(dir,0));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, 500));
        }
    }
}
