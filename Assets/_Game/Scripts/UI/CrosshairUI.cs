using UnityEngine;
using Zenject;

[RequireComponent(typeof(RectTransform))]
public class CrosshairUI : MonoBehaviour
{
    [SerializeField] private RectTransform _crosshairRect;
    [SerializeField] private Canvas _canvas;
        
    [Inject] private IInputService _input;
    private Vector2 _currentScreenPos;

    private void OnEnable() => 
        _input.OnMouseScreenPosition += UpdatePosition;
    private void OnDisable() => 
        _input.OnMouseScreenPosition -= UpdatePosition;

    private void UpdatePosition(Vector2 screenPos)
    {
        _currentScreenPos = screenPos;
        UpdateCrosshairPosition();
    }

    private void LateUpdate() => 
        UpdateCrosshairPosition();

    private void UpdateCrosshairPosition()
    {
        if (_crosshairRect == null || _canvas == null) return;
        
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform, 
                _currentScreenPos, 
                _canvas.worldCamera, 
                out Vector2 localPos))
        {
            _crosshairRect.anchoredPosition = localPos;
        }
    }
}