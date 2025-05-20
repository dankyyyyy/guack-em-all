using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipToGame : MonoBehaviour
{
    public void SkipDialogue()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
