
using UnityEngine;
public class TriggerBox_Camera : MonoBehaviour
{
    public ThirdPersonController camcontrol;
    public GameObject[] cameras; // Assign cameras in the Inspector
    public int activeCameraIndex = 0; // Set which camera to activate (default: first one)
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "P1")
        {
            //-Deactivate all cameras
            foreach (GameObject cam in cameras)
                
        {
            cam.SetActive(false);
            //-Activate only the chosen camera
        }
if (cameras.Length > 0 && activeCameraIndex >= 0 && activeCameraIndex < cameras.Length)
            {
                    cameras[activeCameraIndex].SetActive(true);
                    camcontrol.cameraTransform = cameras[activeCameraIndex].transform;
                }
            }
        }
}