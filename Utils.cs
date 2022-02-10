using UnityEngine;
using System.Collections;

namespace AF
{

    public static class Utils
    {

        public static void FaceTarget(Transform origin, Transform target)
        {
            var lookPos = target.position - origin.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            origin.rotation = Quaternion.Slerp(origin.rotation, rotation, Time.deltaTime * 100f);
        }

        public static bool PlayerIsSighted(Transform enemy, Player player, LayerMask obstructionMask)
        {
            float fovAngle = 100f;
            float sightDistance = 20f;

            if (Vector3.Distance(enemy.transform.position, player.headTransform.position) > sightDistance)
            {
                return false;
            }

            Vector3 directionToTarget = (player.headTransform.position - enemy.transform.position).normalized;

            if (Vector3.Angle(enemy.transform.forward, directionToTarget) < fovAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(enemy.transform.position, player.transform.position);

                if (!Physics.Raycast(enemy.transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    return true;
                }
            }

            return false;
        }

    }

}
