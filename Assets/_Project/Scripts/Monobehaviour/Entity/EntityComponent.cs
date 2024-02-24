using UnityEngine;

public class EntityComponent : MonoBehaviour
{
    protected EntityCore core;
    public void Setup(EntityCore core)
    {
        this.core = core;
    }
}
