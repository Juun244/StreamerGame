using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ranged : Enemy
{
    public float attackDistance = 5;
    bool isAttacking = false;
    public float attackCooltime = 4;
    private float attackCurtime = 0;
    Vector3 dist;
    Vector3 dir;
    public GameObject bullet;

    protected override void Awake()
    {
        base.Awake();
        //추가로 동작할 내용
        hp = 3;
        animSpeed = 0.4f;
        tracingDistance = 100f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //추가로 동작할 내용
    }
    protected override void FixedUpdate()
    {
        DistanceCheck();
        Move();
        AttackedCheck();
        rangedAttack();
        attackCurtime -= Time.deltaTime;
    }
    public void VecCheck()
    {
        dist = player.transform.position - gameObject.transform.position;
        dir = dist.normalized;
    }
    new public void DistanceCheck()
    //플레이어와 거리가 attackDistance 이하일 때 공격상태가 되도록 하는 함수
    {
        distance = Vector3.Distance(player.transform.position, transform.position);
        isTracing = true;
        moveSpeed = 2.0f;

        if(distance < attackDistance)
        {
            isTracing = false;
            isAttacking = true;
        }
        else
        {
            isTracing = true;
            isAttacking = false;
        }
    }

    void rangedAttack()
    {
        if (isAttacking == true && attackCurtime <= 0)
        {
            Instantiate(bullet, transform.position, transform.rotation);
            attackCurtime = attackCooltime;
        }
    }

    new public void AttackedCheck()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }

        if (!attackCheck)   //몬스터가 플레이어의 공격에 피격 시 무적시간 동안 스프라이트 색 변경
        {
            //기본 상태
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 255f / 255f, 255f / 255f);
        }
        else
        {
            //피격 시
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 142f / 255f, 142f / 255f);
        }
    }

    new public void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        string dist = "";
        if (isTracing && !isAttacking)  //추적중일 때는 기존 움직임을 멈추고 플레이어를 따라가도록 설정
        {
           //플레이어와의 수평거리와 수직거리 중 더 먼 것을 우선방향으로 설정하여 이동
           Vector3 playerPos = player.transform.position;
           float hori = Mathf.Abs(transform.position.x - playerPos.x);
           float vert = Mathf.Abs(transform.position.y - playerPos.y);
           if (hori < vert)
           {
               if (transform.position.y > playerPos.y) dist = "Down";
               else if (transform.position.y < playerPos.y) dist = "Up";
           }
           else if (hori >= vert)
           {
                if (transform.position.x > playerPos.x) dist = "Left";
                else if (transform.position.x < playerPos.x) dist = "Right";
           }
        }
        else
        {
            dist = "idle";
        }

        

        if (dist == "Left")
        {
            animator.SetBool("isMoving", true);
            transform.eulerAngles = new Vector3(0, 0, 0);
            moveVelocity = Vector3.left;
        }
        else if (dist == "Right")
        {
            animator.SetBool("isMoving", true);
            transform.eulerAngles = new Vector3(0, 180, 0);
            moveVelocity = Vector3.right;
        }
        else if (dist == "Up")
        {
            animator.SetBool("isMoving", true);
            moveVelocity = Vector3.up;
        }
        else if (dist == "Down")
        {
            animator.SetBool("isMoving", true);
            moveVelocity = Vector3.down;
        }
        else if (dist == "Idle")
        {
            animator.SetBool("isMoving", false);
        }
        transform.position += moveVelocity * moveSpeed * Time.deltaTime;     
    }
}
