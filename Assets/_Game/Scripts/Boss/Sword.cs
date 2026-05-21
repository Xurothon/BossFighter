using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private float _minScale;
    [SerializeField] private float _maxScale;
    [SerializeField] private float _scaleDuration;
    [SerializeField] private Transform _swordAttackTransform;
    [SerializeField] private float _swordAttackSpeed;
    public Transform SwordTargetTransform => _swordAttackTransform;
    public float SwordAttackSpeed => _swordAttackSpeed;

    public UniTask MakeBig()
    {
        return ScaleToAsync(Vector3.one * _maxScale);
    }
    
    public UniTask MakeSmall()
    {
        return ScaleToAsync(Vector3.one * _minScale);
    }

    public void DisableSword()
    {
        gameObject.SetActive(false);
    }
    
    private  UniTask ScaleToAsync(Vector3 targetScale, CancellationToken ct = default)
    {
        return transform.DOScale(targetScale, _scaleDuration)
            .SetEase(Ease.OutQuad)
            .ToUniTask(ct);
    }
}