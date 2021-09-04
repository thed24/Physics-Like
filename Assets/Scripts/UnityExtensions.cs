using UnityEngine;

static class UnityExtensions
{
    public static GameObject LoadPrefabFrom(string path)
    {
        Object prefab = Resources.Load(path);
        return (GameObject) Object.Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
    }
}

