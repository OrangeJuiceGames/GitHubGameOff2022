using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public event Action<UpgradeMaterial> OnUpgradeCollected;
}
