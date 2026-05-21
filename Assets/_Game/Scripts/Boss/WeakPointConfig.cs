[System.Serializable]
public class WeakPointConfig
{
    public BodyPartName Id;
    public BodyPart BoneTarget;
    public float DamageMultiplier = 5f;
    
    public bool IsActive { get; internal set; }
    public WeakPointUIMarker MarkerInstance { get; internal set; }
}

public enum BodyPartName
{
    ArmLeft, ArmRight, LegLeft, LegRight, Body, Head
}