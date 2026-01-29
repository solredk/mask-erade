using Unity.Cinemachine;
using UnityEngine;

public class ShakeEnable : MonoBehaviour
{
    public void EnableCameraShake()
    {
        GetComponent<CinemachineBasicMultiChannelPerlin>().enabled = true;
    }

    public void DisableCameraShake()
    {
        GetComponent<CinemachineBasicMultiChannelPerlin>().enabled = false;
    }
}
