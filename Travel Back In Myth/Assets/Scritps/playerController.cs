using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    Rigidbody2D rb;
    bool jump = false;

    private bool face_right;
    Vector3 move;

    Vector3 startPos;

    public Transform firePoint;
    public GameObject bulletPrefab;
    Vector2 bulletPos;
    public float fireRate = 0.5f;
    float nextFire = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        face_right = true;
        startPos = transform.position;

        rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        move = Vector3.zero;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            move += transform.right;
            if (face_right != true) { flip(); }

        }
        else if (Input.GetKey(KeyCode.A))
        {
            move -= transform.right;
            if (face_right == true) { flip(); }
        }

        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            fire();
        }
    }


    void flip() 
    {
        face_right = !face_right;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        firePoint.transform.Rotate(0f, 180f, 0f);
    }

    void fire()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    private void FixedUpdate()
    {
        if (jump)
        {
            jump = false;
            rb.AddForce(transform.up * 6, ForceMode2D.Impulse);
        }
        rb.AddForce(move * 10);

    }
}
