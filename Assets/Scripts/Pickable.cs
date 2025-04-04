using UnityEngine;

public abstract class Pickable : MonoBehaviour
{
    // Base class for handling interaction logic
    // Each subclass must define this method
    public abstract void Interact(); 
}
