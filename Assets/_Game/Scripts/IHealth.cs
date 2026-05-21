using System;

public interface IHealth
{
    float CurrentHP { get; }
    float MaxHP { get; }
    bool IsAlive { get; }
    event Action<float> HPChanged;
    event Action Died;
}