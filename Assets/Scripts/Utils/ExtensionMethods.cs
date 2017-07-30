using UnityEngine;


public static class ExtensionMethods
{
    public static GameObject AddChild(this GameObject parent, GameObject prefab)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;
        if (go != null && parent != null)
        {
            Transform t = go.transform;
            if (go.GetComponent<RectTransform>() != null) t.SetParent(parent.transform);
            else t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
        return go;
    }
}