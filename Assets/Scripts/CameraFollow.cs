using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private bool bSetHeroCharacterAsTarget = true;
    [SerializeField]
    private Transform Target;
    [SerializeField]
    private Vector3 CameraPositionOffset;
    [SerializeField, Range(1, 10)]
    private float SmoothFactor = 3.0f;

    private void Start()
    {
        if (bSetHeroCharacterAsTarget)
        {
            Target = TempoManager.Instance.GetHeroCharacter().transform;
        }
    }

    private void Update()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        Vector3 TargetPostion = transform.position = Target.position + CameraPositionOffset;
        Vector3 SmoothPositon = Vector3.Lerp(transform.position, TargetPostion, SmoothFactor * Time.fixedDeltaTime);
        transform.position =  SmoothPositon;
    }
}
