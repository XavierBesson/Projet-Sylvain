using UnityEngine;

public class Path : MonoBehaviour
{
    public Transform[] _waypoints;

    public int Length => _waypoints.Length;


    public Vector3 GetPoint(int index)
    {
        return _waypoints[index].position;
    }


    private void OnDrawGizmos()
    {
        if (_waypoints == null || _waypoints.Length < _waypoints.Length-1) return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < _waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(_waypoints[i].position, _waypoints[i + 1].position);
        }
    }
}