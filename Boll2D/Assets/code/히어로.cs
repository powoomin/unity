using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 히어로 : MonoBehaviour
{
    public float maxSpeed = 0;
    public float jumpPower = 18;
    public int jumpcount = 1;
    public float moveSpeed = 10f;
    public float damage_time = 1f; //무적 시간
    Rigidbody2D rigid;//rigid
    SpriteRenderer spriteRenderer;//방향
    Animator anim;//애니메이션

    //---
    Vector3 movement;
    int jumpcnt;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //점프
        jump();
    }

    void FixedUpdate()
    {
        //Move_0(); 사용 안함
        Move_1();
    }
    void Move_0()
    {

        //옛날버전 move
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed * (-1))
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }

        if (h < 0f)
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
        else if (h > 0f)
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        }
    }
    void Move_1()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
            anim.SetBool("iswalking", true);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
            anim.SetBool("iswalking", true);
        }
        else
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            anim.SetBool("iswalking", false);
        }

        transform.position += moveVelocity * moveSpeed * Time.deltaTime;
    }
    void jump()
    {
        //점프
        if (Input.GetButtonDown("Jump") && jumpcnt > 0)
        {

            rigid.velocity = Vector2.up * jumpPower;
            anim.SetBool("isjumping", true);
            anim.SetBool("iswalking", false);
            jumpcnt--;
        }

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rigid.velocity.y < 0)
        {
            if (rayHit.collider != null)
            {
                anim.SetBool("isjumping", false);
                jumpcnt = jumpcount;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            OnDamaged(collision.transform.position);
        }
    }
    void OnDamaged(Vector2 targetPos)
    {
        gameObject.layer = 11;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 8, ForceMode2D.Impulse);

        anim.SetTrigger("doDamaged");


        Invoke("OffDamaged", damage_time);
    }
    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}