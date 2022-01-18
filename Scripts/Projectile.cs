using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject owner;
    public float damage;
    public float moveSpeed;
    public float hits = 0;

    private float direction = 0, timer = 0, lifespan = 5f, _hits;

    private void Start()
    {
        _hits = hits;
        if (owner.GetComponent<Enemy>() != null) direction = owner.GetComponent<SpriteRenderer>().sprite == owner.GetComponent<Enemy>().spr_Left ? -1f : 1f;
        else
        {
            direction = owner.GetComponent<Player>().direction;
            moveSpeed = 0.05f;
        }
    }

    private void Update()
    {

        transform.position += new Vector3(direction, 0f) * moveSpeed * Time.deltaTime;

        if (owner == null) Destroy(gameObject);
        if (owner != null && owner.GetComponent<Player>() != null) transform.localEulerAngles += new Vector3(0, 0, 7);

        if (_hits == 0)
        {
            timer += Time.deltaTime;
            if (timer >= lifespan) Destroy(gameObject);
        }
        else if (hits == 0) Destroy(gameObject);
    }
}