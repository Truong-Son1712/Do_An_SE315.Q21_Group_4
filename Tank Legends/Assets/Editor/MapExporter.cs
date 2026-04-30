using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class MapExporter
{
    [MenuItem("Tools/Export Game World (Tagged Roots)")]
    public static void ExportWorld()
    {
        GameObject[] roots = GameObject.FindObjectsOfType<GameObject>();

        List<ColliderData> colliders = new List<ColliderData>();
        List<SpawnData> spawns = new List<SpawnData>();

        foreach (GameObject obj in roots)
        {
            // CHỈ xử lý root objects (không đụng child trực tiếp ở đây)
            if (obj.transform.parent != null)
                continue;

            // MAP ROOT
            if (obj.CompareTag("Map"))
            {
                ProcessColliders(obj, colliders);
            }

            // SPAWN ROOT
            else if (obj.CompareTag("SpawnPoint"))
            {
                ProcessSpawns(obj, spawns);
            }

            // có thể thêm Trigger, Item, etc sau này
        }

        WorldData world = new WorldData
        {
            colliders = colliders,
            spawns = spawns
        };

        string json = JsonUtility.ToJson(world, true);

        File.WriteAllText(Application.dataPath + "/world.json", json);

        Debug.Log("World export completed!");
    }

    // =========================
    // COLLIDER EXPORT
    // =========================
    static void ProcessColliders(GameObject root, List<ColliderData> list)
    {
        Collider[] cols = root.GetComponentsInChildren<Collider>();

        foreach (var col in cols)
        {
            ColliderData data = new ColliderData();

            data.name = col.gameObject.name;

            // WORLD SPACE (QUAN TRỌNG)
            data.position = col.transform.position;
            data.rotation = col.transform.eulerAngles;

            if (col is BoxCollider box)
            {
                data.type = "box";
                data.center = col.transform.TransformPoint(box.center);
                data.size = Vector3.Scale(box.size, col.transform.lossyScale);
            }
            else if (col is SphereCollider sphere)
            {
                data.type = "sphere";
                data.center = col.transform.TransformPoint(sphere.center);

                float scale = Mathf.Max(
                    col.transform.lossyScale.x,
                    col.transform.lossyScale.y,
                    col.transform.lossyScale.z
                );

                data.radius = sphere.radius * scale;
            }
            else if (col is CapsuleCollider capsule)
            {
                data.type = "capsule";
                data.center = col.transform.TransformPoint(capsule.center);

                float scaleX = col.transform.lossyScale.x;
                float scaleY = col.transform.lossyScale.y;
                float scaleZ = col.transform.lossyScale.z;

                data.radius = capsule.radius * Mathf.Max(scaleX, scaleZ);
                data.height = capsule.height * scaleY;
                data.direction = capsule.direction;
            }
            else
            {
                continue; // ignore MeshCollider etc
            }

            list.Add(data);
        }
    }

    // =========================
    // SPAWN EXPORT
    // =========================
    static void ProcessSpawns(GameObject root, List<SpawnData> list)
    {
        SpawnMarker[] sp = root.GetComponentsInChildren<SpawnMarker>();

        foreach (var s in sp)
        {
            SpawnData data = new SpawnData();
            data.id = s.id;
            data.position = s.transform.position;

            list.Add(data);
        }
    }

    // =========================
    // DATA STRUCTS
    // =========================
    [System.Serializable]
    public class WorldData
    {
        public List<ColliderData> colliders;
        public List<SpawnData> spawns;
    }

    [System.Serializable]
    public class ColliderData
    {
        public string name;
        public string type;

        public Vector3 position;
        public Vector3 rotation;

        public Vector3 center;

        public Vector3 size;

        public float radius;
        public float height;
        public int direction;
    }

    [System.Serializable]
    public class SpawnData
    {
        public string id;
        public Vector3 position;
    }
}
