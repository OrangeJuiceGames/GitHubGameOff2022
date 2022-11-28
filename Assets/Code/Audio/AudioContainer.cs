using UnityEngine;
using RNG = UnityEngine.Random;

[ RequireComponent(typeof( AudioSource )) ]
public class AudioContainer : MonoBehaviour
{
    [ SerializeField, Tooltip( "Allows clip to overlap when enabled" ) ] 
    private bool playOneShot = true;

    [ Header( "Audio Modifiers" ) ]
    [ SerializeField ]
    private bool useModifiers = true;
    
    [ SerializeField, Range( 0.1f, 1f ) ]
    private float maxVolume = 0.75f;
    
    [ SerializeField, Range( 0f, 0.5f ) ]
    private float volumeRange = 0.1f;
    
    [ SerializeField, Range( 0f, 0.5f ) ]
    private float pitchRange = 0.1f;
    
    
    [Header( "References" )]
    [ SerializeField ] 
    private AudioClip audioClip;
    
    [ SerializeField, Space ] 
    private AudioSource audioSource;
    
    // Play audio attached to script and modify pitch/volume if modifier enabled.
    public void Play()
    {
        if ( !audioClip )
        {
            Debug.Log("Missing audio clip", this);
            return;
        }
        
        if ( useModifiers )
        {
            var randomVolume = GetRandomVolume();
            var valueToSet = randomVolume <= maxVolume ? randomVolume : maxVolume;
            
            audioSource.volume = valueToSet;
            audioSource.pitch = GetRandomPitch();
        }
        
        if( playOneShot )
            audioSource.PlayOneShot( audioClip );
        else
            audioSource.Play();
    }
    
    // Used to alter the pitch of the sound based on PitchRange for audio variance.
    private float GetRandomPitch() => RNG.Range( 1 - pitchRange, 1 + pitchRange );
    
    // Used to alter the volume of the sound based on VolumeRange for audio variance.
    private float GetRandomVolume() => RNG.Range( maxVolume - volumeRange, maxVolume );

    private void Start()
    {
        audioSource.clip = audioClip;
    }
    
    private void OnValidate()
    {
        if ( maxVolume < 0 || maxVolume > 1f )
            maxVolume = 0.75f;
        
        if ( volumeRange < 0 || volumeRange > 0.5f )
            volumeRange = 0.1f;
        
        if ( pitchRange < 0 || pitchRange > 0.5f )
            pitchRange = 0.2f;
    }
}