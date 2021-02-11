using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidController : MonoBehaviour
{
    [SerializeField] private bool upperBodyOnly;
    [SerializeField] private bool calibrationStep;

    [SerializeField] private float Speed = 50, DeviationThreshold = 25, TargetPosDistance = 0.47f;

    private BoundsManager boundsManager;
    //targetTransform = HandTransform
    private Transform HandIkTargetTransform, LegIkTargetTransform, ChestTransform;
    private bool isOutsideBounds = false;
    private float initYpos, yOffset, ArmLength, DistanceBtwLegs, DirectionFromTargetMagnitude,DeviationFromForwardVector;
    [SerializeField]private float StandingAmount=0, BlendX=0, BlendY=0;
    [SerializeField] Vector3 DirectionToNewPos;

    private Vector3 projectedHandIkTargetPos, projectedHandIkTargetForward, DirectionToHandIkTarget;
    private Vector3 bodyTargetPos, bodyTargetOrientation, leftLegTargetPos, rightLegTargetPos;

    private Animator HumanoidAnimator;

    private void Awake()
    {
        HumanoidAnimator = GetComponent<Animator>();
        ChestTransform = HumanoidAnimator.GetBoneTransform(HumanBodyBones.UpperChest);
        initYpos = transform.position.y;        
    }

    private void Update()
    {
        if (!HandIkTargetTransform || !ChestTransform || !isOutsideBounds) return;
        yOffset = ChestTransform.position.y - HandIkTargetTransform.position.y;

        if (Mathf.Abs(yOffset) > 0.1f)
        {
                StandingAmount = Mathf.MoveTowards(StandingAmount, (Mathf.Sign(yOffset) + 1) * 0.5f, Time.deltaTime * 2);
                HumanoidAnimator.SetFloat("Stand", StandingAmount);
        }



        if (isArmOutofNormalRange())
        {
            SetNewTargetValues();

            if (!upperBodyOnly)
            {
                DirectionToNewPos = transform.InverseTransformPoint(bodyTargetPos);
                DirectionToNewPos = DirectionToNewPos.normalized * -1;
                float deltaMul = (BlendX < 0.2f || BlendY < 0.4f) ? 10 : 1;
                BlendX = Mathf.MoveTowards(BlendX, DirectionToNewPos.x, Time.deltaTime * deltaMul);
                BlendY = Mathf.MoveTowards(BlendY, DirectionToNewPos.z, Time.deltaTime * deltaMul);
                HumanoidAnimator.SetFloat("BlendX", BlendX);
                HumanoidAnimator.SetFloat("BlendY", BlendY);
            }

            float interpolationPoint = Time.deltaTime * Speed * 0.1f;
            transform.position = Vector3.Lerp(transform.position, bodyTargetPos, interpolationPoint);            
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(bodyTargetOrientation), interpolationPoint);
            //LegIkTargetTransform.rotation = transform.rotation;

            //Vector3 legPos = transform.position;
            //legPos.y = LegIkTargetTransform.position.y;
            //LegIkTargetTransform.position = legPos;
        }
        else if(BlendX!=0 || BlendY!=0)
        {
            float interpolationPoint = Time.deltaTime * Speed * 0.2f;
            transform.position = Vector3.Lerp(transform.position, bodyTargetPos, interpolationPoint);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(bodyTargetOrientation), interpolationPoint);

            if (!upperBodyOnly)
            {
                float deltaMul = (BlendX < 0.4f || BlendY < 0.4f) ? 1.5f : 1;
                BlendX = Mathf.MoveTowards(BlendX, 0, Time.deltaTime * 5 * deltaMul);
                BlendY = Mathf.MoveTowards(BlendY, 0, Time.deltaTime * 5 * deltaMul);
                HumanoidAnimator.SetFloat("BlendX", BlendX);
                HumanoidAnimator.SetFloat("BlendY", BlendY);
            }
        }
    }

    private bool isArmOutofNormalRange()
    {
        SetProjectedHandTransformValues();
        DirectionToHandIkTarget = projectedHandIkTargetPos - transform.position;
        DirectionFromTargetMagnitude = DirectionToHandIkTarget.magnitude;
        DeviationFromForwardVector = Vector3.Angle(transform.forward, DirectionToHandIkTarget);
        bool isExtension = DirectionFromTargetMagnitude >= ArmLength * 0.75f || DirectionFromTargetMagnitude <= ArmLength * 0.6f;
        bool isDeviating = DeviationFromForwardVector >= DeviationThreshold;
        return isExtension || isDeviating;
    }

    private void SetProjectedHandTransformValues()
    {
        projectedHandIkTargetPos = HandIkTargetTransform.position;
        projectedHandIkTargetPos.y = transform.position.y;

        projectedHandIkTargetForward = HandIkTargetTransform.forward;
        projectedHandIkTargetForward.y = 0;
        projectedHandIkTargetForward = projectedHandIkTargetForward.normalized;
    }

    private void SetNewTargetValues()
    {
        bodyTargetPos = projectedHandIkTargetPos - projectedHandIkTargetForward * TargetPosDistance;
        bodyTargetOrientation = transform.rotation.eulerAngles;
        bodyTargetOrientation.y = HandIkTargetTransform.rotation.eulerAngles.y;
        Vector3 leftDirection = Vector3.Cross(projectedHandIkTargetForward,Vector3.up);
        leftLegTargetPos = bodyTargetPos + leftDirection * DistanceBtwLegs * 0.5f;
        rightLegTargetPos = bodyTargetPos - leftDirection * DistanceBtwLegs * 0.5f;
    }

    public void SetDependecies(BoundsManager b, Transform handTarget, Transform legTarget)
    {
        HumanoidAnimator = GetComponent<Animator>();
        ChestTransform = HumanoidAnimator.GetBoneTransform(HumanBodyBones.UpperChest);

        boundsManager = b;
        HandIkTargetTransform = handTarget;
        LegIkTargetTransform = legTarget;
        ArmLength = (ChestTransform.position - HandIkTargetTransform.position).magnitude;
        DistanceBtwLegs = (LegIkTargetTransform.GetChild(0).position - LegIkTargetTransform.GetChild(1).position).magnitude;
        boundsManager.OnPhoneEnter += PhoneEnterHandler;
        boundsManager.OnPhoneExit += PhoneExitHandler;
    }

    public void SetDependeciesBasic(Transform handTarget, Transform legTarget)
    {
        HumanoidAnimator = GetComponent<Animator>();
        ChestTransform = HumanoidAnimator.GetBoneTransform(HumanBodyBones.UpperChest);

        //boundsManager = b;
        HandIkTargetTransform = handTarget;
        LegIkTargetTransform = legTarget;
        ArmLength = (ChestTransform.position - HandIkTargetTransform.position).magnitude;
        DistanceBtwLegs = (LegIkTargetTransform.GetChild(0).position - LegIkTargetTransform.GetChild(1).position).magnitude;
        //boundsManager.OnPhoneEnter += PhoneEnterHandler;
        //boundsManager.OnPhoneExit += PhoneExitHandler;
        Debug.Log("Dependencies set, marking state as out of bounds...");
        isOutsideBounds = true;
    }

    private void OnDrawGizmos()
    {
        SetProjectedHandTransformValues();
        SetNewTargetValues();

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.05f);
        Gizmos.DrawWireSphere(projectedHandIkTargetPos, 0.05f);
        Gizmos.DrawLine(transform.position, projectedHandIkTargetPos);

        ////Draw projected forward
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(projectedHandIkTargetPos, projectedHandIkTargetPos + projectedHandIkTargetForward);

        Vector3 DirectionToNewPos = bodyTargetPos - (transform.position + transform.forward);
        //Draw projected forward
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position+DirectionToNewPos);

        //Draw target positions
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(bodyTargetPos, 0.03f);
        Gizmos.DrawWireSphere(leftLegTargetPos, 0.02f);
        Gizmos.DrawLine(bodyTargetPos, leftLegTargetPos);
    }

    void OnEnable()
    {
        if (!boundsManager) return;
        boundsManager.OnPhoneEnter += PhoneEnterHandler;
        boundsManager.OnPhoneExit += PhoneExitHandler;
    }

    void PhoneExitHandler()
    {
        isOutsideBounds = true;
        Debug.Log("phone exit");
    }

    void PhoneEnterHandler()
    {
        isOutsideBounds = true;       
    }

    private void OnDisable()
    {
        if (!boundsManager) return;
        boundsManager.OnPhoneEnter -= PhoneEnterHandler;
        boundsManager.OnPhoneExit -= PhoneExitHandler;
    }

}
