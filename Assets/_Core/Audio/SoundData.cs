using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData")]
public class SoundData : ScriptableObject
{
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
