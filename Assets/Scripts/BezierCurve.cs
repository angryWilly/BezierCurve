using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    [SerializeField] private PointsSpawner _spawner;
    
    private void OnDrawGizmos()
    {
        if (_spawner.Anchors.Count < 2) return;

        DrawBezier();
    }

    private void DrawBezier()
    {
        for (var i = 0; i < _spawner.NumberOfSegments; i++)
        {
            var pointsInSegment = _spawner.GetPointsInSegment(i);
            var previousPointPosition = pointsInSegment[0].position;
            
            const int lineIntervals = 20;
            for (var j = 0; j < lineIntervals + 1; j++)
            {
                Gizmos.color = Color.green;
                var t = (float)j / lineIntervals;

                var point = GetPoint(pointsInSegment[0].position, pointsInSegment[1].position,
                    pointsInSegment[2].position, pointsInSegment[3].position, t);
                Gizmos.DrawLine(previousPointPosition, point);
                previousPointPosition = point;
            }
        }
    }

    private Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        var oneMinusT = 1f - t;

        return Mathf.Pow(oneMinusT, 3) * p0 +
               3f * Mathf.Pow(oneMinusT, 2) * t * p1 + 
               3f * oneMinusT * Mathf.Pow(t,2) * p2 +
               Mathf.Pow(t, 3) * p3;
    }
}
