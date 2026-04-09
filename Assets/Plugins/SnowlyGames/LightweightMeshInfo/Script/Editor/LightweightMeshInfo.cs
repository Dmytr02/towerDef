using UnityEngine;
using UnityEditor;

public class LightweightMeshInfo : EditorWindow
{
    // Variables to store mesh information
    private int vertexCount;
    private int submeshCount;
    private int triangleCount;

    // Strings used for the window title, and menu title. (Found in Tools -> Mesh Info).
    // Change as you want or leave as it is.
    private const string windowTitle = "Mesh Info";
    private const string menuItemTitle = "Tools/Mesh Info";

    // Strings used for the variables, no need to change.
    private const string verticesTitle = "Vertices: ";
    private const string trianglesTitle = "Triangles: ";
    private const string submeshesTitle = "SubMeshes: ";

    [MenuItem(menuItemTitle)]
    static void Init()
    {
        LightweightMeshInfo window = (LightweightMeshInfo)EditorWindow.GetWindow(typeof(LightweightMeshInfo));
        window.titleContent.text = windowTitle;
    }

    void OnSelectionChange()
    {
        Repaint();
    }

    void OnGUI()
    {
        vertexCount = 0;
        triangleCount = 0;
        submeshCount = 0;

        foreach (GameObject g in Selection.gameObjects)
        {
            CountMeshInfo(g);
        }

        EditorGUILayout.LabelField(verticesTitle, vertexCount.ToString());
        EditorGUILayout.LabelField(trianglesTitle, triangleCount.ToString());
        EditorGUILayout.LabelField(submeshesTitle, submeshCount.ToString());
    }

    void CountMeshInfo(GameObject g)
    {
        MeshFilter mf = g.GetComponent<MeshFilter>();
        if (mf != null && mf.sharedMesh != null)
        {
            vertexCount += mf.sharedMesh.vertexCount;
            triangleCount += mf.sharedMesh.triangles.Length / 3;
            submeshCount += mf.sharedMesh.subMeshCount;
        }

        SkinnedMeshRenderer smr = g.GetComponent<SkinnedMeshRenderer>();
        if (smr != null && smr.sharedMesh != null)
        {
            vertexCount += smr.sharedMesh.vertexCount;
            triangleCount += smr.sharedMesh.triangles.Length / 3;
            submeshCount += smr.sharedMesh.subMeshCount;
        }

        foreach (Transform child in g.transform)
        {
            CountMeshInfo(child.gameObject);
        }
    }
}
