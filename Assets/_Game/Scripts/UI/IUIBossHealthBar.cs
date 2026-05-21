public interface IUIBossHealthBar : IUIHealthBar
{
    void OnHPChanged(float currentHP);
    void Active();
}