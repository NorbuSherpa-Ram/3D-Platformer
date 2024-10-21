using System;
using UnityEngine;

public class GameWinTrigger : MonoBehaviour
{
    public static EventHandler OnGameWin;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (player != null)
            {
                OnGameWin?.Invoke(this, EventArgs.Empty);
            }
        }
    }

}
