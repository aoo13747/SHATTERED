using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;
    public int pistolDamage = 25;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }
    private void FixedUpdate()
    {
        Destroy(gameObject,0.2f);
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        EnemyBehavior enemy01 = hitInfo.GetComponent<EnemyBehavior>();

        if (enemy01 != null)
        {
            enemy01.TakeDamage(pistolDamage);
        }
        Destroy(gameObject);
    }
}
