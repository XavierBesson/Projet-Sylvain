using UnityEngine;

public class FPS : MonoBehaviour
{
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "FPS: " + (1f / Time.unscaledDeltaTime).ToString("0"));
    }
}