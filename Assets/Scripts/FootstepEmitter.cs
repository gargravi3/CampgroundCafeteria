using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepEmitter : MonoBehaviour
{
    public AudioSource audioSrc;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    public void ExecuteFootstep()
    {
        audioSrc.Play();
    }
}
