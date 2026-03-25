using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private Path _path;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _reachDistance = 0.2f;
    [SerializeField] private bool _loop = false;

    private int _currentIndex = 0;



    public void ActivateFollowPath()
    {
        if (_path == null || _path.Length == 0) return;

        Vector3 target = _path.GetPoint(_currentIndex);
        Vector3 dir = (target - transform.position).normalized;

        transform.position += dir * _speed * Time.deltaTime;

        // Rotation vers le point
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);

        if (Vector3.Distance(transform.position, target) < _reachDistance)
        {
            _currentIndex++;

            if (_currentIndex >= _path.Length)
            {
                if (_loop)
                    _currentIndex = 0;
                else
                    GameManager.Instance.GameLoop -= ActivateFollowPath;
            }
        }
    }


}