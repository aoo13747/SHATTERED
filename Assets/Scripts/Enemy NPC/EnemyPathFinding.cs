using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyPathFinding : MonoBehaviour
{
    public Transform NPC;
    private GameObject target;
    public float speed = 100f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachEndOfPath = false;
    bool faceleft = true;
    public Animator npcAnim;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player");
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachEndOfPath = true;
            //npcAnim.SetBool("Run", false);
            return;
        }
        else
        {
            reachEndOfPath = false;
            //npcAnim.SetBool("Run", true);
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >= 0.01f && faceleft)
        {
            //NPC.localScale = new Vector3(-2.546225f, 2.546225f, 2.546225f);
            Flip();
        }
        else if (rb.velocity.x <= -0.01f && !faceleft)
        {
            //NPC.localScale = new Vector3(2.546225f, 2.546225f, 2.546225f);
            Flip();
        }

    }
    void Flip()
    {
        faceleft = !faceleft;
        transform.Rotate(0f, 180f, 0f);
    }
}
