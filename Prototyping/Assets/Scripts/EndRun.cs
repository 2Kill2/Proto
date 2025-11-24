using UnityEngine;
using UnityEngine.SceneManagement;

public class EndRun : MonoBehaviour
{
   public void End()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
