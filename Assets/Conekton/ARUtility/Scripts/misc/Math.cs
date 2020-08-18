using System.Collections.Generic;
using UnityEngine;

namespace Conekton.ARUtility.Application
{
    public class Math
    {
        private const float VelocityThreshold = 55f;

        /// <summary>
        /// Calculate standard deviation and filter vectors with it.
        /// Refer: http://www.suguru.jp/nyuushi/hensachi.html
        /// </summary>
        public static Vector3[] FilterVelocity(Vector3[] data)
        {
            float avg = 0;
            
            foreach (var d in data)
            {
                avg += d.magnitude;
            }
            
            avg /= data.Length;

            float s = 0;
            
            foreach (var t in data)
            {
                float E = t.magnitude - avg;
                s += E * E;
            }
            s = Mathf.Sqrt(s / data.Length);

            List<Vector3> filterdData = new List<Vector3>();
            
            foreach (var t1 in data)
            {
                Vector3 d = t1;
                float value = d.magnitude;
                float t = Mathf.Abs(avg - value) * 10f / s;
                
                if (value > avg)
                {
                    t += 50f;
                }
                else
                {
                    t = 50f - t;
                }

                if (t >= VelocityThreshold)
                {
                    filterdData.Add(t1);
                }
            }

            return filterdData.ToArray();
        }

        /// <summary>
        /// Low-pass filter
        /// </summary>
        public static Vector3[] LowPassFilter(Vector3[] data)
        {
            if (data.Length <= 1)
            {
                return data;
            }

            // Const for the low-pass filter.
            const float a = 0.8f;
            const float ia = 1f - a;
            Vector3[] filterdData = new Vector3[data.Length];
            filterdData[0] = data[0];
            
            for (int i = 1; i < data.Length; i++)
            {
                Vector3 prev = filterdData[i - 1];
                Vector3 cur = data[i];
                float x = (a * prev.x) + (ia * cur.x);
                float y = (a * prev.y) + (ia * cur.y);
                float z = (a * prev.z) + (ia * cur.z);
                filterdData[i] = new Vector3(x, y, z);
            }

            return filterdData;
        }

        /// <summary>
        /// Calculate throw power.
        /// </summary>
        public static float[] LeastSquaresPlane(Vector3[] data)
        {
            float x = 0;
            float x2 = 0;
            float xy = 0;
            float xz = 0;

            float y = 0;
            float y2 = 0;
            float yz = 0;

            float z = 0;

            foreach (var v in data)
            {
                float vx = v.x;
                float vy = v.z;
                float vz = v.y;

                x += vx;
                x2 += (vx * vx);
                xy += (vx * vy);
                xz += (vx * vz);

                y += vy;
                y2 += (vy * vy);
                yz += (vy * vz);

                z += vz;
            }

            float l = 1 * data.Length;
            
            float[,] matA = new float[,]
            {
                {l,  x,  y},
                {x, x2, xy},
                {y, xy, y2},
            };

            float[] b = new float[]
            {
                z, xz, yz
            };

            return LUDecomposition(matA, b);
        }

        private static float[] LUDecomposition(float[,] aMatrix, float[] b)
        {
            int N = aMatrix.GetLength(0);

            float[,] lMatrix = new float[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    lMatrix[i, j] = 0;
                }
            }

            float[,] uMatrix = new float[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    uMatrix[i, j] = i == j ? 1f : 0;
                }
            }

            float[,] buffer = new float[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    buffer[i, j] = 0;
                }
            }

            for (int i = 0; i < N; i++)
            {
                int n = N - i - 1;

                float l0 = lMatrix[i, i] = aMatrix[0, 0];

                float[] l1 = new float[n];
                for (int j = 0; j < n; j++)
                {
                    lMatrix[j + i + 1, i] = l1[j] =  aMatrix[j + 1, 0];
                }

                float[] u1 = new float[n];
                for (int j = 0; j < n; j++)
                {
                    uMatrix[i, j + i + 1] = u1[j] = aMatrix[0, j + 1] / l0;
                }

                for (int j = 0; j < n; j++)
                {
                    for (int k = 0; k < n; k++)
                    {
                        buffer[j, k] = l1[j] * u1[k];
                    }
                }

                float[,] A1 = new float[n, n];
                for (int j = 0; j < n; j++)
                {
                    for (int k = 0; k < n; k++)
                    {
                        A1[j, k] = aMatrix[j + 1, k + 1] - buffer[j, k];
                    }
                }

                aMatrix = A1;
            }

            float[] y = new float[N];
            for (int i = 0; i < N; i++)
            {
                float sum = 0;
                for (int k = 0; k <= i - 1; k++)
                {
                    sum += lMatrix[i, k] * y[k];
                }
                y[i] = (b[i] - sum) / lMatrix[i, i];
            }

            float[] x = new float[N];
            for (int i = N - 1; i >= 0; i--)
            {
                float sum = 0;
                for (int j = i + 1; j <= N - 1; j++)
                {
                    sum += uMatrix[i, j] * x[j];
                }
                x[i] = y[i] - sum;
            }

            return x;
        }
    }
}
