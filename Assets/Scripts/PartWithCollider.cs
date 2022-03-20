using UnityEngine;
using UnityEngine.Events;

public class PartWithCollider : MonoBehaviour
{
    public event UnityAction<GameObject> ColliderPartClicked;
    
    private void OnMouseDown()
    {
        ColliderPartClicked?.Invoke(gameObject);
    }
}
