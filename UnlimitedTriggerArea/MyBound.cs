using UnityEngine;

namespace fuliu
{
    public class MyBound : MonoBehaviour
    {
        public Vector3 size;

        public void ResetData(Collider collider)
        {
           var tCStatus = collider.enabled;
           collider.enabled = true;
           var bounds = collider.bounds;
           collider.enabled = tCStatus;
           size = bounds.size;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, size);
        }
    }
}