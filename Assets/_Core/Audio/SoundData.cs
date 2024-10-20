using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// STORE SOUND INFORMATION AND REFERANCE OF SOUND CLIP IT SELF 
/// </summary>
[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData")]
public class SoundData : ScriptableObject
{
    //ODIN INSPECTOR THAT ALLOW SOUND TO PLAY ON THIS REFERANCE ON EDITOR 
    [InlineEditor(InlineEditorModes.FullEditor)]
    public AudioClip clip;

    [Range(0, 1f)]
    public float volume;


    [SerializeField] private bool ranPitch;
    [MinMaxSlider(-2, 2, true)]
    [SerializeField] private Vector2 minMaxPitch;

    public bool loop;

    public float GetPitch()
    {
        float pitch = ranPitch ? Random.Range(minMaxPitch.x, minMaxPitch.y) : 1;
        return pitch;
    }
}
