using UnityEngine;

//Credits: https://www.youtube.com/watch?v=7BVAlYrM2FU
public class CameraShake : MonoBehaviour
{

    [SerializeField] private float shakeAmount = 0.02f;
    private Vector3 initialPos;
    void Awake()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = initialPos + Random.insideUnitSphere * shakeAmount;
    }

    public void ShakeCamera()
    {
        transform.position = initialPos + Random.insideUnitSphere * shakeAmount;
    }
}
