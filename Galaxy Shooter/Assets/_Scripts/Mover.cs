using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float minSpeed, maxSpeed;
    private Rigidbody2D rb;
    public bool metroid;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (metroid == true)
        {
            rb.velocity = Vector2.down * Random.Range(minSpeed, maxSpeed);
        }
        else
        {
            rb.velocity = transform.up * Random.Range(minSpeed, maxSpeed);
        }
    }
}
