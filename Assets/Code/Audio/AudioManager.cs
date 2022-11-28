using System;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
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
        _audioDictionary[ ( int ) audioType ].Play();
    }
    
    // Instantiate and populate the audioDictionary
    private void Awake()
    {
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

    private void Start()
    {
        Instance = this;
    }
}

// Add audio file names to list. Ensure the order matches the list's order
public enum AudioType
{
    CharacterChangeDirection,
    CharacterChangeWeapon,
    CharacterFireWeapon,
    CharacterWalk,
    HelmetKnockOff,
    DogSpawn,
    DogSuccessfulCatch,
    DogFailureCatch,
    CatSpawn,
    CatSuccessfulCatch,
    CatLandWithoutHelmet,
    CatLandWithHelmet
    
}
