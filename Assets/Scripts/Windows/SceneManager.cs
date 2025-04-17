using UnityEngine;
using UnityEngine.SceneManagement; 

public class ButtonActions : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
