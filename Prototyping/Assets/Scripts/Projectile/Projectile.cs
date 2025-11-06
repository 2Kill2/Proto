using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent (typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour
{
    [Tooltip("All data for this projectile")]
    [SerializeField] internal ProjectileData Data;

    public string Name;

    private SpriteRenderer _spriteRenderer;
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
}
