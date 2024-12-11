using FluffyUnderware.Curvy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gs.chef.game.Scripts.Objects
{
    public class PathPrefab : MonoBehaviour
    {
        public CurvySpline spline;
        public GameObject pathRef;

        [Button]
        public void CreatePath()
        {
            int childCount = pathRef.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var child = pathRef.transform.GetChild(i);
                GameObject go = new GameObject();
                go.transform.SetParent(spline.transform);
                go.transform.position = child.position;
                go.AddComponent<CurvySplineSegment>();
            }
        }
        
    }
}