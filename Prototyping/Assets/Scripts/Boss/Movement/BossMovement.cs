using System.Collections;
using UnityEngine;

public abstract class BossMovement : ScriptableObject
{
    [Tooltip("Optional delay before the move (windup)")]
    public float windUp = 0f;

    [Tooltip("Optional delay after the move (endlag)")]
    public float postAttack = 0f;

    public abstract IEnumerator Execute(BossBase boss);
}