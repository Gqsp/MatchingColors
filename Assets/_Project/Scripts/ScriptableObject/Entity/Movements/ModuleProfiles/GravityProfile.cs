using UnityEngine;

[CreateAssetMenu(menuName = "Collections/ModuleProfile/Gravity")]
public class GravityProfile : ModuleProfile
{
    public float gravity;
    public float upwardGravityMultiplier;
    public float timeToTerminalVelocity;
    public float terminalVelocity;
}
