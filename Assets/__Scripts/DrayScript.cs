using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrayScript : MonoBehaviour
{
    public enum eMode { idle, move, attack }
    [Header("Inscribed")]
    public float speed = 5;
    public float attackDuration = .25f;
    public float attackDelay = .5f;

    [Header("Dynamic")]
    public int dirHeld = -1;
    public int facing = 1;
    public eMode mode = eMode.idle;
    private float timeAtkDone = 0;
    private float timeAtkNext = 0;

    private Rigidbody2D rb;
    private Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == eMode.attack && Time.time >= timeAtkDone)
        {
            mode = eMode.idle;
        }
        else if (mode == eMode.idle || mode == eMode.move)
        {
            dirHeld = -1;
            if (Input.GetKey(KeyCode.D)) dirHeld = 0;
            if (Input.GetKey(KeyCode.W)) dirHeld = 1;
            if (Input.GetKey(KeyCode.A)) dirHeld = 2;
            if (Input.GetKey(KeyCode.S)) dirHeld = 3;

            if (dirHeld == -1)
            {
                mode = eMode.idle;
            }
            else
            {
                facing = dirHeld;
                mode = eMode.move;
            }
            if (Input.GetKey(KeyCode.Return) && Time.time >= timeAtkNext)
            {
                mode = eMode.attack;
                timeAtkDone = Time.time + attackDuration;
                timeAtkNext = Time.time + attackDelay;

            }
        }
        Vector2 vel = Vector2.zero;
        switch (mode)
        {
            case eMode.attack:
                animator.Play("Dray_Attack_" + facing);
                animator.speed = 0;
                break;
            case eMode.idle:
                animator.Play("Dray_Walk_" + facing);
                animator.speed = 0;
                break;
            case eMode.move:
                animator.Play("Dray_Walk_" + facing);
                switch (facing)
                {
                    case 0:
                        vel = Vector2.right;
                        break;
                    case 1:
                        vel = Vector2.up;
                        break;
                    case 2:
                        vel = Vector2.left;
                        break;
                    case 3:
                        vel = Vector2.down;
                        break;

                }
                animator.speed = 1;
                break;
        }
     
        rb.velocity = vel * speed;

    }
}
