using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // public GameObject player;
    private Rigidbody2D rb;
    private Animator animator;
    public bool flip;
    public float speed;
    public float checkRadius, meleeAttackRadius;
    // rangedAttackRadius;
    public LayerMask PlayerLayer;
    public Transform target;
    private Vector2 movement;
    public Vector3 dir;
    private bool isInChaseRange, isInMeleeAttackRange, isInRangedAttackRange;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collide with player");
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null) // collide with player
        {
            player.changeHealth(-1);
        }
    }

    private void Update()
    {
        // Vector3 scale = transform.localScale;
        // if (player.transform.position.x > transform.position.x)
        // {
        //     scale.x = Mathf.Abs(scale.x) * -1 * (flip ? -1 : 1);
        //     transform.Translate(speed * Time.deltaTime, 0, 0);
        // }
        // else
        // {
        //     scale.x = Mathf.Abs(scale.x) * (flip ? -1 : 1);
        //     transform.Translate(speed * Time.deltaTime * -1, 0, 0);
        // }
        // transform.localScale = scale;

        animator.SetBool("isInCheckRange", isInChaseRange);
        isInChaseRange = Physics2D.OverlapCircle(transform.position, checkRadius, PlayerLayer);
        isInMeleeAttackRange = Physics2D.OverlapCircle(transform.position, meleeAttackRadius, PlayerLayer);
        // isInRangedAttackRange = Physics2D.OverlapCircle(transform.position, rangedAttackRadius, PlayerLayer);

        dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        dir.Normalize();
        
        movement = dir;
        Debug.Log(movement);
    }

    private void FixedUpdate()
    {
        if (isInChaseRange && !isInMeleeAttackRange)
        {
            MoveEnemy(movement);
        }

        if (isInMeleeAttackRange)
        {
            rb.velocity = Vector2.zero;
        }
        // if (isInRangedAttackRange)
        // {
        //     rb.velocity = Vector2.zero;
        // }
    }

    private void MoveEnemy(Vector2 dir)
    {
        // Y boundaries
        if(transform.position.y >= 13.18f){
            transform.position = new Vector2(transform.position.x, 13.18f);
        }
        else if(transform.position.y <= 0f){
            transform.position = new Vector2(transform.position.x, 0f);
        }
        // X boundaries
        if(transform.position.x >= 61.55f){
            transform.position = new Vector2(61.55f, transform.position.y);
        }
        else if(transform.position.x <= 27.66f){
            transform.position = new Vector2(27.66f, transform.position.y);
        }
        rb.MovePosition((Vector2)transform.position + (dir * speed * Time.deltaTime));
    }
}
