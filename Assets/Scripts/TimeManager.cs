
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public float slowDownFactor;
    public float slowDownLength;

    private void Update()
    {
        Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void SlowMotion()
    {
        Time.timeScale = slowDownFactor;
    }
}



/*CameraFocusControl(25f, 90f, 23f, 10f, .5f, .85f, .1f, .1f);
 CameraFocusControl(45, 0, 10, 23, .85f, .5f, 0, 0);
cam.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(25f, 90, 0));*/
