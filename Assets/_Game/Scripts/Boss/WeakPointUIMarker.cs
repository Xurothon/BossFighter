using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class WeakPointUIMarker : MonoBehaviour
{
    [SerializeField] private float _offsetY = 20f;
    [SerializeField] private bool _hideWhenOffScreen = true;
    
    private RectTransform _rect;
    private Transform _worldTarget;
    private Canvas _canvas;
    private RectTransform _canvasRect;
    private Camera _uiCamera;
    private bool _isInitialized;
    
    public void Initialize(Transform target, Canvas canvas, Camera uiCamera)
    {
        _rect = GetComponent<RectTransform>();
        _worldTarget = target;
        _canvas = canvas;
        _canvasRect = canvas.GetComponent<RectTransform>();
        _uiCamera = uiCamera;

        _isInitialized = true;
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (!_isInitialized || _worldTarget == null || _canvas == null || !gameObject.activeSelf)
            return;
        
        Vector3 screenPos = _uiCamera.WorldToScreenPoint(_worldTarget.position);
        
        if (screenPos.z <= 0)
        {
            _rect.gameObject.SetActive(false);
            return;
        }
        
        screenPos.y += _offsetY;

        bool success = RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPos, null, out Vector2 localPos);

        if (success)
        {
            _rect.anchoredPosition = localPos;
            _rect.gameObject.SetActive(true);
        }
        else
        {
            _rect.gameObject.SetActive(false);
        }
    }
}