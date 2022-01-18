using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Sprite left, right, idle;
    public float moveSpeed = 3.0f;
    Rigidbody2D _rb;
    public int direction = 0;
    public float health, maxHealth;
    public Text health_Text/*, debug*/;
    public Scrollbar health_Bar;
    public enum ProjectileType { BIG_PIZZA, SMALL_PIZZA };
    public ProjectileType activeProjectile;
    private float _rechargeTime_BIG = 5.0f, _rechargeTime_SMALL = 1.5f, _idleTimer = 10.0f;
    public GameObject pizza_Lrg, pizza_Sml;
    public Image lrg_reload, sml_reload;

    private Vector3 prevPos;

    public void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (SystemInfo.supportsAccelerometer) Debug.Log("Using Accelerometer");
        else Debug.Log("No Accelerometer found, using WASD");
        health = maxHealth;
        prevPos = transform.position;
    }

    public void FixedUpdate()
    {
        // Yes, im using the accelerometer to run the gyroscopes job.
        // the accelerometer still functions the same in this case, as it acts like
        // a less sensitive gyroscope.
        if(SystemInfo.supportsAccelerometer)
        {
            Vector3 movement = new Vector3(Input.acceleration.x, 0.0f, 0.0f);
            transform.position += movement * Time.deltaTime * moveSpeed * 2.0f;
        }
        else
        {
            float movement = Input.GetAxis("Horizontal");
            transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * moveSpeed;
        }

        lrg_reload.fillAmount = 1.0f - (_rechargeTime_BIG / 5.0f);
        sml_reload.fillAmount = 1.0f - (_rechargeTime_SMALL / 1.5f);

        health_Text.text = (int)health + " / " + (int)maxHealth;
        health_Bar.size = health / maxHealth;

        if (prevPos.x < transform.position.x)
        {
            GetComponent<SpriteRenderer>().sprite = right;
            direction = 1;
        }

        if (prevPos.x > transform.position.x) 
        {
            GetComponent<SpriteRenderer>().sprite = left;
            direction = -1;
        }

        if (prevPos == transform.position)
        {
            if (_idleTimer <= 0.0f)
            {
                GetComponent<SpriteRenderer>().sprite = idle;
                direction = 0;
            }
            _idleTimer -= Time.deltaTime;
        }
        else _idleTimer = 10.0f;

        _rechargeTime_BIG -= Time.deltaTime;
        _rechargeTime_SMALL -= Time.deltaTime;
        prevPos = transform.position;
    }

    public void setActive(int i)
    {
        if (i == 0) activeProjectile = ProjectileType.SMALL_PIZZA;
        else activeProjectile = ProjectileType.BIG_PIZZA;
    }

    public void Shoot()
    {
        GameObject obj = null;
        switch (activeProjectile)
        {
            case ProjectileType.SMALL_PIZZA:
                if (_rechargeTime_SMALL > 0.0f || direction == 0) break;
                _rechargeTime_SMALL = 1.5f;
                obj = Instantiate(pizza_Sml, transform.position + new Vector3(direction, 0), new Quaternion());
                break;

            case ProjectileType.BIG_PIZZA:
                if (_rechargeTime_BIG > 0.0f || direction == 0) break;
                _rechargeTime_BIG = 5.0f;
                obj = Instantiate(pizza_Lrg, transform.position + new Vector3(direction, 0), new Quaternion());
                break;
        }
        if(obj != null) obj.GetComponent<Projectile>().owner = this.gameObject;
    }
}