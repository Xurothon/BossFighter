using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIBossHealthBar : MonoBehaviour, IUIBossHealthBar
{
    [SerializeField] private Image _image;
    [Inject] private IHealth _health;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

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

    public void Active()
    {
        gameObject.SetActive(true);
    }
}