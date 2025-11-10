using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInteract : MonoBehaviour
{
    
    [SerializeField] LayerMask InteractibleLayers;

    private Collider2D _collision;
    public void InteractInput(InputAction.CallbackContext input)
    {
        if(input.performed && _collision != null)
            _collision.gameObject.SendMessage("Interact",gameObject ,SendMessageOptions.DontRequireReceiver);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & InteractibleLayers) != 0)
            _collision = collision;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & InteractibleLayers) != 0)
            _collision = null;

    }
   
}
