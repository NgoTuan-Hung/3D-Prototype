class RecommendedAIBehavior
{
    private float maxDistanceToTarget = 0f;
    private float minDistanceToTarget = 0f;
    private bool isLookingAtTarget = false;

    public float MaxDistanceToTarget { get => maxDistanceToTarget; set => maxDistanceToTarget = value; }
    public float MinDistanceToTarget { get => minDistanceToTarget; set => minDistanceToTarget = value; }
    public bool IsLookingAtTarget { get => isLookingAtTarget; set => isLookingAtTarget = value; }
}