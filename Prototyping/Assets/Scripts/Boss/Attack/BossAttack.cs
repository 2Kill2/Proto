using System.Collections;
using UnityEngine;

public abstract class BossAttack : ScriptableObject
{
    [Tooltip("Optional delay before this attack executes (windup)")]
    public float windUp = 0f;

    [Tooltip("Optional delay after this attack ends")]
    public float postAttack = 0f;

    public abstract IEnumerator Execute(BossBase boss);
}