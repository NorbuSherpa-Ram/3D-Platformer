using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{

    [SerializeField] private SoundData jumpSFX;
    [SerializeField] private SoundData runnignSfx;
    [SerializeField] private SoundData walkingSfx;
    [SerializeField] private SoundData landSfx;



    public void Jump()
    {
        //AudioManager.instance.PlaySFx(jumpSFX);
    }
    public void Walk()
    {
        //AudioManager.instance.PlaySFx(walkingSfx);

    }

    public void Run()
    {
        //AudioManager.instance.PlaySFx(runnignSfx);

    }

    public void Land()
    {
        //AudioManager.instance.PlaySFx(landSfx);
    }
}
