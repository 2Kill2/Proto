using UnityEditor.Animations;
using UnityEngine;

public class SkinSelector : MonoBehaviour
{
    [SerializeField] Canvas SkinMenu;

    [SerializeField] AnimatorController[] Skins;

    [SerializeField] Animator ThisAnimator;
    void Start()
    {
        SkinMenu.enabled = true;
    }
    public void Choice(int choice)
    {
        ThisAnimator.runtimeAnimatorController = Skins[choice];
        SkinMenu.enabled = false;
    }
    
}
