using System.Linq;
using UnityEngine;

static class UnityExtensions
{
    public static GameObject LoadPrefabFrom(string path)
    {
        Object prefab = Resources.Load(path);
        return (GameObject) Object.Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public static void CombineChildMeshesOf(GameObject parentObject, Material sharedMaterial)
    {
        var position = parentObject.transform.position;
        parentObject.transform.position = Vector3.zero;

        var meshFilters = parentObject.GetComponentsInChildren<MeshFilter>();
        var combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        parentObject.transform.GetComponent<MeshFilter>().mesh = new Mesh() { indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
        parentObject.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true, true);
        parentObject.transform.GetComponent<MeshRenderer>().material = sharedMaterial;
        parentObject.transform.GetComponent<MeshCollider>().sharedMesh = null;
        parentObject.transform.GetComponent<MeshCollider>().sharedMesh = parentObject.transform.GetComponent<MeshFilter>().mesh;
        parentObject.transform.gameObject.SetActive(true);
        parentObject.transform.position = position;
    }
}

