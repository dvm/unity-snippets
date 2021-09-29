using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGenerator = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            if (mapGenerator.autoUpdate)
            {
                mapGenerator.Generate();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            mapGenerator.Generate();
        }

        if (GUILayout.Button("Random Generate"))
        {
            mapGenerator.GenerateRandom(Mathf.RoundToInt(Random.Range(-100000, 100000)));
        }

        if (GUILayout.Button("Save to PNG"))
        {
            if (mapGenerator.NoiseTexture == null)
            {
                return;
            }

            string path = EditorUtility.SaveFilePanelInProject("Save png",
                mapGenerator.NoiseTexture.name, "png",
                "Please enter a file name to save the texture to");
            if (path.Length != 0)
            {
                byte[] pngData = mapGenerator.NoiseTexture.EncodeToPNG();
                if (pngData != null)
                {
                    File.WriteAllBytes(path, pngData);
                    AssetDatabase.Refresh();
                }
            }
        }
    }
}
