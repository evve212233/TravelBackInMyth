using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Fox : MonoBehaviour
{
    public int current_scene;
    //public int last_scene;
    Rigidbody2D rb;
    public Vector2 unitMove;
    public Vector3 downPos;
    public Vector3 upPos;
    public float maxLength;
    public float powerScale;
    public int jump_chance;
    private Vector3 startPos;
    public static int face_to ; //-1 = left, 1 = right
    public bool on_the_wall;
    public bool can_move;

    public float dash_speed;
    private bool is_dashing;
    public float default_dash_time; //默认冲刺的冻结时间
    private float dash_time;
    private int dash_chance;

    Vector3 walljump_dir;

    //var for explosion
    public GameObject spawnee;
    public GameObject explosion;
    public float default_ex_time;
    public float ex_time;
    public bool is_explosing;
    public bool have_explosion;

    //sound sources
    private AudioSource src;
    public AudioClip jumpSnd;
    public AudioClip spikeSnd;
    public AudioClip goldSnd;
    public AudioClip dashSnd;

    //
    public float win_time;
    public bool is_winning;

    //var for shooting
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireRate = 0.1f;
    private float nextFire = 0.0f;

    // Start is called before the first frame update
    private void Awake()
    {
        //last_scene = 2;
        
    }
    void Start()
    {
        current_scene = SceneManager.GetActiveScene().buildIndex;
        rb = GetComponent<Rigidbody2D>();
        unitMove = new Vector2(0.16f, 0.0f);
        maxLength = 100.0f;
        powerScale = 6.0f;
        jump_chance = 1;
        startPos = transform.position;

        face_to = 1;
        dash_speed = 15.0f;
        is_dashing = false;
        default_dash_time = 0.3f;
        dash_time = 0;
        dash_chance = 1;

        on_the_wall = false;
        can_move = false;

        walljump_dir = new Vector3(2, 1.5f, 0);

        default_ex_time = 0.55f;
        ex_time = default_ex_time;
        is_explosing = false;
        have_explosion = false;

        is_winning = false;
        win_time = default_ex_time;

        src = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (is_dashing)
        {
            Vector3 t_v = rb.velocity;
            t_v.x = dash_speed * face_to;
            t_v.y = 0;
            rb.velocity = t_v;

            dash_time -= Time.deltaTime;
            if (dash_time <= 0)
            {
                is_dashing = false;
                dash_time = 0;
            }
        }
        else if (!is_dashing)
        {
            if (Input.GetKey(KeyCode.D) && can_move)
            {
                Vector3 temp = transform.localScale;
                temp.x = Mathf.Abs(transform.localScale.x);
                transform.localScale = temp;

                Vector3 t_v = rb.velocity;
                t_v.x = 0;
                rb.velocity = t_v;

                rb.position = rb.position + unitMove;

                face_to = 1;

            }
            if (Input.GetKey(KeyCode.A) && can_move)
            {
                Vector3 temp = transform.localScale;
                temp.x = -Mathf.Abs(transform.localScale.x);
                transform.localScale = temp;

                Vector3 t_v = rb.velocity;
                t_v.x = 0;
                rb.velocity = t_v;

                rb.position = rb.position - unitMove;

                face_to = -1;
            }
            if (Input.GetKeyDown(KeyCode.W) )      
            {
                if (on_the_wall)
                {
                    src.PlayOneShot(jumpSnd);
                    can_move = false; //不准移动

                    //转向
                    Vector3 temp = transform.localScale;
                    temp.x = -temp.x;
                    transform.localScale = temp;
                    face_to = -face_to;

                    Vector3 t_dir = walljump_dir;
                    t_dir.x = face_to * t_dir.x; //反向跳
                    rb.velocity = Vector3.zero;
                    rb.AddForce(t_dir.normalized * powerScale * 1.3f, ForceMode2D.Impulse);
                }
                else if (jump_chance > 0)
                {
                    src.PlayOneShot(jumpSnd);
                    rb.AddForce(transform.up * powerScale, ForceMode2D.Impulse);
                    jump_chance -= 1;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) && dash_chance > 0)
            {
                src.PlayOneShot(dashSnd);
                //dash
                is_dashing = true;
                dash_chance -= 1;
                dash_time = default_dash_time;

                Vector3 t_v = rb.velocity;
                t_v.x = dash_speed * face_to;
                t_v.y = 0;
                rb.velocity = t_v;
            }
            if (Input.GetKeyDown(KeyCode.Q) && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                fire();
                
            }
            if (is_explosing )
            {
                if (!have_explosion)
                {
                    explosion = spawnee;
                    explosion = Instantiate(explosion, transform.position, transform.rotation);
                    have_explosion = true;
                }
                ex_time -= Time.deltaTime;
                if (ex_time <= 0)
                {
                    ex_time = default_ex_time;
                    is_explosing = false;
                    Destroy(explosion);
                    have_explosion = false;
                    reset_game();
                }
            }
            if (is_winning)
            {
                win_time -= Time.deltaTime;
                if (win_time <= 0)
                {
                    //current_scene += 1;
                    win_time = default_ex_time;
                    is_winning = false;
                    //if (current_scene <= last_scene)
                    SceneManager.LoadScene(current_scene+1);
                }
            }
        }
        if (transform.position.y < -100)
            reset_game();
    }
    private void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.collider.tag == "goodwall")
        {
            on_the_wall = true;
        }   
        if (coll.collider.tag == "platform")
        {
            can_move = true;
        }
    }
    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.collider.tag == "goodwall")
        {
            on_the_wall = false;
        }
        if (coll.collider.tag == "platform")
        {
            
        }
    }
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag == "platform")
        {
            jump_chance = 1;
            dash_chance = 1;
        }
        if (coll.collider.tag == "enemy" || coll.collider.tag == "boss")
        {
            reset_game();
        }
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "gem")
            jump_chance += 1;
        if (coll.tag == "spike")
        {
            can_move = false; //不准移动
            rb.velocity = Vector2.zero;
            src.PlayOneShot(spikeSnd);
            is_explosing = true;
        }
        if (coll.tag == "cherry")
        {
            is_winning = true;
            src.PlayOneShot(goldSnd);
            
        }
        if (coll.tag == "enemy" || coll.tag == "boss")
        {
            reset_game();
        }
    }
    private void fire()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
    private void reset_game()
    {
        transform.position = startPos;
        rb.velocity = Vector3.zero;
        SceneManager.LoadScene(current_scene);
    }

}
