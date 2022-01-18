using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public string rname;
    private string[] names = {"Charles", "Harrelson", "Richard", "Kuklinski", "Alexander", "Solonik", "Giuseppe", "Greco", "Glennon", "Engleman", "Benjamin", "Siegel"};
    public GameObject bullet_Spawnpoint, bullet_Prefab, pizzeria, player;
    public Sprite spr_Left, spr_Right;

    public float health, maxHealth, moveSpeed;
    public Text enName, health_Text;
    public Scrollbar health_Bar;

    private enum Targets { NONE, PIZZERIA, PLAYER };
    private Targets target;
    public GameObject col1, col2;
    private List<Projectile> hasHit = new List<Projectile>();

    private void Start()
    {
        pizzeria = GameObject.FindObjectOfType<Pizzeria>().gameObject;
        player = GameObject.FindObjectOfType<Player>().gameObject;
        rname = names[Random.Range(0, names.Length - 1)];
        enName.text = "ENEMY " + rname;
        target = Targets.NONE;
        health = maxHealth;
        canShoot = true;
    }

    private void Update()
    {
        if (transform.position.x > pizzeria.transform.position.x) GetComponent<SpriteRenderer>().sprite = spr_Left;
        else GetComponent<SpriteRenderer>().sprite = spr_Right;

        health_Text.text = (int)health + " / " + (int)maxHealth;
        health_Bar.size = health / maxHealth;

        transform.position = Vector3.Lerp(transform.position, new Vector3(pizzeria.transform.position.x, transform.position.y), Time.deltaTime * moveSpeed);

        if (Vector3.Distance(transform.position, pizzeria.transform.position) < 9f) target = Targets.PIZZERIA;
        else if (Vector3.Distance(transform.position, player.transform.position) < 7f) target = Targets.PLAYER;
        else target = Targets.NONE;

        if(target != Targets.NONE)
        {
            if (canShoot)
            {
                canShoot = false;
                StartCoroutine(Shoot());
            }
        }

        Collider2D[] cols = Physics2D.OverlapAreaAll(col1.transform.position, col2.transform.position);
        if (cols.Length > 0)
        {
            Projectile proj = null;
            foreach (Collider2D col in cols)
            {
                if (col.GetComponent<Projectile>() != null && col.GetComponent<Projectile>().owner == player) proj = col.GetComponent<Projectile>();
            }

            if(proj != null && !hasHit.Contains(proj))
            {
                health -= proj.damage;
                if (proj.hits <= 0) Destroy(proj.gameObject);
                else
                {
                    proj.hits -= 1;
                    proj.damage = proj.damage * 0.75f;
                }
                hasHit.Add(proj);
            }
        }

        if (health <= 0)
        {
            GameObject.FindObjectOfType<GameManager>().enemyKilled();
            Destroy(this.gameObject);
        }
    }

    private bool canShoot;
    public float shootDelay = 2.0f;
    private IEnumerator Shoot()
    {
        yield return null;
        GameObject obj = Instantiate(bullet_Prefab, bullet_Spawnpoint.transform.position, new Quaternion());
        obj.name = "Enemy-Bullet";
        Projectile p = obj.AddComponent<Projectile>();
        p.damage = 10f;
        p.owner = this.gameObject;
        p.moveSpeed = 0.01f;
        yield return null;
        yield return new WaitForSecondsRealtime(shootDelay);
        canShoot = true;
    }
}
