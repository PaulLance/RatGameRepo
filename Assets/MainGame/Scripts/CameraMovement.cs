/*
 * Script by Bizzy (Bizzy Bee Creator)
 * License: Contact me for acquiring one!
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraMovement : MonoBehaviour
{
    public Transform cameraTarget;

    public float cameraMoveSpeed = 1.5f;
    public float cameraZoomSpeed = 0.2f;
    public float SmoothFactor = 0.1f;
    public float SmoothFactorXAuto = 0.2f;
    public float SmoothFactorYAuto = 0.3f;
    public float SmoothFactorZAuto = 0.2f;
    public float SmoothFactorLookAt = 0.5f;
    float xVelAuto, yVelAuto, zVelAuto;

    const float ZOOM_MULTIPLIER = 5;

    public float minAngle = -89.999f;
    public float maxAngle = 89.999f;

    public float minDistance = 0.5f;
    public float maxDistance = 16f;

    public float distance = 2.5f;

    public float camForwardExtra = 0.5f;

    bool holdingCameraKey = false;

    public Vector3 cameraMoveDirection = Vector2.zero;
    public Vector3 lastMousePos = Vector2.zero;

    public Vector3 camOffset = new Vector3(0, 2, -2.5f);
    public Vector3 targetOffset = new Vector3(0, 1.25f, 0f);


    public Texture2D cursor_default, cursor_turn;

    public LayerMask rayLayers;

    private Vector3 velocity = Vector3.zero;

    private Vector3 lastLookAt = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        if (cameraTarget != null)
        {
            InitStuff();
        }
    }

    void InitStuff()
    {
        lastMousePos = Input.mousePosition;
        distance = camOffset.magnitude;
        transform.position = cameraTarget.position + targetOffset + camOffset;
        transform.LookAt(cameraTarget.position + targetOffset);
    }

    public void Initialize(Transform transform)
    {
        cameraTarget = transform;
        InitStuff();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GetComponent<AudioListener>().enabled = !GetComponent<AudioListener>().enabled;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cameraTarget == null) return;

        Vector2 moveDir = Vector2.zero;
        bool zoomCamera = false;
#if UNITY_EDITOR || !UNITY_PSP2
        zoomCamera = (Input.mouseScrollDelta.y != 0);
        if (!holdingCameraKey)
        {
            if (Input.GetMouseButton(1))
            {
                holdingCameraKey = true;
                if (cursor_turn != null)
                    Cursor.SetCursor(cursor_turn, new Vector2(cursor_turn.width / 2, cursor_turn.height / 2), CursorMode.Auto);
            }
        }
        else
        {
            if (!Input.GetMouseButton(1))
            {
                holdingCameraKey = false;
                if (cursor_default != null)
                    Cursor.SetCursor(cursor_default, Vector2.zero, CursorMode.Auto);
            }
        }
#else
        zoomCamera = Input.GetButton("Triangle");
        holdingCameraKey = true;
#endif
        if (holdingCameraKey)
        {
            //cameraMoveDirection = Input.mousePosition - lastMousePos;
            //transform.RotateAround(cameraTarget.position, Vector3.up, cameraMoveDirection.x * cameraMoveSpeed * Time.deltaTime);
            //transform.RotateAround(cameraTarget.position, transform.right, cameraMoveDirection.y * cameraMoveSpeed * Time.deltaTime);
            //camOffset = (transform.position - cameraTarget.position).normalized * distance;

#if UNITY_EDITOR || !UNITY_PSP2
            moveDir.x = Input.GetAxisRaw("Mouse X");
            moveDir.y = -Input.GetAxisRaw("Mouse Y");
#else
            moveDir.x = Input.GetAxisRaw("Right Stick Horizontal");
            moveDir.y = -Input.GetAxisRaw("Right Stick Vertical");
            if (zoomCamera)
            {
                moveDir = Vector2.zero;
            }
#endif

            Quaternion camTurnAngleX =
                Quaternion.AngleAxis(moveDir.x * cameraMoveSpeed, Vector3.up);

            Quaternion camTurnAngleY =
                Quaternion.AngleAxis(moveDir.y * cameraMoveSpeed, transform.right);

            camOffset = camTurnAngleX * camOffset;
            camOffset = camTurnAngleY * camOffset;
        }




        /*
        Vector3 newPos = cameraTarget.position + camOffset;
        Vector3 newHelperPos = new Vector3(newPos.x, transform.position.y, newPos.z);
        newPos = Vector3.Slerp(transform.position, newHelperPos, SmoothFactor * Time.deltaTime);
        transform.position = newPos;
        transform.LookAt(cameraTarget);
        */

        if (zoomCamera)
        {
#if UNITY_EDITOR || !UNITY_PSP2
            distance -= Input.mouseScrollDelta.y * ZOOM_MULTIPLIER * cameraZoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            camOffset = camOffset.normalized * distance;
#else
            float zoomAmount = -Input.GetAxisRaw("Right Stick Vertical");
            distance -= zoomAmount * ZOOM_MULTIPLIER * cameraZoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            camOffset = camOffset.normalized * distance;
#endif
        }

        Vector3 newPos = cameraTarget.position + targetOffset + camOffset;

        // pull cam forward
        RaycastHit rayhitInfo;
        //Ray r = new Ray(cameraTarget.position + targetOffset, (cameraTarget.position + targetOffset - newPos).normalized);
        //if(Physics.Raycast(r, out rayhitInfo, 999, rayLayers))
        if (Physics.Linecast(cameraTarget.position + targetOffset, newPos, out rayhitInfo, rayLayers))
        {
            Vector3 dir = cameraTarget.position + targetOffset - rayhitInfo.point;
            //Debug.Log("Hit : " + rayhitInfo.transform.name + " : " + dir.magnitude + " <> " + minDistance);
            if (dir.magnitude >= minDistance)
            {
                newPos = rayhitInfo.point + (dir.normalized * camForwardExtra);
            }
        }
        else
        {

        }

        //Vector3 newPos2 = new Vector3(newPos.x, transform.position.y, newPos.z);
        if (moveDir.magnitude > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, SmoothFactor);
        }
        else
        {
            Vector3 newPosAdjusted = Vector3.zero;
            newPosAdjusted.x = Mathf.SmoothDamp(transform.position.x, newPos.x, ref xVelAuto, SmoothFactorXAuto * Time.deltaTime);
            newPosAdjusted.y = Mathf.SmoothDamp(transform.position.y, newPos.y, ref yVelAuto, SmoothFactorYAuto * Time.deltaTime);
            newPosAdjusted.z = Mathf.SmoothDamp(transform.position.z, newPos.z, ref zVelAuto, SmoothFactorZAuto * Time.deltaTime);
            transform.position = newPosAdjusted;
        }

        Vector3 currentLookAt = cameraTarget.position + targetOffset;

        transform.LookAt(Vector3.Lerp(lastLookAt, currentLookAt, SmoothFactorLookAt * Time.deltaTime));
        lastLookAt = currentLookAt;
        //transform.LookAt(cameraTarget);
    }


}
