using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public int Stars { get; private set; }
    public event UnityAction<int> StarsAdded; 

    private void Awake()
    {
        AddStars(100);
    }

    public void AddStars(int addedValue)
    {
        Stars += addedValue;
        StarsAdded?.Invoke(addedValue);
    }
}
