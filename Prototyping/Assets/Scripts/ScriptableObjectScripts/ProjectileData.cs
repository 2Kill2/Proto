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
    [Tooltip("Vector to launch player when this is shot, is rotated around the aim angle")]
    [SerializeField] Vector2 Recoil;
   
    [Tooltip("Shots per second")]
    [SerializeField] float Firerate;
    [SerializeField] float Size;

    [Header("Firing Mode Settings")]
    [SerializeField] FireMode FireType;
    [Header("Settings for Arc Projectiles Only")]
    [SerializeField] int Count;
    [SerializeField] float Angle;

    [Header("Visuals")]

    [SerializeField] Sprite Sprite;

    [Tooltip("Leave as white to keep original sprite")]
    [SerializeField] Color32 Color;

    [Header("Extra")]
    [SerializeField] AudioClip ShootAudio;

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
    public FireMode fireType => FireType;
    public AudioClip shootAudio => ShootAudio;
    public float angle => Angle;
    public int count => Count;
    public Vector2 recoil => Recoil;
    #endregion

   public enum FireMode
    {
        Single = 0,
        Arc = 1
    }
}
