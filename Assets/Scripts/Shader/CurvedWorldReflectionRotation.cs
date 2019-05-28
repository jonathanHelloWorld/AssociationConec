using UnityEngine;
using System.Collections;

public class rotateTeste : MonoBehaviour {

    public Material rotateMat;
    public float angle, speed;

    void Update()
    {
        //for fixed rotation uncomment the line below:
        //Quaternion rot = Quaternion.Euler (0, angle, 0);

        //for animated cubemap::
        Quaternion rot = Quaternion.Euler(0, (Time.time * speed), 0);

        Matrix4x4 m = new Matrix4x4();
        m.SetTRS(Vector3.zero, rot, new Vector3(1, 1, 1));
        rotateMat.SetMatrix("_Rotation", m);
    }
}
