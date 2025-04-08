using UnityEngine;
using UnityEngine.AI;

public class pointgene : MonoBehaviour
{
    public static Vector3 pointgen(Vector3 Start_point, float Radius)
    {
        Vector3 Dir = Random.insideUnitSphere * Radius;
        Dir += Start_point;
        NavMeshHit Hit_;
        Vector3 Final_Pos = Vector3.zero;
        if (NavMesh.SamplePosition(Dir, out Hit_, Radius, 1))
        {
            Final_Pos = Hit_.position;
        }
        return Final_Pos;
    }
}
