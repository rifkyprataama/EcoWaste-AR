using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    void LateUpdate()
    {
        // Memastikan ada kamera yang aktif
        if (Camera.main != null)
        {
            // Memaksa papan teks agar rotasinya selalu sejajar/menghadap kamera
            transform.forward = Camera.main.transform.forward;
        }
    }
}