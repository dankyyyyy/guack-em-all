using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour 
{
   

    public void LoadScene(int SceneBuildIndex) 
    {
        SceneManager.LoadScene(SceneBuildIndex);
    }
}
