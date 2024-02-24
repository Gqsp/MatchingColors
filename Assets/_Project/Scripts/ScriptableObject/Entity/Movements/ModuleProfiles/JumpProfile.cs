using UnityEngine;

[CreateAssetMenu(menuName = "Collections/ModuleProfile/Jump")]
public class JumpProfile : ModuleProfile
{
    public int jumpCount;
    public float jumpCutOff;
    public float flyCutOff;
    public float coyoteTime;
    public float jumpBuffer;
    public float jumpForce;
    public float minJumpForceBounceHoldJump;
    public float minJumpForceBounceNoHold;
    public float jumpAttenuation;
    public float hoverJumpForce;
    public float downwardForce;
    public float downwardForceHover;

    public float jumpHeight;
    public float timeToApex;
    public float downwardGravityMultiplier;
    
    public AnimationCurve bounceCurve;
    public AnimationCurve hoverCurve;
}
