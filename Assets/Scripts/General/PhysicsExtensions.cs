using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class PhysicsExtensions {
        public const float ConeCastDefaultDistance = 1000.0f;

        public static RaycastHit[] ConeCastAll
            (
                Vector3 worldPosition,
                float angle,
                Vector3 direction,
                float distance = ConeCastDefaultDistance,
                int layerMask = Physics.DefaultRaycastLayers,
                QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal
            )
        {

            float alpha = Mathf.Deg2Rad * angle;
            float radius = distance * Mathf.Tan(alpha);

            List<RaycastHit> outHits = new List<RaycastHit>();

            RaycastHit[] hits = Physics.SphereCastAll(
                worldPosition,
                radius,
                direction,
                distance,
                layerMask,
                queryTriggerInteraction
            );

            foreach(var hit in hits)
            {
                float relativeAngle = Vector3.Angle(
                    direction,
                    hit.collider.transform.position - worldPosition
                );

                if (relativeAngle <= angle) outHits.Add(hit);
            }

            return outHits.ToArray();
        }

        public static bool ConeCastAngle
            (
                Vector3 worldPosition,
                float angle,
                Vector3 direction,
                out RaycastHit hitInfo,
                float distance = ConeCastDefaultDistance,
                int layerMask = Physics.DefaultRaycastLayers,
                QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal
            )
        {
            float alpha = Mathf.Deg2Rad * angle;
            float radius = distance * Mathf.Tan(alpha);

            List<RaycastHit> outHits = new List<RaycastHit>();

            RaycastHit[] hits = Physics.SphereCastAll(
                worldPosition,
                radius,
                direction,
                distance,
                layerMask,
                queryTriggerInteraction
            );

            RaycastHit closestHit = new RaycastHit(); 
            float smallestAngle = angle;

            foreach(var hit in hits)
            {
                float relativeAngle = Vector3.Angle(
                    direction,
                    (hit.collider.transform.position - worldPosition).normalized
                );

                if (relativeAngle <= smallestAngle)
                {
                    smallestAngle = relativeAngle;
                    closestHit = hit;
                }
            }

            hitInfo = closestHit;
            return (closestHit.collider != null);
        }
    }
}
