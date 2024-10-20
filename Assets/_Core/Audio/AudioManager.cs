using UnityEngine;
using ObjectPool;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private ObjectPooler sfxPool;

    public static AudioManager instance;

    [SerializeField] private AudioSource oneShotAudioSource;

    private void Awake()
    {
        instance = this;
    }



    public void PlayOneShotSFX(SoundData soundData)
    {
        if (oneShotAudioSource != null)
        {
            oneShotAudioSource.pitch = soundData.GetPitch();
            oneShotAudioSource.PlayOneShot(soundData.clip, soundData.volume);
        }
    }



    private AudioSourceObject GetAudioSourceFromPool()
    {
        Transform poolObject = sfxPool.GetObjectFromPool();
        return poolObject.GetComponent<AudioSourceObject>();
    }
}
