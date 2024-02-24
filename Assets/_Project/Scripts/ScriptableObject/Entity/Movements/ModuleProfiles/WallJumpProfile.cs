using UnityEngine;

[CreateAssetMenu(menuName = "Collections/ModuleProfile/WallJump")]
public class WallJumpProfile : ModuleProfile
{
    public float verticalForce;
    public float verticalAttenuation;
    public float jumpCutOff;
    public float jumpBuffer;
    
    public float horizontalForce;

    public float gravityScale;

}
