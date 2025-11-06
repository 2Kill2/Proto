using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour
{
    [Tooltip("All data for this projectile")]
    [SerializeField] internal ProjectileData Data;

    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
       _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = Data.sprite;
        _spriteRenderer.color = Data.color;
    }


}
