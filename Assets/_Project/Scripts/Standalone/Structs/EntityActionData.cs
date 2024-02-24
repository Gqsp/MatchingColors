public struct EntityActionData
{
    public float movement;
    public int actionRequests;
    public int previousFrameInputs;

    public bool Performed(EntityActionRequest type)
    {
        return (actionRequests & (1 << (int)type)) != 0;
    }

    public bool PerformedLastFrame(EntityActionRequest type)
    {
        return (previousFrameInputs & (1 << (int)type)) != 0;
    }

    public bool Started(EntityActionRequest type)
    {
        return Performed(type) && !PerformedLastFrame(type);
    }

    public bool Canceled(EntityActionRequest type)
    {
        return PerformedLastFrame(type) && !Performed(type);
    }
}
