using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerTransform;
    public Transform pivot;
    private Vector3 _cameraOffset;

    private float rotationSpeed = 1;

    private float maxViewAngle = 70f;
    private float minViewAngle = -30;

    private bool invertYAxis = true;
    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        //gambiarra to prevent camera bug (if player spawn not facing +z)
        playerTransform.LookAt(new Vector3(0, playerTransform.position.y, 1));

        Camera.main.transform.parent = null;
        _cameraOffset = playerTransform.position - transform.position;
        pivot.transform.position = playerTransform.transform.position;

    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        //Get the X position of the mouse and rotate the target
        float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
        // playerTransform.Rotate(0, horizontal, 0);
        pivot.Rotate(0, horizontal, 0);
        
        //Get the y position of the mouse and rotate the pivot
        float vertical = Input.GetAxis("Mouse Y") * rotationSpeed;
        if (invertYAxis) {
            pivot.Rotate(vertical, 0, 0);
        } else {
            pivot.Rotate(-vertical, 0, 0);
        }
          
        //Move the camera based on the current rotation of the target and the original offset
        float desiredYAngle = pivot.eulerAngles.y;
        float desiredXAngle = pivot.eulerAngles.x;
        rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        //Rotation bugs when player doesn't start facing +z -> solution: force spawn facing +z
        transform.position = playerTransform.position - (rotation * _cameraOffset);

        //Limit up/down camera rotation
        if (pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180 ) {
            pivot.rotation = Quaternion.Euler(maxViewAngle, pivot.rotation.eulerAngles.y, pivot.rotation.eulerAngles.z);
        }
        if (pivot.rotation.eulerAngles.x > 180 && pivot.rotation.eulerAngles.x < 360f + minViewAngle ) {
            pivot.rotation = Quaternion.Euler(360f + minViewAngle, pivot.rotation.eulerAngles.y, pivot.rotation.eulerAngles.z);
        }

        //Camera above ground
        if (transform.position.y < playerTransform.position.y) {
            transform.position += new Vector3(transform.position.x, playerTransform.position.y - 0.5f, transform.position.z);
        }

        transform.LookAt(playerTransform.position + new Vector3(0, 2.5f, 0));

    }
}
