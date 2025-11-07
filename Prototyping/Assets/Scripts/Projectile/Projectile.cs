using UnityEngine;


[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Tooltip("All data for this projectile")]
    [SerializeField] internal ProjectileData Data;

    [SerializeField] LayerMask LayerToDamage;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private float _lifetime;
    private void Awake()
    {
        transform.localScale = Data.size;
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void OnEnable()
    {
        
        _spriteRenderer.sprite = Data.sprite;
        _spriteRenderer.color = Data.color;
        _lifetime = Data.lifetime;

        float spread = Random.Range(-Data.spread, Data.spread);
        transform.rotation *= Quaternion.Euler(0f, 0f, spread);

        _rb.linearVelocity = (Vector2)transform.right * Data.velocity;

    }

    private void OnDisable()
    {
        ProjectileManager.Instance.AddToPool(this);
    }

    private void Update()
    {

        if(_lifetime < 0)
        {
            gameObject.SetActive(false);
        }
        _lifetime -= Time.deltaTime;
        CheckForHit();
    }

    private void CheckForHit()
    {
        // Calculate movement distance this frame
        float distance = Data.velocity * Time.deltaTime;
        Vector2 direction = transform.right; // local forward

        // CircleCast for hits
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, Data.size.x / 2f, direction, distance, LayerToDamage);
        if (hit.collider != null)
        {
            Debug.Log($"Hit: {hit.collider.name}");

            //deal damage
            hit.collider.gameObject.SendMessage("Damage", Data.damage);
            
            gameObject.SetActive(false);
        }

       
    }
}
