using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    private readonly float _range = 15;
    private FigureRotater _figureRotater;
    private Vector2 _startPosition;
    private bool _isLeftDirection;
    private bool _isRightDirection;
    private bool _isBlockedRuling;

    public event UnityAction MouseUpped;

    private void OnEnable()
    {
        _figureRotater = GetComponent<FigureRotater>();
        _isBlockedRuling = false;
    }

    private void FixedUpdate()
    {
        if (_isBlockedRuling)
            return;
        
        TrySetDirection();

        if (_isLeftDirection)
            _figureRotater.TryRotate(RotateDirection.LEFT);
        else if (_isRightDirection)
            _figureRotater.TryRotate(RotateDirection.RIGHT);
    }

    private void Update()
    {
        if (_isBlockedRuling)
            return;
        
        TrySaveStartData();
        TryResetStartData();
    }

    private void TrySaveStartData()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startPosition = Input.mousePosition;
            ResetDirections();
        }
    }

    private void TryResetStartData()
    {
        if (Input.GetMouseButtonUp(0))
        {
            ResetStarData();
            StartCoroutine(BlockRuling(ParamsController.Figure.BackRotationTime + ParamsController.Figure.DelayMerging));
        }
    }

    private IEnumerator BlockRuling(float time)
    {
        _isBlockedRuling = true;
        yield return new WaitForSeconds(time);
        _isBlockedRuling = false;
    }

    private void ResetStarData()
    {
        ResetDirections();
        MouseUpped?.Invoke();
    }

    private void TrySetDirection()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePosition = Input.mousePosition;
            ResetDirections();
            
            if (Vector2.Distance(_startPosition, currentMousePosition) > _range)
            {
                if (currentMousePosition.x - _startPosition.x > 0)
                    _isRightDirection = true;
                else
                    _isLeftDirection = true;

                _startPosition = Input.mousePosition;
            }
        }
    }

    private void ResetDirections()
    {
        _isLeftDirection = false;
        _isRightDirection = false;
    }
}