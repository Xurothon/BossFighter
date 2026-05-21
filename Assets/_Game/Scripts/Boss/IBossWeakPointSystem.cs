public interface IBossWeakPointActivator
{
    void Activate(BodyPartName id);
    void Deactivate(BodyPartName id);
    void DeactivateAll();
}