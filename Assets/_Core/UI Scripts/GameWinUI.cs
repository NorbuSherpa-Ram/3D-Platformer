using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinUI : MonoBehaviour
{


    [SerializeField] private string gameName;
    public void LoadGame()
    {
        SceneManager.LoadScene(gameName);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
