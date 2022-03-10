using UnityEngine;
using UnityEngine.UI;

public class CursorShower : MonoBehaviour
{
    [SerializeField] private Image _cursorImage;
    [SerializeField] private RectTransform _canvas;

    private void Start()
    {
        //todo uncomment
        // Cursor.visible = false; 
    }

    private void Update()
    {
        ShowCustomCursor();
    }

    private void ShowCustomCursor()
    {
        var mousePosition = Input.mousePosition;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas, mousePosition, Camera.main,
                out Vector2 localPointInRec))
            _cursorImage.transform.localPosition = localPointInRec;
    }
}