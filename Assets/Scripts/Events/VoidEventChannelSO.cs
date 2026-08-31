using System;
using UnityEngine;

[CreateAssetMenu(menuName ="Events/VoidEventChannel")]
public class VoidEventChannel : ScriptableObject
{
    public event Action onEventRaised;
    public void Rasie() => onEventRaised?.Invoke();
}
