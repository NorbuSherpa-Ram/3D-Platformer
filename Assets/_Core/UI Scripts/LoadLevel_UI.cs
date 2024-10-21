using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel_UI : MonoBehaviour
{
    [SerializeField] private string loadScene;
    public void LoadLevel()
    {
        SceneManager.LoadScene(loadScene);
    }
}
