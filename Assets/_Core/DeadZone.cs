using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (player != null)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
