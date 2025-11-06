using UnityEngine;


[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Tooltip("All data for this projectile")]
    [SerializeField] internal ProjectileData Data;

    public string Name;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private void Awake()
    {
       _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = Data.sprite;
        _spriteRenderer.color = Data.color;
        Name = Data.nameID;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        ProjectileManager.Instance.AddToCache(this);
    }

}
