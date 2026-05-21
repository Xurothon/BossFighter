using System.Collections.Generic;
using UnityEngine;

public class BossWeakPointSystem : MonoBehaviour, IBossWeakPointActivator
{
    [SerializeField] private WeakPointConfig[] _configs;
    [SerializeField] private RectTransform _weakPointUIMarker;
    [SerializeField] private Canvas _uiCanvas;
    
    private Dictionary<BodyPartName, WeakPointConfig> _lookup;
    private Camera _uiCamera;

    private void Awake()
    {
        _lookup = new Dictionary<BodyPartName, WeakPointConfig>(_configs.Length);
        _uiCamera = _uiCanvas?.worldCamera ?? Camera.main;
        
        foreach (var cfg in _configs)
        {
            _lookup[cfg.Id] = cfg;
            cfg.IsActive = false;
        }
    }

    public void Activate(BodyPartName id) => SetState(id, true);
    public void Deactivate(BodyPartName id) => SetState(id, false);
    public void DeactivateAll()
    {
        foreach (var id in _lookup.Keys) SetState(id, false);
    }

    private void SetState(BodyPartName id, bool active)
    {
        if (!_lookup.TryGetValue(id, out var cfg)) return;
        
        cfg.IsActive = active;
        if (active)
            cfg.BoneTarget.ActiveWeakPoint(cfg.DamageMultiplier);
        else 
            cfg.BoneTarget.DeactiveWeakPoint();
        EnsureMarker(cfg);
        if (cfg.MarkerInstance != null)
            cfg.MarkerInstance.gameObject.SetActive(active);
    }

    private void EnsureMarker(WeakPointConfig cfg)
    {
        if (cfg.MarkerInstance != null) return;
        GameObject go = Instantiate(_weakPointUIMarker.gameObject, _uiCanvas.transform);
        var marker = go.AddComponent<WeakPointUIMarker>();
        marker.Initialize(cfg.BoneTarget.AnimationTransform, _uiCanvas, _uiCamera);
        cfg.MarkerInstance = marker;
    }
}