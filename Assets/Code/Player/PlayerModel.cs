using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel 
{ 
    public float ShotSizeX { get; set; }
    public float ShotSizeY { get; set; }
    public float ShotVelocity { get; set; }
    public float Damage { get; set; }
    public float RateOfFire { get; set; }
    public bool Penetration { get; set; }
    public int ShotCount { get; set; }
    public bool Dash { get; set; }
    public float MoveSpeed { get; set; }
    public float DashCoolDown { get; set; }
    public bool HasShield { get; set; } //give dogs shiels until one do pops it then you loose it
}
