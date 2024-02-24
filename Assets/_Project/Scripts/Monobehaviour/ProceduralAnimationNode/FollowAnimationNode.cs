using GaspDL.Utils;
using UnityEngine;

public class FollowAnimationNode : MonoBehaviour
{
    public FollowAnimationNode followTarget;
    public float damping;
    public float lowestDamping;
    public float anuglarDamping;
    public float maxDistance;
    public float currentAngle;
    public float distanceToTarget;
    public float preferredAngle;
    public float defaultAngle;
    public float minVelocityToReajustAngle;

    public bool hasRotationAnimation;
    public AnimationCurve rotationAnimation;
    public float rotationAmplitude;
    public float rotationSpeed;

    Vector2 velocity;
    Vector2 lastFramePos;
    float angularVelocity;

    private void Start()
    {
        if (followTarget != null)
        {
            var dir = transform.position - followTarget.transform.position;
            followTarget.distanceToTarget = dir.magnitude;
            followTarget.defaultAngle = Vector2.Angle(dir, Vector2.up) * -Mathf.Sign(dir.x);
            followTarget.currentAngle = followTarget.defaultAngle;
            followTarget.preferredAngle = followTarget.defaultAngle;
        }
    }

    float progress;

    private void Update()
    {
        if (followTarget != null)
        {
            var dirToFollowTargetCenter = transform.position - followTarget.transform.position;
            var dirToFollowTargetPosition = (Vector2)transform.position - followTarget.GetPosition();
            var lerp = Mathf.InverseLerp(distanceToTarget, maxDistance, dirToFollowTargetCenter.magnitude);
            var unclampedDamp = damping * (1 - Mathf.Clamp01(lerp));
            var damp = Mathf.Max(lowestDamping, unclampedDamp);
            Debug.Log(dirToFollowTargetCenter.magnitude + " " + lerp + " " + unclampedDamp + " " + damp);
            var targetPosition = Vector2.SmoothDamp(transform.position, followTarget.GetPosition(), ref velocity, damp * Time.deltaTime);
            transform.position = targetPosition;

            var currentFacingAngle = Vector2.Angle(dirToFollowTargetCenter, Vector2.up) * -Mathf.Sign(dirToFollowTargetCenter.x);
            TryUpdateAngle(currentFacingAngle);
            //followTarget.currentAngle = Mathf.SmoothDampAngle(currentAngle, currentFacingAngle, ref angularVelocity, anuglarDamping / 2 * Time.deltaTime, 100f);
        }
        else
        {
            velocity = ((Vector2)transform.position - lastFramePos).normalized * 1 / Time.deltaTime;
            lastFramePos = transform.position;
        }

        if (hasRotationAnimation)
        {
            progress = (progress + Time.deltaTime * rotationSpeed) % 1;
            preferredAngle = defaultAngle + rotationAnimation.Evaluate(progress) * rotationAmplitude;
        }
    }

    private void LateUpdate()
    {
        if (velocity.magnitude <= minVelocityToReajustAngle)
            currentAngle = Mathf.SmoothDamp(currentAngle, preferredAngle, ref angularVelocity, anuglarDamping);
    }

    public void TryUpdateAngle(float angle)
    {
        if (followTarget == null && velocity.magnitude > minVelocityToReajustAngle) followTarget.currentAngle = angle;
        if (followTarget.velocity.magnitude > minVelocityToReajustAngle) followTarget.currentAngle = angle;// Mathf.SmoothDampAngle(currentAngle, angle, ref angularVelocity, anuglarDamping / 2 * Time.deltaTime, 100f);
    }

    public Vector2 GetPosition()
    {
        return (Vector2)transform.position + Vector2.up.Rotate(currentAngle).normalized * distanceToTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.up.Rotate(preferredAngle).normalized * distanceToTarget);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, GetPosition());
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.up.Rotate(defaultAngle).normalized * distanceToTarget);
    }
}
