﻿namespace IMRE.HandWaver.ScaleStudy
{
    public class HyperTorusCrossSection : UnityEngine.MonoBehaviour, ISliderInput
    {
        public enum crossSectionPlane
        {
            w,
            x,
            y,
            z
        }

        public float P;

        public crossSectionPlane plane = crossSectionPlane.z;
        public float R;

        public float slider
        {
            set => renderToricSection(-1 + (2 * value));
        }

        // Start is called before the first frame update
        private void Start()
        {
            gameObject.AddComponent<UnityEngine.MeshRenderer>();
            gameObject.AddComponent<UnityEngine.MeshFilter>();
            GetComponent<UnityEngine.MeshRenderer>().material = sphereMaterial;
        }

        private Unity.Mathematics.float3 HyperToricSection(float alpha, float beta, float h)
        {
            float a;
            float b;
            float c;

            //since we are fixing one of the x,y,z,w values, we can find a value for a, b, or c and use alpha and beta to parameterize the other two.
            switch (plane)
            {
                //TODO check multiple solutions for acos and asin
                case crossSectionPlane.x:
                    a = alpha;
                    b = beta;
                    c = Unity.Mathematics.math.asin(
                        h / (R + ((P + Unity.Mathematics.math.cos(a)) * Unity.Mathematics.math.cos(b))));
                    if (solutionA) c = UnityEngine.Mathf.PI - c;

                    break;
                case crossSectionPlane.y:
                    a = alpha;
                    b = beta;
                    c = Unity.Mathematics.math.asin(h / (P + Unity.Mathematics.math.cos(a)));
                    if (solutionA) c = UnityEngine.Mathf.PI - c;

                    break;
                case crossSectionPlane.z:
                    b = alpha;
                    c = beta;
                    a = Unity.Mathematics.math.asin(h);
                    if (solutionA) a = UnityEngine.Mathf.PI - a;

                    break;
                case crossSectionPlane.w:
                    a = alpha;
                    b = beta;
                    c = Unity.Mathematics.math.asin(
                        h / (R + ((P + Unity.Mathematics.math.cos(a)) * Unity.Mathematics.math.cos(b))));
                    if (solutionA) c = UnityEngine.Mathf.PI - c;

                    break;
                default:
                    b = alpha;
                    c = beta;
                    a = Unity.Mathematics.math.asin(h);
                    if (solutionA) a = UnityEngine.Mathf.PI - a;

                    break;
            }

            float w = (R + ((P + Unity.Mathematics.math.cos(a)) * Unity.Mathematics.math.cos(b))) *
                      Unity.Mathematics.math.cos(c);
            float x = (R + ((P + Unity.Mathematics.math.cos(a)) * Unity.Mathematics.math.cos(b))) *
                      Unity.Mathematics.math.sin(c);
            float y = (P + Unity.Mathematics.math.cos(a)) * Unity.Mathematics.math.sin(b);
            float z = Unity.Mathematics.math.sin(a);

            switch (plane)
            {
                case crossSectionPlane.x:
                    return new Unity.Mathematics.float3(w, y, z);
                case crossSectionPlane.y:
                    return new Unity.Mathematics.float3(w, x, z);
                case crossSectionPlane.z:
                    return new Unity.Mathematics.float3(x, y, w);
                case crossSectionPlane.w:
                    return new Unity.Mathematics.float3(x, y, z);
                default:
                    return new Unity.Mathematics.float3(0, 0, 0);
            }
        }

        private void renderToricSection(float height)
        {
            UnityEngine.Vector3[] verts = new UnityEngine.Vector3[((n + 1) * (n - 1)) + 1];

            //Array of 2D vectors for UV map of vertices
            UnityEngine.Vector2[] uvs = new UnityEngine.Vector2[verts.Length];
            float oneNth = 1f / n;

            //loop through n-1 times, since edges wrap around
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    //index value computation
                    int idx = i + (n * j);

                    //find radian value of length of curve
                    float alpha = i * oneNth * 2 * UnityEngine.Mathf.PI;
                    float beta = j * oneNth * 2 * UnityEngine.Mathf.PI;

                    //map vertices from 2 dimensions to 3
                    //TODO testing with a = arcsin(z), cross secting in z
                    verts[idx] = HyperToricSection(alpha, beta, height);

                    //uv mapping 
                    uvs[idx] = new UnityEngine.Vector2(j * oneNth, i * oneNth);
                }
            }

            //integer array for number of triangle vertices-3 verts x 2 triangles per square x n^2
            int[] triangles = new int[6 * n * n];

            //for each triangle
            for (int k = 0; k < (2 * n * n); k++)
            {
                //for each vertex on each triangle
                for (int m = 0; m < 3; m++)
                    triangles[(3 * k) + m] = triangle(k, m);
            }

            crossSectionRenderer.vertices = verts;
            crossSectionRenderer.triangles = triangles;
            crossSectionRenderer.uv = uvs;
            crossSectionRenderer.RecalculateNormals();
            crossSectionRenderer.Optimize();
        }

        /// <summary>
        ///     Finds the vertex indices for the kth triangle
        ///     returns the mth vertex index of the kth triangle
        ///     Triangles indexed for clockwise front face
        /// </summary>
        /// <param name="k"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private int triangle(int k, int m)
        {
            if (k < (n * n))
            {
                //lower half triangle
                switch (m)
                {
                    case 0:
                        return k;
                    case 2:
                        return (UnityEngine.Mathf.FloorToInt(k / n) * n) + ((k + 1) % n);
                    case 1:
                        return (((UnityEngine.Mathf.FloorToInt(k / n) + 1) % n) * n) + ((k + 1) % n);
                }
            }
            else
            {
                k = k - (n * n);

                switch (m)
                {
                    case 0:
                        return k;
                    case 2:
                        return (((UnityEngine.Mathf.FloorToInt(k / n) + 1) % n) * n) + ((k + 1) % n);
                    case 1:
                        return (((UnityEngine.Mathf.FloorToInt(k / n) + 1) % n) * n) + (k % n);
                }
            }

            UnityEngine.Debug.LogError("Invalid parameter.");

            return 0;
        }

        #region variables/components

        public int n;
        private UnityEngine.Mesh crossSectionRenderer => GetComponent<UnityEngine.MeshFilter>().mesh;
        public UnityEngine.Material sphereMaterial;

        public bool solutionA;

        #endregion
    }
}