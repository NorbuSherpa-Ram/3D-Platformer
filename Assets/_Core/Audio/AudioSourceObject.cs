using ObjectPool;
using UnityEngine;

public class AudioSourceObject : MonoBehaviour
{
    private ObjectPooler pool;
    private AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }


    public void SetPool(ObjectPooler pool)
    {
        this.pool = pool;
    }



    public void SetUpClip(AudioClip clip, float volume, float pitch, bool loop = false)
    {
        source.loop = loop;
        source.clip = clip;  // Set the clip
        source.volume = volume;  // Set the volume
        source.pitch = pitch;
    }


    public bool IsPlaying() => source.isPlaying;
    public void StopPlaying()
    {
        source.Stop();
        ReturnToPool();
    }
    public void Play() => source.Play();

    private void ReturnToPool()
    {
        pool.ReturnToPool(transform);
    }
}

