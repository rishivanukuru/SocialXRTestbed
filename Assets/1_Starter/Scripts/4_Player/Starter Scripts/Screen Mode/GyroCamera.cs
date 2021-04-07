using UnityEngine;
using UnityEngine.Android;
using System.Collections;

public class GyroCamera : MonoBehaviour
{
    // Disclaimer: I don't understand everything this script does. 
    // For now, just stick it on a gameobject, and if you're on Phone, the gameobject will rotate according to the direction the phone is facing
    // Assuming a landscape left, outward facing direction

    // STATE
    private float _initialYAngle = 0f;
    private float _appliedGyroYAngle = 0f;
    private float _calibrationYAngle = 0f;
    private Transform _rawGyroRotation;
    private float _tempSmoothing;

    // WAIT
    public float waitBuffer = 0.1f;

    // SETTINGS
    [SerializeField] private float _smoothing = 0.1f;

    // BODY
    public GameObject playerBody; //In case there's another body to be rotated

    // DO
    private bool isPhone;

    private IEnumerator Start()
    {

        //Only do this for phones
#if UNITY_EDITOR
        isPhone = false;
#else
       isPhone = true;
#endif
        if(isPhone)
        {
            Input.gyro.enabled = true;
            Application.targetFrameRate = 60;
            _initialYAngle = transform.eulerAngles.y;

            _rawGyroRotation = new GameObject("GyroRaw").transform;
            _rawGyroRotation.position = transform.position;
            _rawGyroRotation.rotation = transform.rotation;

            // Wait until gyro is active, then calibrate to reset starting rotation.
            yield return new WaitForSeconds(waitBuffer);

            StartCoroutine(CalibrateYAngle());
        }

    }

    private void Update()
    {

        if(isPhone)
        {
            ApplyGyroRotation();
            ApplyCalibration();
            transform.rotation = Quaternion.Slerp(transform.rotation, _rawGyroRotation.rotation, _smoothing);
            if (playerBody)
            {
                //playerBody.transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
                playerBody.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            }
        }

    }

    public void CalibrateFunction() //Calibrates the forward direction
    {
        if(isPhone)
        StartCoroutine(CalibrateYAngle());
    }



    private IEnumerator CalibrateYAngle()
    {
        _tempSmoothing = _smoothing;
        _smoothing = 1;
        _calibrationYAngle = _appliedGyroYAngle - _initialYAngle; // Offsets the y angle in case it wasn't 0 at edit time.
        yield return null;
        _smoothing = _tempSmoothing;
    }

    private void ApplyGyroRotation()
    {
        _rawGyroRotation.rotation = Input.gyro.attitude;
        _rawGyroRotation.Rotate(0f, 0f, 180f, Space.Self); // Swap "handedness" of quaternion from gyro.
        _rawGyroRotation.Rotate(90f, 180f, 0f, Space.World); // Rotate to make sense as a camera pointing out the back of your device.
        _appliedGyroYAngle = _rawGyroRotation.eulerAngles.y; // Save the angle around y axis for use in calibration.
    }

    private void ApplyCalibration()
    {
        _rawGyroRotation.Rotate(0f, -_calibrationYAngle, 0f, Space.World); // Rotates y angle back however much it deviated when calibrationYAngle was saved.
    }

    public void SetEnabled(bool value)
    {
        enabled = true;
        StartCoroutine(CalibrateYAngle());
    }
}