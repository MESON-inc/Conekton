using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARMultiplayer.PersistentCoordinate.Domain;

namespace Conekton.ARMultiplayer.PersistentCoordinate.Mock
{
    public class DummyPCACreator : MonoBehaviour
    {
        [Inject] private IPersistentCoordinateService _service = null;

        private int _index = 0;

        private void OnDrawGizmos()
        {
            if (!UnityEngine.Application.isPlaying)
            {
                return;
            }

            var pcas = _service.GetAllPCA();

            if (pcas.Count == 0)
            {
                return;
            }

            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;
            GetAverage(pcas, out pos, out rot);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pos, 0.05f);

            Gizmos.color = Color.green;
            Vector3 n = rot * Vector3.up;
            Gizmos.DrawLine(pos, pos + n * 0.1f);
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 150, 30), "Add PCA"))
            {
                AddPCA();
            }
        }

        private void GetAverage(IReadOnlyCollection<IPCA> pcas, out Vector3 avePos, out Quaternion aveRot)
        {
            Vector3 pos = Vector3.zero;
            Quaternion firstRot = Quaternion.identity;

            bool foundFirstRot = false;

            float x = 0, y = 0, z = 0, w = 0;

            foreach (var p in pcas)
            {
                pos += p.Position;

                if (!foundFirstRot)
                {
                    firstRot = p.Rotation;
                    foundFirstRot = true;
                }

                float dot = Quaternion.Dot(firstRot, p.Rotation);
                float multi = dot > 0f ? 1f : -1f;

                x += p.Rotation.x * multi;
                y += p.Rotation.y * multi;
                z += p.Rotation.z * multi;
                w += p.Rotation.w * multi;
            }

            float k = 1f / Mathf.Sqrt(x * x + y * y + z * z + w * w);

            avePos = pos / pcas.Count;
            aveRot = new Quaternion(x * k, y * k, z * k, w * k);
        }

        private void AddPCA()
        {
            IPCA pca = CreatePCA();
            _service.Register(pca);
        }

        private IPCA CreatePCA()
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            go.transform.position = Random.insideUnitSphere * 3f;
            go.transform.rotation = Random.rotation;
            go.transform.localScale = Vector3.one * 0.1f;
            DummyPCA pca = go.AddComponent<DummyPCA>();
            pca.SetUniqueID((_index++).GetHashCode().ToString());
            return pca;
        }
    }
}

