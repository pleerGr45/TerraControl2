using TC_func;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BaseCameraMovment : MonoBehaviour
{
    [SerializeField] private float movement_speed;
    [SerializeField] private float rotation_speed;
    [SerializeField] private float max_orthographic_size;
    [SerializeField] private float min_orthographic_size;
    [SerializeField] private float time;
    [SerializeField] private Vector3 new_position;

    void Start()
    {
        new_position = transform.position;
    }

    void Update() {
        HandleMovementInput();
        HandleRotationInput();
        HandleSizeInput();
    }

    void HandleMovementInput() {
        if (Control.GetKeyControl("camera_move_forward"))
            new_position += (transform.forward * movement_speed);
        if (Control.GetKeyControl("camera_move_back"))
            new_position += (transform.forward * -movement_speed);
        if (Control.GetKeyControl("camera_move_right"))
            new_position += (transform.right * movement_speed);
        if (Control.GetKeyControl("camera_move_left"))
            new_position += (transform.right * -movement_speed);
        
        transform.position = Vector3.Lerp(transform.position, new_position, Time.deltaTime * time);

    }

    void HandleRotationInput() {
        if (Control.GetKeyControl("camera_rotate_right"))
            transform.Rotate(0, rotation_speed, 0);
        if (Control.GetKeyControl("camera_rotate_left"))
            transform.Rotate(0, -rotation_speed, 0);
    }

    void HandleSizeInput() {
        if (Control.GetKeyControl("camera_zoom_in")) {
            Camera cam = transform.GetChild(0).GetComponent<Camera>();
            if(cam.orthographicSize - 0.2f >= 7.0f) {
                cam.orthographicSize = cam.orthographicSize - 0.2f;
            }
        }
        if (Control.GetKeyControl("camera_zoom_out")) {
            Camera cam = transform.GetChild(0).GetComponent<Camera>();
            if(cam.orthographicSize + 0.2f <= 30.0f)
                cam.orthographicSize = cam.orthographicSize + 0.2f;
        }
    }
}
