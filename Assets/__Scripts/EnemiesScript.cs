using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesScript : MonoBehaviour
{
    protected static Vector2[] directions = new Vector2[] {Vector2.right,
    Vector2.up,Vector2.left,Vector2.down,Vector2.zero};

    [Header("Inscribed : Enemy")]
    public float maxHealth = 1;

    [Header("Dynamic : Enemy")]
    public float health;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected SpriteRenderer sRend;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sRend = GetComponent<SpriteRenderer>();
    }

}
