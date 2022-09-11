using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public float speed = 0.1f;
    public int pistolDamage = 20;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        PlayerController player = hitInfo.GetComponent<PlayerController>();

        if (player != null)
        {
            player.TakeDamage(pistolDamage);
            Debug.Log("You got hole in your Stomach");
        }

        Destroy(gameObject);
    }
}
