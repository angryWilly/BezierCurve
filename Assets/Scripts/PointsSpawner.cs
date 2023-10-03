using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PointsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _anchorPrefab;
    [SerializeField] private GameObject _controlPrefab;
    
    private Camera _camera;
    public List<Transform> Anchors { get; } = new();
    private List<Transform> Controls { get; } = new();

    public Transform[] GetPointsInSegment(int index)
    {
        if (index == 0)
        {
            return new[] { Anchors[index], Controls[index], Controls[index + 1], Anchors[index + 1] };
        }

        return new[]
            { Anchors[index], Controls[3 + 2 * (index - 1)], Controls[2 + 2 * (index - 1)], Anchors[index + 1] };

        /*return new Transform[] { Anchors[1], Controls[3], Controls[2], Anchors[2] };
        return new Transform[] { Anchors[2], Controls[5], Controls[4], Anchors[3] };
        return new Transform[] { Anchors[3], Controls[7], Controls[6], Anchors[4] };*/
    }

    public int NumberOfSegments => Anchors.Count - 1;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            SpawnPointsOnMousePosition();
        }
    }

    private void SpawnPointsOnMousePosition()
    {
        var mouseWorldPosition = _camera!.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        var point = Instantiate(_anchorPrefab, mouseWorldPosition, quaternion.identity);

        Anchors.Add(point.transform);
        Controls.Add(point.transform.GetChild(0).transform);

        if (Anchors.Count > 2)
        {
            SpawnControlPoint();
            UpdateLastControlPoint();
        }
    }

    private void UpdateLastControlPoint()
    {
        var newLastControlPointPosition = (Anchors[^2].GetChild(2).position + Anchors[^1].position) / 2;
        Anchors[^1].GetChild(0).position = newLastControlPointPosition;
    }

    private void SpawnControlPoint()
    {
        var previousAnchor = Anchors[^2];
        var previousAnchorPointPosition = previousAnchor.position;
        var previousControlPointPosition = previousAnchor.GetChild(0).position;
        var newControlSpawnPosition = 2 * previousAnchorPointPosition - previousControlPointPosition;
        
        var controlPoint = Instantiate(_controlPrefab, newControlSpawnPosition, Quaternion.identity, previousAnchor);
        Controls.Add(controlPoint.transform);

        ConnectLineToNewControlPoint(previousAnchor, controlPoint.transform);
    }

    private void ConnectLineToNewControlPoint(Transform previousAnchor, Transform newControlPoint)
    {
        var line = previousAnchor.GetChild(1).GetComponent<Line>();
        line.SetNewPosition(newControlPoint);
    }
}
