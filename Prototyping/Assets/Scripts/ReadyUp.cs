using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReadyUp : MonoBehaviour
{
    // No time to properly code this idc

    [SerializeField] GameObject SlimeKing;
    [SerializeField] GameObject Cam;
    [SerializeField] TextMeshProUGUI Label;

    public string targetTag = "Player";
    private bool db = false;
    private HashSet<GameObject> touchingObjects = new HashSet<GameObject>();
    private GameObject[] taggedObjects;

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
        taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);
        Label.text = touchingObjects.Count+"/"+taggedObjects.Length;
        if (touchingObjects.Count > 0 && touchingObjects.Count == taggedObjects.Length && db==false)
        {
            db = true;
            MoveUp();
        }
    }

    private void MoveUp()
    {
        SlimeKing.gameObject.SetActive(true);
        Cam.transform.position = new Vector3(0,10,-10);
        foreach (GameObject obj in taggedObjects)
        {
            obj.transform.position = new Vector3(0, 6, 0);
        }
    }
}
