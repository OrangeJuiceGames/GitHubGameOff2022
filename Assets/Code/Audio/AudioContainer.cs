using UnityEngine;

[RequireComponent( typeof( AudioSource ) )]
public class AudioContainer : MonoBehaviour
{
    [ SerializeField, Tooltip( "Allows clip to overlap when enabled" ) ] 
    private bool playOneShot = true;

    [Header( "References" )]
    [ SerializeField ] 
    private AudioClip audioClip;
    
    [ SerializeField, Space ] 
    private AudioSource audioSource;
    
    public void Play(float volume, float pitch)
    {
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        
        if( playOneShot )
            audioSource.PlayOneShot( audioClip );
        else
            audioSource.Play();
    }

    private void Start()
    {
        audioSource.clip = audioClip;
    }
}