using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    public GameObject playerManager;
    public GameObject swordRad;
    public GameObject attack;
    SpriteRenderer attackSprite;
    SpriteRenderer playerSprite;
    public bool check;
    public float defense;
    public float cooltime;
    public float invincibleTime = 1.5f; // �ǰ� �� �����ð�
    public float itemInvincibleTime = 2.5f;
    public bool attackedCheck = false;
    public bool aimChoice = false;
    Vector2 _mousePos, _playerPos;
    private float enemyPower = 0;


    void Awake()
    {
        swordRad = gameObject.transform.GetChild(0).gameObject;
        attack = swordRad.gameObject.transform.GetChild(0).gameObject;
        attackSprite = attack.GetComponent<SpriteRenderer>();
        playerSprite = this.gameObject.GetComponent<SpriteRenderer>();
        check = true;
    }

    void FixedUpdate()
    {
        PlayerMovement();
        Aim();
        Attack();
    }

    void PlayerMovement() 
    {
        if (Input.GetKey(KeyCode.UpArrow)||Input.GetKey(KeyCode.W))
            transform.Translate(Vector2.up * Time.deltaTime * playerManager.GetComponent<PlayerInfo>().moveSpeed);

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            transform.Translate(Vector2.left * Time.deltaTime * playerManager.GetComponent<PlayerInfo>().moveSpeed);

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            transform.Translate(Vector2.down * Time.deltaTime * playerManager.GetComponent<PlayerInfo>().moveSpeed);

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            transform.Translate(Vector2.right * Time.deltaTime * playerManager.GetComponent<PlayerInfo>().moveSpeed);
    }

 
    public float rotateDegree;
    public void Aim()
    {
        if(!aimChoice)
        {
            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
                rotateDegree = -45;
            //N.E.
            else if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
                rotateDegree = 45;
            //S.W.
            else if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
                rotateDegree = 225;
            //N.W.
            else if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
                rotateDegree = 135;
            //S.
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                rotateDegree = -90;
            //E.
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                rotateDegree = 0;
            //N.
            else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                rotateDegree = 90;
            //W.
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                rotateDegree = 180;

        }
        else
        {
            _mousePos = Input.mousePosition;
            _playerPos = this.gameObject.transform.position;

            Vector3 target = Camera.main.ScreenToWorldPoint(_mousePos);

            float dy = target.y - _playerPos.y;
            float dx = target.x - _playerPos.x;

            rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        }
        
        swordRad.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rotateDegree);
    }
        
   
    
    public void Attack()
    {
        if (check && Input.GetKey(KeyCode.E))
        {
            check = false;
            aimChoice = false;
            //attack ������Ʈ�� �� ���� �ڷ�ƾ ����
            attack.GetComponent<Attack>().StartCoroutine(attack.GetComponent<Attack>().SwingSword());
            StartCoroutine(attackCooldown());
        }
        else if(check && Input.GetMouseButton(0))
        {
            check = false;
            aimChoice = true;
            //attack ������Ʈ�� �� ���� �ڷ�ƾ ����
            attack.GetComponent<Attack>().StartCoroutine(attack.GetComponent<Attack>().SwingSword());
            StartCoroutine(attackCooldown());
        }
    }

    IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(cooltime);
        check = true;
    }

    public IEnumerator attacked()
    {
        StartCoroutine(Blinking());
        yield return new WaitForSeconds(invincibleTime);
        attackedCheck = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            enemyPower = 1000;
        }
        else if(collision.gameObject.tag == "Enemy_Ranger")
        {
            enemyPower = 200;
        }
        else if(collision.gameObject.tag == "Enemy_Berserker")
        {
            enemyPower = 2500;
        }
        if (enemyPower!=0 && !attackedCheck)
        {
            minusHP(enemyPower);
            attackedCheck = true;
            enemyPower = 0;
            StartCoroutine(attacked());
        }
    }
    public void minusHP(float power)
    {
        playerManager.GetComponent<PlayerInfo>().curHP -= power - (power * (defense / 100)); //���� ���� HP���� ��� �� ����
    }
    protected IEnumerator Blinking()
    {
        float countTime = 0;
        float blinkTic = 0.5f;
        if (invincibleTime < blinkTic) blinkTic = invincibleTime / 2.0f;

        while (countTime < invincibleTime)
        {
            if ((countTime / blinkTic) % 2 == 0) 
                playerSprite.color = new Color(1, 1, 1, 0.4f);
            else
                playerSprite.color = new Color(1, 1, 1, 0.8f);

            yield return new WaitForSeconds(blinkTic);

            countTime += blinkTic;
            //Debug.Log(countTime);
        }

        playerSprite.color = new Color32(255, 255, 255, 255);

        yield return null;
    }

    //60�ʰ� ���� 30% ����
    public void IcreaseDefensive()
    {
        defense += 30;
        Invoke("DecreaseDefensive", 30);
    }

    public void DecreaseDefensive()
    {
        defense -= 30;
    }

}
