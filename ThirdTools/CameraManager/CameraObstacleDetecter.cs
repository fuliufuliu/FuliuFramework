using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class CameraObstacleDetecter : MonoBehaviour
{
    private Transform _target;
    private BoxCollider triggerCollider;
    private Transform eyeCameraTrans;
//    public LayerMask layerMask;
    public string obstacleTag = "Obstacle";
    public Material transparentMaterial;
    Dictionary<MeshRenderer, Material> materialCache = new Dictionary<MeshRenderer, Material>();

    Transform target
    {
        get
        {
            if (_target)
            {
                return _target;
            }
            _target = GetComponentInParent<FreeLookCam>()?.Target;
            return _target;
        }
        set { _target = value; }
    }

    private void Awake()
    {
        triggerCollider = GetComponent<BoxCollider>();
        GetComponent<Rigidbody>().isKinematic = true;
        triggerCollider.isTrigger = true;
        eyeCameraTrans = GetComponentInParent<Camera>().transform;
    }

    private void LateUpdate()
    {
        if (eyeCameraTrans && triggerCollider && target)
        {
            triggerCollider.transform.position = (target.position + eyeCameraTrans.position)/2;
            triggerCollider.size = new Vector3(triggerCollider.size.x, triggerCollider.size.y, Vector3.Distance(target.position, eyeCameraTrans.position));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (other.gameObject.tag.Equals(obstacleTag, StringComparison.OrdinalIgnoreCase))
        {
            ChangeMaterials(other);
        }
    }

    private void ChangeMaterials(Collider other)
    {
        var render = other.GetComponent<MeshRenderer>();
        if (render != null)
        {
            materialCache[render] = render.sharedMaterial;
            render.material = transparentMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;
        if (other.gameObject.tag.Equals(obstacleTag, StringComparison.OrdinalIgnoreCase))
        {
            ChangeBackMaterials(other);
        }
    }

    private void ChangeBackMaterials(Collider other)
    {
        var render = other.GetComponent<MeshRenderer>();
        if (render != null)
        {
            if (materialCache.ContainsKey(render)) render.sharedMaterial = materialCache[render];
        }
    }
}
