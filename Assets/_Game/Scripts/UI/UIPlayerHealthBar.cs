using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIPlayerHealthBar : MonoBehaviour, IUIHealthBar
{
    [SerializeField] private Image _image;
    [Inject] private IHealth _health;
    
    private void OnEnable()
    {
        _health.HPChanged += OnHPChanged;
    }

    private void OnDisable()
    {
        _health.HPChanged -= OnHPChanged;
    }
    
    public void OnHPChanged(float currentHP)
    {
        _image.fillAmount = currentHP;
    }
}