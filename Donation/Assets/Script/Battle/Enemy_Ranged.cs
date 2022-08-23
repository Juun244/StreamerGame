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
        //�߰��� ������ ����
        hp = 3;
        animSpeed = 0.4f;
        tracingDistance = 100f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //�߰��� ������ ����
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
    //�÷��̾�� �Ÿ��� attackDistance ������ �� ���ݻ��°� �ǵ��� �ϴ� �Լ�
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

        if (!attackCheck)   //���Ͱ� �÷��̾��� ���ݿ� �ǰ� �� �����ð� ���� ��������Ʈ �� ����
        {
            //�⺻ ����
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 255f / 255f, 255f / 255f);
        }
        else
        {
            //�ǰ� ��
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 142f / 255f, 142f / 255f);
        }
    }

    new public void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        string dist = "";
        if (isTracing && !isAttacking)  //�������� ���� ���� �������� ���߰� �÷��̾ ���󰡵��� ����
        {
           //�÷��̾���� ����Ÿ��� �����Ÿ� �� �� �� ���� �켱�������� �����Ͽ� �̵�
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
