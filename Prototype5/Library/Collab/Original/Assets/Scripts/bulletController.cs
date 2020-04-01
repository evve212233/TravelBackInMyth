using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    public float speed = 10f;
    Rigidbody2D rb;
    private int fire_dir;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fire_dir = Fox.face_to;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = fire_dir * transform.right * speed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "wall")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "goodwall")
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemy")
        {
            Destroy(gameObject);
        }

        if (collision.tag == "wall")
        {
            Destroy(gameObject);
        }
    }
}
