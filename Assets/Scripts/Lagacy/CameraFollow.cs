using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    public Vector3 goalPosition;
    private float speed = 10f;

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, goalPosition, speed* Time.deltaTime);
    }

    public void UpdateGoalPosition(Vector3 newPosition)
    {
        goalPosition = newPosition;
    }
}
