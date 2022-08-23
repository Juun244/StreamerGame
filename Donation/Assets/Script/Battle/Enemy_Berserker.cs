using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Berserker : Enemy
{
    Vector3 dist;
    Vector3 dir;
    
    // Start is called before the first frame update
   protected override void Awake()
    {
        base.Awake();
        //추가로 동작할 내용
        hp = 2;
        VecCheck();
        animSpeed = 0.8f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        animator.SetBool("isMoving", true);
        //추가로 동작할 내용
    }
    protected override void FixedUpdate()
    {
        Move();
        AttackedCheck();
    }
    
    public void VecCheck()
    {
        dist = player.transform.position - gameObject.transform.position;
        dir = dist.normalized;
    }
    new public void Move()
    {
        this.transform.position += dir * moveSpeed * Time.deltaTime;
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
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 136f / 255f, 136f / 255f);
        }
        else
        {
            //피격 시
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 28f / 255f, 28f / 255f);
        }
    }
}
