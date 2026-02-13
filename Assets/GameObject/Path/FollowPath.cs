using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private Path path;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float reachDistance = 0.2f;
    [SerializeField] private bool loop = false;

    private int currentIndex = 0;



    public void ActivateFollowPath()
    {
        if (path == null || path.Length == 0) return;

        Vector3 target = path.GetPoint(currentIndex);
        Vector3 dir = (target - transform.position).normalized;

        transform.position += dir * speed * Time.deltaTime;

        // Rotation vers le point
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);

        if (Vector3.Distance(transform.position, target) < reachDistance)
        {
            currentIndex++;

            if (currentIndex >= path.Length)
            {
                if (loop)
                    currentIndex = 0;
                else
                    GameManager.Instance.GameLoop -= ActivateFollowPath;
            }
        }
    }


}