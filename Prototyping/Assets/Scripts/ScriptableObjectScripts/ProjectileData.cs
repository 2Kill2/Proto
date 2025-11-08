using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Create Data Object/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    [Tooltip("Will be used to find this type in the projectile manager")]
    [SerializeField] string NameID;

    [Header("Stats")]
    [SerializeField] float Damage;
    [SerializeField] float Velocity;
    [SerializeField] float Spread;
    [SerializeField] float Lifetime;
    [SerializeField] float Firerate;
    [SerializeField] float Size;

    [Tooltip("Allows the use of bounce or friction, leave empty for no extra behaviour")]
    [SerializeField] PhysicsMaterial2D PhysicsMaterial = null;

    [Header("Visuals")]

    [SerializeField] Sprite Sprite;

    [Tooltip("Leave as white to keep original sprite")]
    [SerializeField] Color32 Color;

    #region Getters
  
    public float velocity => Velocity;
    public float spread => Spread;
    public float lifetime => Lifetime;
    public float firerate => Firerate;
    public Sprite sprite => Sprite;
    public Color32 color => Color;
    public string nameID => NameID;
    public float size => Size;
   
    public float damage => Damage;
    #endregion

   
}
