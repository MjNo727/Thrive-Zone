using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float speed;
    public float checkRadius;
    public float attackRadius;
    public bool shouldRotate;
    public LayerMask whatIsPlayer;
    private Transform target;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    public Vector3 dir;

    private bool inChaseRange;
    private bool isInAttackRange;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null) // collide with player
        {
            player.changeHealth(-1);
        }
    }
}
