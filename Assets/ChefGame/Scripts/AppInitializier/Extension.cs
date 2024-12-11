using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Giroo.Core.Extension
{
    public static class Extension
    {
        /// <summary>
        /// string to int
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ToInt(this string input)
        {
            return int.Parse(input);
        }

        /// <summary>
        /// Vector3 to Vector2
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Vector2 ToVector2(this Vector3 input)
        {
            return input;
        }

        /// <summary>
        /// Vector2 to Vector3 and Vector3.z = z
        /// </summary>
        /// <param name="input"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Vector3 ToVector3(this Vector2 input, int z)
        {
            return (Vector3)input + new Vector3(0, 0, z);
        }

        /// <summary>
        /// Vector2 to Vector3
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Vector3 ToVector3(this Vector2 input)
        {
            return input;
        }

        /// <summary>
        /// Vector3.x = x;
        /// </summary>
        /// <param name="input"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        /// 
        public static Vector3 ChangeX(this Vector3 input, float x)
        {
            return new Vector3(x, input.y, input.z);
        }

        /// <summary>
        /// Vector3.y = y;
        /// </summary>
        /// <param name="input"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector3 ChangeY(this Vector3 input, float y)
        {
            return new Vector3(input.x, y, input.z);
        }

        /// <summary>
        /// Vector3.z =z;
        /// </summary>
        /// <param name="input"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Vector3 ChangeZ(this Vector3 input, float z)
        {
            return new Vector3(input.x, input.y, z);
        }

        public static Color ChangeA(this Color input, float alpha)
        {
            return new Color(input.r, input.g, input.b, alpha);
        }

        /// <summary>
        /// Obje in camera wiev 
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        }

        /// <summary>
        /// Rotate point around center
        /// </summary>
        /// <param name="point"></param>
        /// <param name="center"></param>
        /// <param name="angles"></param>
        /// <returns></returns>
        public static Vector3 RotateDistanceCenter(this Vector3 center, Vector3 distance, Vector3 angles)
        {
            return Quaternion.Euler(angles) * (distance) + center;
        }

        /// <summary>
        /// gameobject collider in platform area 
        /// </summary>
        /// <param name="obje"></param>
        /// <param name="direction"></param>
        /// <param name="navMesh"></param>
        /// <returns></returns>
        public static bool InArea(this GameObject obje, Vector3 direction = new Vector3(), bool navMesh = false)
        {
            if (navMesh)
            {
                if (NavMesh.FindClosestEdge(obje.transform.position + direction, out var hit, NavMesh.AllAreas))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (Physics.Raycast(obje.transform.position + direction, new Vector3(0, -1, 0), out var hit, 10))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ball")) return true;
                    if (hit.transform.CompareTag("Plane"))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

#if UNITY_EDITOR
        public static void SaveMesh(this Mesh mesh, int count, string name = "wall")
        {
            MeshUtility.Optimize(mesh);
            AssetDatabase.AddObjectToAsset(mesh, "Assets/Meshs/" + name + count + ".asset");
        }
#endif


        public static float PointInEllipse(this Vector3 origin, Vector3 point, float a, float b)
        {
            float angle = Mathf.Atan(origin.z / origin.x) * Mathf.Rad2Deg;
            if (float.IsNaN(angle)) angle = 0;
            float cosa = Mathf.Cos(angle);
            float sina = Mathf.Sin(angle);

            float aa = a / 2 * a / 2;
            float bb = b / 2 * b / 2;
            float x = Mathf.Pow(cosa * (point.x - origin.x) + sina * (point.z - origin.z), 2);
            float z = Mathf.Pow(sina * (point.x - origin.x) - cosa * (point.z - origin.z), 2);
            float ellipse = x / aa + z / bb;

            return ellipse;
        }


        /// <summary>
        /// Remaps given value from range to new range
        /// </summary>
        /// <param name="value"></param>
        /// <param name="from1"></param>
        /// <param name="to1"></param>
        /// <param name="from2"></param>
        /// <param name="to2"></param>
        /// <returns></returns>
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static List<Vector3> PointInEllipse(float a, float b, int angle)
        {
            List<Vector3> points = new List<Vector3>();

            for (int i = 0; i < 360; i += angle)
            {
                points.Add(new Vector3(a * Mathf.Cos(i * Mathf.Deg2Rad), 0, b * Mathf.Sin(i * Mathf.Deg2Rad)));
            }

            return points;
        }

        public static T RandomElement<T>(this IEnumerable<T> list)
        {
            return list.ElementAt(Random.Range(0, list.Count()));
        }

        /// <summary>
        /// Checks if mouse/touch is over an UI element
        /// </summary>
        /// <returns></returns>
        public static bool IsPointerOverUIElement()
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }

        #region PoitnerOverElementFunctions

        public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
        {
            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = eventSystemRaysastResults[index];
                if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                    return true;
            }

            return false;
        }

        static List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }

        #endregion

        public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max)
        {
            value.x = Mathf.Clamp(value.x, min.x, max.x);
            value.y = Mathf.Clamp(value.y, min.y, max.y);
            value.z = Mathf.Clamp(value.z, min.z, max.z);
            return value;
        }

        public static Vector3 Clamp(this Vector3 value, float min, float max)
        {
            value.x = Mathf.Clamp(value.x, min, max);
            value.y = Mathf.Clamp(value.y, min, max);
            value.z = Mathf.Clamp(value.z, min, max);
            return value;
        }

        public static Vector3 NegativeAngle(ref this Vector3 rotate)
        {
            float angle = rotate.x;
            rotate.x = (angle > 180) ? angle - 360 : angle;
            angle = rotate.y;
            rotate.y = (angle > 180) ? angle - 360 : angle;
            angle = rotate.z;
            rotate.z = (angle > 180) ? angle - 360 : angle;
            return rotate;
        }

        /// <summary>
        /// Lerps float to the target value
        /// </summary>
        /// <param name="target"> Target of the lerping function </param>
        /// <param name="distToEqual"> The distance in which the value will be equal to target </param>
        /// <param name="lerpVal"> The t value for the Lerp function </param>
        public static void LerpTo(ref this float value, float target, float distToEqual, float lerpVal)
        {
            if (Mathf.Abs(value - target) > distToEqual)
            {
                value = Mathf.Lerp(value, target, lerpVal);
            }
            else
            {
                value = target;
            }
        }

        public static bool LerpTo(ref this Vector3 value, Vector3 target, float distToSnap, float lerpVal)
        {
            if (Vector3.Distance(value, target) > distToSnap)
            {
                value = Vector3.Lerp(value, target, lerpVal);
                return false;
            }

            value = target;
            return true;
        }

        public static bool LerpTo(ref this Quaternion value, Quaternion target, float angleToSnap, float lerpVal)
        {
            if (Quaternion.Angle(value, target) > angleToSnap)
            {
                value = Quaternion.Lerp(value, target, lerpVal);
                return false;
            }

            value = target;
            return true;
        }

        /// <summary>
        /// Moves float to the target value
        /// </summary>
        /// <param name="target"> Target of the MoveTowards function </param>
        /// <param name="distToEqual"> The distance in which the value will be equal to target </param>
        /// <param name="delta"> amx movement </param>
        public static void MoveTo(ref this float value, float target, float distToEqual, float delta)
        {
            if (Mathf.Abs(value - target) > distToEqual)
            {
                value = Mathf.MoveTowards(value, target, delta);
            }
            else
            {
                value = target;
            }
        }

        public static bool RotateTo(ref this Quaternion value, Quaternion target, float angleToSnap, float slerpVal)
        {
            if (Quaternion.Angle(value, target) > angleToSnap)
            {
                value = Quaternion.RotateTowards(value, target, slerpVal);
                return false;
            }

            value = target;
            return true;
        }

        /// <summary>
        /// Check if layerMask (this) contains a given layer 
        /// </summary>
        /// <param name="layer"> The layer to be checked </param>
        /// <returns> True if is contained false if not </returns>
        public static bool Contains(this LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }

        public static RaycastHit GetHitFromMouse(this Camera camera, LayerMask layer)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layer))
            {
                return hit;
            }

            return default;
        }

        public static void DrawLineTo(this Transform transform, Vector3 dir)
        {
            Debug.DrawLine(transform.position, transform.position + dir);
        }

        public static void DrawLineTo(this Transform transform, Vector3 dir, Color color)
        {
            Debug.DrawLine(transform.position, transform.position + dir, color);
        }

        /// <summary>
        /// Gives a point on quadratic bezier curve.(De Casteljau's Algorithm)
        /// </summary>
        /// <param name="p0"> First Point</param>
        /// <param name="m0"> Bezier Control Point</param>
        /// <param name="p1"> Second Point </param>
        /// <param name="normalizedTime"> Time between 0 to 1. 0 is first point and 1 is second point </param>
        /// <returns></returns>
        public static Vector3 GetPointOnBezier(Vector3 p0, Vector3 m0, Vector3 p1, float normalizedTime)
        {
            Vector3 b1 = Vector3.Lerp(p0, m0, normalizedTime);
            Vector3 b2 = Vector3.Lerp(m0, p1, normalizedTime);
            Vector3 b3 = Vector3.Lerp(b1, b2, normalizedTime);
            return b3;
        }

        /// <summary>
        /// Gives a ray hit point on plane at given y
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector3 RayToPointAtY(this Ray ray, float y)
        {
            return ray.origin + (((ray.origin.y - y) / -ray.direction.y) * ray.direction);
        }

        /// <summary>
        /// Gives a ray hit point on plane at given z
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Vector3 RayToPointAtZ(this Ray ray, float z)
        {
            return ray.origin + (((ray.origin.z - z) / -ray.direction.z) * ray.direction);
        }

        /// <summary>
        /// Gives a ray hit point on plane at given x
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Vector3 RayToPointAtX(this Ray ray, float x)
        {
            return ray.origin + (((ray.origin.x - x) / -ray.direction.x) * ray.direction);
        }

        public static bool NearlyEquals(this float thisValue, float otherValue, float epsilon = 1e-3f)
        {
            return Mathf.Abs(thisValue - otherValue) <= epsilon;
        }

        public static List<Vector3> SmoothCorners(List<Vector3> inputPoints, float smoothingDistance)
        {
            List<Vector3> smoothedPoints = new List<Vector3>();

            for (int i = 0; i < inputPoints.Count; i++)
            {
                Vector3 currentPoint = inputPoints[i];
                Vector3 average = currentPoint;
                int count = 1;

                for (int j = i + 1; j < inputPoints.Count; j++)
                {
                    Vector3 nextPoint = inputPoints[j];
                    float distance = Vector3.Distance(currentPoint, nextPoint);

                    if (distance <= smoothingDistance)
                    {
                        average += nextPoint;
                        count++;
                    }
                    else
                    {
                        break; // Stop when the distance exceeds the smoothing threshold
                    }
                }

                average /= count;
                smoothedPoints.Add(average);

                // Skip the points that were averaged
                i += count - 1;
            }

            return smoothedPoints;
        }

        public static List<Vector3> BevelCorners(List<Vector3> inputPoints, float bevelRadius, float bevelAmount)
        {
            List<Vector3> beveledPoints = new List<Vector3>();

            for (int i = 0; i < inputPoints.Count; i++)
            {
                Vector3 currentPoint = inputPoints[i];

                if (i == 0 || i == inputPoints.Count - 1)
                {
                    // The first and last points remain unchanged
                    beveledPoints.Add(currentPoint);
                    continue;
                }

                Vector3 previousPoint = inputPoints[i - 1];
                Vector3 nextPoint = inputPoints[i + 1];


                Vector3 directionToPrevious = (currentPoint - previousPoint).normalized * bevelRadius;
                Vector3 directionToNext = -(currentPoint - nextPoint).normalized * bevelRadius;

                Vector3 p0 = currentPoint - directionToPrevious;
                Vector3 m0 = currentPoint;
                Vector3 p1 = currentPoint + directionToNext;

                for (int j = 0; j < bevelAmount; j++)
                {
                    float currentTime = j / (bevelAmount - 1);

                    Vector3 point = Extension.GetPointOnBezier(p0, m0, p1, currentTime);
                    beveledPoints.Add(point);
                }
            }

            return beveledPoints;
        }

        public static Vector3 BevelCorner(Vector3 currentPoint, Vector3 previousPoint, Vector3 nextPoint, float bevelRadius)
        {
            Vector3 toPrevious = previousPoint - currentPoint;
            Vector3 toNext = nextPoint - currentPoint;

            toPrevious.Normalize();
            toNext.Normalize();

            Vector3 beveledPoint = currentPoint + toPrevious * bevelRadius + toNext * bevelRadius;

            return beveledPoint;
        }
    }
}