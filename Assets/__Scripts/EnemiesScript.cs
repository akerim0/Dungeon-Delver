using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesScript : MonoBehaviour,ISwappable
{
    protected static Vector2[] directions = new Vector2[] {Vector2.right,
    Vector2.up,Vector2.left,Vector2.down,Vector2.zero};

    [Header("Inscribed : Enemy")]
    public float maxHealth = 1;
    public float knockbackSpeed = 10;
    public float knockbackDuration = 0.25f;
    public float invincibleDuration = 0.5f;
    [SerializeField]
    private GameObject _guaranteedDrop = null;
    public List<GameObject> randomItems;

    [Header("Dynamic : Enemy")]
    public float health;
    public bool invincible = false;
    public bool knockback = false;

    private float knockbackDone = 0;
    private float invincibleDone = 0;
    private Vector2 knockbackVel;

    protected Animator animator;
    protected Rigidbody2D rb;
    protected SpriteRenderer sRend;

    //Iswappable Imp
    public GameObject guaranteedDrop { get => _guaranteedDrop; set => _guaranteedDrop = value; }
    public int tileNum { get; private set; }
    //

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sRend = GetComponent<SpriteRenderer>();
    }
    protected virtual  void Update()
    {
        if (invincible && Time.time > invincibleDone) { invincible = false; }
        sRend.color = invincible ? Color.red : Color.white;
        if (knockback)
        {
            rb.velocity = knockbackVel;
            if (Time.time < knockbackDone) return;
        }
        animator.speed = 1;
        knockback = false;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger !!!");
        if (invincible) return;
        DamageEffectScript dEf = collision.gameObject.GetComponent<DamageEffectScript>();
        if (dEf == null) return;
        health -= dEf.damage;

        if (health <= 0) Die();

        invincible = true;
        invincibleDone = Time.time + invincibleDuration;

        if (dEf.knockback)
        {
            Vector2 delta;

            IFacingMover iFM = collision.GetComponentInParent<IFacingMover>();
            if(iFM != null)
            {
                delta = directions[iFM.GetFacing()];
            }
            else
            {

                delta = transform.position - collision.transform.position;
                if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
                {
                    delta.x = (delta.x > 0) ? 1 : -1;
                    delta.y = 0;
                }
                else
                {
                    delta.x = 0;
                    delta.y = (delta.y > 0) ? 1 : -1;
                }

            }

            knockbackVel = delta * knockbackSpeed;
            rb.velocity = knockbackVel;
            knockback = true;
            knockbackDone = Time.time + knockbackDuration;
            animator.speed = 0;
        }
    }
    void Die()
    {
        GameObject go;
        if(guaranteedDrop != null)
        {
            go = Instantiate<GameObject>(guaranteedDrop);
            go.transform.position = transform.position;
        }
        else if(randomItems.Count > 0)
        {
            int n = Random.Range(0, randomItems.Count);
            GameObject pref = randomItems[n];
            if(pref != null)
            {
                go = Instantiate<GameObject>(pref);
                go.transform.position = transform.position;
            }
        }
        Destroy(gameObject);
    }

    //Iswappable Imp
    public virtual void Init(int fromTileNum, int tileX, int tileY)
    {
        tileNum = fromTileNum;
        transform.position = new Vector3(tileX, tileY, 0) + MapInfo.Offset;
    }
    //
}

