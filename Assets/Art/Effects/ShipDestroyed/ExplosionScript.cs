using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public ParticleSystem[] explosion;
    // Start is called before the first frame update
    void Start()
    {
        SetExplosionSize();
    }
    void SetExplosionSize() 
    {
        for(int i = 0; i < explosion.Length; i++) 
        {
            var main = explosion[i].main;
            main.startSize = .01f;
            
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
