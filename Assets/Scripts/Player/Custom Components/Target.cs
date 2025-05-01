using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    //An event which is accesible in the inspector
    public event Action<Target> OnDestroyed;

    //Simply on destroy invoke the event
    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
}
