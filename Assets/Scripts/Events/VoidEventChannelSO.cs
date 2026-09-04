using System;
using UnityEngine;

[CreateAssetMenu(menuName ="Events/VoidEventChannel")]
public class VoidEventChannel : ScriptableObject
{
    public event Action OnEventRaised;
    public void Raise() => OnEventRaised?.Invoke();
}
