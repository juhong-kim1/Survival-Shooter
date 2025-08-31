using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlaySFX(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;

        AudioSource.PlayClipAtPoint(clip, position, sfxVolume);
    }
}
