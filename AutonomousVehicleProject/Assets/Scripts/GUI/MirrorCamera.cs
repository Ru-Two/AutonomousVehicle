using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MirrorFlipCamera(this.gameObject.GetComponent<Camera>());
    }

    void MirrorFlipCamera(Camera camera) 
    {
        Matrix4x4 mat = camera.projectionMatrix;

        mat *= Matrix4x4.Scale(new Vector3(-1, 1, 1));

        camera.projectionMatrix = mat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
