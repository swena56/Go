using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Torque")]
[RequireComponent(typeof(Rigidbody))]

/**
 * MouseTorque.cs - a mouselook implementation using torque
 * 
 * All of the smooth mouse look scripts I found used input averages over the last several frames, which
 * ends up not being all that smooth really.  This camera script uses rotational forces (torques) to
 * rotate the camera in response to mouse movement.
 * 
 * The caveat of this camera is that it is possible for the camera to become slightly unstable.  Use
 * the rigidbody properties (mainly angular drag) as well as the correctiveStrength variable to affect
 * the stability of the rotations.
 * 
 * Note: make sure that "Use Gravity" is unchecked in the rigidbody settings.
 * Note: use an angular drag of about 5 or 6.
 * Note: setting a sensitivity value to a negative value inverts that axis.
 * 
 * Author: Robert Grant
 */
public class MouseTorque : MonoBehaviour
{
    /** Controls how sensitive the horizontal axis is. */
    public float horizontalSensitivity = 30;

    /** Controls how sensitive the vertical axis is. */
    public float verticalSensitivity = 30;

    /** Controls how strongly the camera tries to keep itself upright. */
    public float correctiveStrength = 20;

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddTorque(0, Input.GetAxis("Mouse X") * horizontalSensitivity, 0);
        GetComponent<Rigidbody>().AddRelativeTorque(Input.GetAxis("Mouse Y") * verticalSensitivity, 0, 0);

        // Adding the two forces above creates some wobble that causes the camera to become
        // less than perfectly upright.  Set the corrective strength to zero to see what I'm
        // talking about.  The following lines help keep the camera upright.
        Vector3 properRight = Quaternion.Euler(0, 0, -transform.localEulerAngles.z) * transform.right;
        Vector3 uprightCorrection = Vector3.Cross(transform.right, properRight);
        GetComponent<Rigidbody>().AddRelativeTorque(uprightCorrection * correctiveStrength);
    }
}