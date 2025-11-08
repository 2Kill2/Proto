using UnityEngine;

[CreateAssetMenu(fileName = "ClassData", menuName = "Create Data Object/ClassData")]
public class ClassData : ScriptableObject
{
    [SerializeField] string Name;
    [SerializeField] string Description;

    [SerializeField] Projectile PrimaryFire;
    [SerializeField] Projectile SecondaryFire;

    public string ClassName => Name;
    public string description => Description;
    public Projectile Primary => PrimaryFire;
    public Projectile Secondary => SecondaryFire;
}
