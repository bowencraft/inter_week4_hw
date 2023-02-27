using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static System.Net.WebRequestMethods;
using static UnityEngine.Tilemaps.TilemapRenderer;

public class PlayerControl : MonoBehaviour
{
    float horizontalMove;
    private float speed;
    public float maxSpeed = 1;
    public float friction = 0.95f;

    public static int health = 3;

    public GameObject frontGround;
    public GameObject backGround;
    public GameObject startPoint;

    public GameObject healthStatus;

    public Sprite health1;
    public Sprite health2;
    public Sprite health3;


    //public float exchangeSpeed = 0.01f;

    //public float exchangeDepth = 0.5f;
    //private float frontExchangeDepth = 0f;
    //private float backExchangeDepth = 0.5f;

    private int ftbStatus = 1;

    Rigidbody2D myBody;

    bool grounded = false;
    public float castDist = 0.2f;
    public float gravityScale = 5f;
    public float gravityFall = 40f;
    public float jumpLimit = 2f;


    bool jump = false;

    Animator myAnim;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        FTBExchange();
        if (health == 3)
        {
            healthStatus.GetComponent<SpriteRenderer>().sprite = health3;
        }
        else if (health == 2)
        {
            healthStatus.GetComponent<SpriteRenderer>().sprite = health2;
        }
        else if (health == 1)
        {
            healthStatus.GetComponent<SpriteRenderer>().sprite = health1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if (speed < maxSpeed)
            {
                speed += 1;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            //horizontalMove = Input.GetAxis("Horizontal");
        }
        else if (Input.GetKey(KeyCode.A))
        {

            if (speed > -maxSpeed)
            {
                speed -= 1;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else {
            speed *= friction;
        }

        //Debug.Log(horizontalMove);
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            FTBExchange();
        }

        //if (horizontalMove > 0) {

        //}
        //if (horizontalMove < 0)
        //{
        //}

        if (speed > 0.2f || speed < -0.2f)
        {
            myAnim.SetBool("walking", true);

        }
        else
        {
            myAnim.SetBool("walking", false);

        }


    }

    private void FixedUpdate()
    {
        //float moveSpeed = horizontalMove * speed;
        //myBody.velocity = new Vector3(speed, myBody.velocity.y, 0);
        Vector3 newPos = transform.position;
        newPos.x += speed * Time.deltaTime;
        transform.position = newPos;
        //Debug.Log(speed);

        if (jump)
        {
            myBody.AddForce(Vector2.up * jumpLimit, ForceMode2D.Impulse);
            jump = false;

        }

        if (myBody.velocity.y > 0)
        {
            myBody.gravityScale = gravityScale;
        }
        else if (myBody.velocity.y < 0)
        {
            myBody.gravityScale = gravityFall;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDist);
        Debug.DrawRay(transform.position, Vector2.down, Color.red);

        if (hit.collider != null && hit.transform.tag == "Ground")
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }


        //Vector3 frontPos = frontGround.transform.position;
        //Vector3 backPos = backGround.transform.position;
        ////frontPos.z = Mathf.Lerp(frontPos.z, frontExchangeDepth, exchangeSpeed);
        ////backPos.z = Mathf.Lerp(backPos.z, backExchangeDepth, exchangeSpeed);
        //frontGround.transform.position = frontPos;
        //frontGround.transform.position = backPos;
    }

    private void FTBExchange()
    {

        if (ftbStatus == 0)
        { // front to back

            Vector3 frontPos = frontGround.transform.position;
            Vector3 backPos = backGround.transform.position;
            frontPos.z = 1f;
            backPos.z = 0f;
            frontGround.transform.position = frontPos;
            backGround.transform.position = backPos;

            frontGround.GetComponent<TilemapCollider2D>().enabled = false;
            backGround.GetComponent<TilemapCollider2D>().enabled = true;
            frontGround.GetComponent<Tilemap>().color = new Color(0.4f, 0.4f, 0.4f);
            backGround.GetComponent<Tilemap>().color = new Color(1, 1, 1);
            //TilemapRenderer renderer = frontGround.GetComponent<TilemapRenderer>();

            //frontGround.GetComponent<TilemapRenderer>().sortOrder = order;
            ftbStatus = 1;

        }
        else
        {
            Vector3 frontPos = frontGround.transform.position;
            Vector3 backPos = backGround.transform.position;
            frontPos.z = 0f;
            backPos.z = 1f;
            frontGround.transform.position = frontPos;
            backGround.transform.position = backPos;

            frontGround.GetComponent<TilemapCollider2D>().enabled = true;
            backGround.GetComponent<TilemapCollider2D>().enabled = false;
            frontGround.GetComponent<Tilemap>().color = new Color(1, 1, 1);
            backGround.GetComponent<Tilemap>().color = new Color(0.4f, 0.4f, 0.4f);
            ftbStatus = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        {
            if (collision.gameObject.tag.Equals("FailTrigger"))
            {
                health--;
                if (health == 3)
                {
                    healthStatus.GetComponent<SpriteRenderer>().sprite = health3;
                }
                else if (health == 2)
                {
                    healthStatus.GetComponent<SpriteRenderer>().sprite = health2;
                }
                else if (health == 1)
                {
                    healthStatus.GetComponent<SpriteRenderer>().sprite = health1;
                }

                Vector3 playerPos = transform.position;
                playerPos = startPoint.transform.position;
                transform.position = playerPos;

                ftbStatus = 1;
                FTBExchange();
                if (health == 0)
                {
                    SceneManager.LoadScene("FailScene");
                    // goto fail scene
                }

            }
            else if (collision.gameObject.tag.Equals("finish1"))
            {
                SceneManager.LoadScene("Level2");

            }
            else if (collision.gameObject.tag.Equals("finish2"))
            {

                SceneManager.LoadScene("WinScene");

            }
            //Debug.Log("Start Collide" + collision.collider.gameObject.name);
        }
    }

    public void resetHealth() {
        health = 3;
    }
}
