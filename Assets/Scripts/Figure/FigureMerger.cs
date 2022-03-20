using UnityEngine;
using UnityEngine.Events;

public class FigureMerger : MonoBehaviour
{
    public event UnityAction<GameObject> FigureMerged;

    public void Merge(GameObject _baseFigures, GameObject _mergedFigure)
    {
        foreach (var partWithCollider in _mergedFigure.transform.gameObject.GetComponentsInChildren<PartWithCollider>())
            partWithCollider.gameObject.transform.parent = _baseFigures.transform;

        Destroy(_mergedFigure.gameObject);
        FigureMerged?.Invoke(_mergedFigure);
    }
}