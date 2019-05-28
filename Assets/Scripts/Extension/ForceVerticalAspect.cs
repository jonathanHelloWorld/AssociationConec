using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ForceVerticalAspect : MonoBehaviour {

    public float aspectY = 3.4f;
    public float aspectYOrtho = 0.5f;
    private bool showConfig = false;

    public Camera[] cameras;

    void OnGUI() 
    {
        if (showConfig) 
        {
            aspectY = GUI.HorizontalSlider(new Rect(10, Screen.height - 50, 500, 50), aspectY, 0, 6);
            aspectYOrtho = GUI.HorizontalSlider(new Rect(10, Screen.height - 120, 500, 50), aspectYOrtho, 0, 6);
            GUI.Label(new Rect(520, Screen.height - 50, 500, 50), "ATUAL: " + aspectY.ToString("F3"));
            GUI.Label(new Rect(520, Screen.height - 120, 500, 50), "ATUAL ORTHO: " + aspectYOrtho.ToString("F3"));
        }
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.F1))
            showConfig = !showConfig;

        for (int x = 0; x < cameras.Length; x++)
        {
            Matrix4x4 temp = cameras[x].projectionMatrix;

            if (!cameras[x].orthographic)
                temp[1, 1] = aspectY;
            else
                temp[1, 1] = aspectYOrtho;

            cameras[x].projectionMatrix = temp;
        }
    }
}
