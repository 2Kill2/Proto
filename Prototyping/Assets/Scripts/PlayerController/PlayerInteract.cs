using UnityEngine;


public class PlayerInteract : MonoBehaviour
{
    private bool inRange;
    [SerializeField] LayerMask InteractibleLayers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & InteractibleLayers) != 0)
            inRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & InteractibleLayers) != 0)
            inRange = false;
    }
}
