using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 망토적 : MonoBehaviour
{
    Rigidbody2D rigid;
    public float moveSpeed = 3;
    public int nextMove;
    SpriteRenderer spriteRenderer;//방향

    Animator anim;



    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        Invoke("random", 5);
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {

        //벽 확인
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rigid.velocity.y < 0)
        {
            if (rayHit.collider == null)
            {
                Return();
            }
        }
        //이동
        Vector3 moveVelocity = Vector3.zero;
        if (nextMove == -1)
        {
            moveVelocity = Vector3.left;

        }
        else if (nextMove == 0)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);

        }
        else if (nextMove == 1)
        {
            moveVelocity = Vector3.right;

        }
        transform.position += moveVelocity * moveSpeed * Time.deltaTime;



    }
    void random()
    {
        nextMove = Random.Range(-1, 2);
        if (nextMove == 0)
            anim.SetBool("walking", false);
        else
            anim.SetBool("walking", true);
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("random", nextThinkTime);

        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == -1;
        }

    }

    void Return()
    {
        nextMove *= -1;
        if (nextMove != 0)
            anim.SetBool("walking", true);
        else
            anim.SetBool("walking", false);
        spriteRenderer.flipX = nextMove == -1;

        CancelInvoke();
        Invoke("random", 2);
    }
}
