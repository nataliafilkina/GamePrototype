using UnityEngine;

public class DependentTransform : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Transform _mainTransform;

    private Vector3 _position;
    private Vector3 _forward;
    private Vector3 _up;

    #endregion

    private void Start()
    {
        _position = _mainTransform.transform.InverseTransformPoint(transform.position);
        _forward = _mainTransform.transform.InverseTransformDirection(transform.forward);
        _up = _mainTransform.transform.InverseTransformDirection(transform.up);
    }

    private void Update()
    {
        var newPosition = _mainTransform.transform.TransformPoint(_position);
        var newForward = _mainTransform.transform.TransformDirection(_forward);
        var newUp = _mainTransform.transform.TransformDirection(_up);
        var newRotation = Quaternion.LookRotation(newForward, newUp);

        transform.position = newPosition;
        transform.rotation = newRotation;
    }
}
