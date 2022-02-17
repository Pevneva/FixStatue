using UnityEngine;
using UnityEngine.Events;

public class PartWithCollider : MonoBehaviour
{
    public event UnityAction<GameObject> ColliderPartClicked;
    private void OnMouseDown()
    {
        Debug.Log("AAA CLICKED on : " + gameObject.name);
        ColliderPartClicked?.Invoke(gameObject);
    }
}
