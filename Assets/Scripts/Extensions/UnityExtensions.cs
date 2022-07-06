using UnityEngine;

public static class UnityExtensions
{
    public static GameObject LoadPrefab(string path, Transform parent = null)
    {
        Object prefab = Resources.Load(path);

        return parent is null 
            ? Object.Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject
            : Object.Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity, parent) as GameObject;
    }

    public static Maybe<GameObject> GetItemAtCrosshair<T>()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 7.5f))
        {
            if (hit.collider != null && hit.collider.gameObject.GetComponent<T>() != null)
            {
                return Maybe<GameObject>.Some(hit.collider.gameObject);
            }
        }

        return Maybe<GameObject>.None();
    }
}

