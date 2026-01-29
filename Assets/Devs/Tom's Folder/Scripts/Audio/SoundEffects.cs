using UnityEngine;

[CreateAssetMenu(fileName = "SoundEffects", menuName = "Scriptable Objects/SoundEffects")]
public class SoundEffects : ScriptableObject
{
    public string Name;

    public AudioClip Clip;
}
