using ObjectPool;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{

    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private List<AudioSource> loopSfx = new List<AudioSource>();

    [Button]
    public void PlayOneShotSFX(SoundData soundData)
    {
        if (sfxSource != null)
        {
            sfxSource.pitch = soundData.GetPitch();
            sfxSource.PlayOneShot(soundData.clip, soundData.volume);
        }
    }
    [Button]

    public void PlayLoopSFX(SoundData soundData)
    {
        foreach (var source in loopSfx)
        {
            if (!source.isPlaying)
            {
                source.pitch = soundData.GetPitch();
                source.loop = soundData.loop;
                source.clip = soundData.clip;
                source.Play();
                break;
            }
        }
    }
    [Button]

    public void StopLoopSFX(SoundData soundData)
    {
        foreach (var source in loopSfx)
        {
            if ((source.clip == soundData.clip))
            {
                source.Stop();
            }
        }
    }
}
