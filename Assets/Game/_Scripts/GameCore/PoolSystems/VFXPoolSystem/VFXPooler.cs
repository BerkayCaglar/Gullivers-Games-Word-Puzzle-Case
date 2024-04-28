using System.Collections;
using System.Collections.Generic;
using GameCore.SingletonSystem;
using UnityEngine;

namespace GameCore.PopupSystem.VFXPoolSystem
{
    public class VFXPooler : AutoSingleton<VFXPooler>
    {
        [BHeader("VFX Parent", true)]
        [SerializeField] private Transform poolerParent;

        [BHeader("VFX Prefabs", true)]
        [SerializeField] private GameObject[] VFXPrefabs;
        [SerializeField] private int[] VFXAmounts;

        private Dictionary<string, Queue<GameObject>> _poolDictionary;

        private void Start()
        {
            _poolDictionary = new Dictionary<string, Queue<GameObject>>();
            for (int i = 0; i < VFXPrefabs.Length; i++)
            {
                Queue<GameObject> objectPool = new();
                for (int j = 0; j < VFXAmounts[i]; j++)
                {
                    GameObject obj = Instantiate(VFXPrefabs[i], poolerParent);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                _poolDictionary.Add(VFXPrefabs[i].name, objectPool);
            }
        }

        public GameObject SpawnFromPool(VFXType type, Vector3 position, Quaternion rotation, float despawnTime)
        {
            var vfxName = type.ToString();

            if (!_poolDictionary.ContainsKey(vfxName))
            {
                Debug.LogWarning("Pool with tag " + name + " doesn't exist.");
                return null;
            }

            GameObject objectToSpawn = _poolDictionary[vfxName].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.SetPositionAndRotation(position, rotation);

            StartCoroutine(DespawnAfterTime(vfxName, objectToSpawn, despawnTime));

            return objectToSpawn;
        }

        public GameObject SpawnFromPool(VFXType type, Vector3 position, float despawnTime)
        {
            var vfxName = type.ToString();
            if (!_poolDictionary.ContainsKey(vfxName))
            {
                Debug.LogWarning("Pool with tag " + vfxName + " doesn't exist.");
                return null;
            }

            GameObject objectToSpawn = _poolDictionary[vfxName].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.SetPositionAndRotation(position, objectToSpawn.transform.rotation);

            StartCoroutine(DespawnAfterTime(vfxName, objectToSpawn, despawnTime));

            return objectToSpawn;
        }

        private IEnumerator DespawnAfterTime(string name, GameObject objectToDespawn, float despawnTime)
        {
            yield return new WaitForSeconds(despawnTime);
            _poolDictionary[name].Enqueue(objectToDespawn);
            objectToDespawn.SetActive(false);
        }
    }
}
