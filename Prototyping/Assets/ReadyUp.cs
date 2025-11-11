using System.Collections.Generic;
using UnityEngine;

public class ReadyUp : MonoBehaviour
{
    // No time to properly code this idc

    [SerializeField] GameObject SlimeKing;
    [SerializeField] GameObject Cam;
    private bool db = false;

    private HashSet<GameObject> touchingObjects = new HashSet<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        touchingObjects.Add(collision.gameObject);
        Debug.Log(touchingObjects.Count);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        touchingObjects.Remove(collision.gameObject);

    }


    // You can call this method to get the current count of touching objects
    public int GetTouchingObjectCount()
    {
        return touchingObjects.Count;
    }

    private void Update()
    {
        if (touchingObjects.Count == 1&&db==false)
        {
            db = true;
            MoveUp();
        }
    }

    private void MoveUp()
    {
        SlimeKing.gameObject.SetActive(true);
        Cam.transform.position = new Vector3(0,10,-10);
    }
}
