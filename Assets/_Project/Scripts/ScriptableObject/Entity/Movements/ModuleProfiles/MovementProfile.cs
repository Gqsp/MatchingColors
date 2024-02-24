using UnityEngine;

[CreateAssetMenu(menuName = "Collections/ModuleProfile/Movement")]
public class MovementProfile : ModuleProfile
{
    public float groundedSpeed;
    public float speedCap;
    
    public float acceleration;
    public float deceleration;
    public float turnAroundAcceleration;
    public float airControl;
    
    public float timeAfterAirbornToDecelerate;
    
    public AnimationCurve decelerationCurveLinkedToAirbornVelocity;
    public AnimationCurve accelerationLinkedToVelocity;
}
