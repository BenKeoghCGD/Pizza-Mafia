using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pizzeria : MonoBehaviour
{
    public Text health_Text;
    public Scrollbar health_Bar;
    public GameObject col1, col2;

    public float health, maxHealth;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        health_Text.text = (int)health + " / " + (int)maxHealth;
        health_Bar.size = health / maxHealth;

        Collider2D[] cols = Physics2D.OverlapAreaAll(col1.transform.position, col2.transform.position);
        if (cols.Length > 0)
        {
            foreach (Collider2D col in cols)
            {
                if(col.name == "Enemy-Bullet")
                {
                    health -= col.GetComponent<Projectile>().damage;
                    Destroy(col.gameObject);
                }
            }
        }

        if(health <= 0)
        {
            GameManager gm = GameObject.FindObjectOfType<GameManager>();
            gm.paused = true;
            gm.endGame();
        }
    }
}
