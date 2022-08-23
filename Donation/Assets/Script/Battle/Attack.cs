using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    float cooltime;
    float curtime;
    public GameObject PlayerManager;
    // Start is called before the first frame update
    void Awake()
    {
        cooltime = PlayerManager.GetComponent<PlayerCon>().cooltime;
    }
    
    void FixedUpdate()
    {
        curtime -= Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (collision.tag == "Enemy")
        {
            enemy = collision.gameObject;
        }
        */
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (PlayerManager.GetComponent<PlayerCon>().check == false && curtime <= 0)
            {
                curtime = cooltime;
                if (!collision.gameObject.GetComponent<Enemy>().attackCheck)
                {
                    collision.gameObject.GetComponent<Enemy>().attackCheck = true;
                    collision.gameObject.GetComponent<Enemy>().Attack();
                    collision.gameObject.GetComponent<Enemy>().hp--;
                }
            }
        }
    }
   
}
