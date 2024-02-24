public class EntityStateResolverResponse
{
    public bool validThisFrame;
    public bool validPreviousFrame;

    public void UpdateValue(bool valid)
    {
        validPreviousFrame = validThisFrame;
        validThisFrame = valid;
    }
}
