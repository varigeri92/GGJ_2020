using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WallProperties : ScriptableObject
{
    public int durability;
    public WallType type;
    public AudioClip destroyedClip;
    public AudioClip damagedClip;
}
