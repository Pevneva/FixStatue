using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public int Stars { get; private set; }
    public event UnityAction<int> StarsAdded; 

    public void AddStars(int addedValue)
    {
        Stars += addedValue;
        StarsAdded?.Invoke(addedValue);
    }
}
