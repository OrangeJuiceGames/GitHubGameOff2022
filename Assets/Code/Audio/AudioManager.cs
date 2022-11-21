using System.Collections.Generic;
using UnityEngine;
using RNG = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [ Header( "Audio Modifiers" ) ] 
    [ SerializeField, Range( 0f, 0.5f ) ]
    private float volumeRange = 0f;
    
    [ SerializeField, Range( 0f, 0.5f ) ]
    private float pitchRange = 0f;


    [ Header( "References" ) ] 
    [ SerializeField ]
    private GameObject audioPoolContainer;
    
    [ SerializeField, Space ] 
    // Unused. This is purely to be able to see the enums for quick verification in inspector.
    private AudioType typesOfEnums_Demo; 
    
    [ SerializeField, Tooltip("Order must match Enum order"), Space ]
    private List<AudioContainer> audioClipPrefabs = new List<AudioContainer>();
    
    
    public static AudioManager Instance { get; private set; }
    
    private readonly Dictionary<int, AudioContainer> _audioDictionary = new Dictionary<int, AudioContainer>();
    
    // Plays audio based on input enum type. Use AudioType enum order for reference.
    public void PlayAudioByEnumType( AudioType audioType )
    {
        var audioContainer = _audioDictionary[ ( int ) audioType ];
        audioContainer.Play( GetRandomVolume(), GetRandomPitch() );
    }
    
    // Used to alter the pitch of the sound based on PitchRange for audio variance.
    private float GetRandomPitch() => RNG.Range( 1 - pitchRange, 1 + pitchRange );
    
    // Used to alter the volume of the sound based on VolumeRange for audio variance.
    private float GetRandomVolume() => RNG.Range( 1 - volumeRange, 1 );
    
    // Populate the audioDictionary
    private void Awake()
    {
        Instance = this;
        
        if ( audioClipPrefabs.Count == 0 )
        {
            Debug.Log( "No audio clips added to list", this );
            return;
        }

        for ( var i = 0; i < audioClipPrefabs.Count; i++ )
        {
            var audioObject = Instantiate( audioClipPrefabs[ i ], audioPoolContainer.transform );
            _audioDictionary.Add( i, audioObject );
        }
    }

    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.P ) )
            PlayAudioByEnumType( AudioType.testGood );

        if ( Input.GetKeyDown( KeyCode.O ) )
            PlayAudioByEnumType( AudioType.testBad );
    }
}

// Add audio file names to list. Ensure the order matches the list's order
public enum AudioType
{
    testGood,
    testBad
}
