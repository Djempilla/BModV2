using UnityEngine;

namespace BModv2.Client.Render;

public class LineExample : MonoBehaviour
{
    
    private LineRenderer line;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Awake()
    {
        line = gameObject.AddComponent<LineRenderer>();
        line.positionCount = 2;
        line.useWorldSpace = true;
        line.startWidth = 0.2f;
        line.endWidth = 0.2f;
        
        
    }

    private void Update()
    {
        if(!_camera) return;
        
        Vector3 p1 = _camera.transform.position + _camera.transform.forward * 2f;
        Vector3 p2 = p1 + _camera.transform.right * 1f;
        
        line.SetPosition(0, p1);
        line.SetPosition(1, p2);
        
        
    }
}