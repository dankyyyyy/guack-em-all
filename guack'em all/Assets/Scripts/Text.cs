using UnityEngine;
using TMPro;

public class Breathing3DText : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public float breathSpeed = 1.5f;  // Speed of the breathing effect
    public float depthAmount = 0.1f;  // Very subtle forward/backward movement
    public float scaleAmount = 0.02f; // Very subtle scale effect

    private TMP_TextInfo textInfo;

    private void Start()
    {
        textMeshPro.ForceMeshUpdate(); // Ensures text is updated on start
        textInfo = textMeshPro.textInfo;
    }

    private void Update()
    {
        // Force an update each frame
        textMeshPro.ForceMeshUpdate();
        ApplyBreathingEffect();
    }

    void ApplyBreathingEffect()
    {
        // Calculate depth and scale based on sine wave
        float depth = Mathf.Sin(Time.time * breathSpeed) * depthAmount;
        float scaleFactor = 1 + Mathf.Sin(Time.time * breathSpeed) * scaleAmount;

        // Iterate over each character in the text
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue; // Skip invisible characters

            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            // Apply subtle movement (depth) and scaling to the character's vertices
            for (int j = 0; j < 4; j++) // Each character has 4 vertices
            {
                Vector3 originalVertex = vertices[vertexIndex + j];

                // Apply depth (move on Z-axis)
                vertices[vertexIndex + j] = originalVertex + new Vector3(0, 0, depth);

                // Apply scaling based on distance from the center (no distortion)
                Vector3 center = (vertices[vertexIndex] + vertices[vertexIndex + 1] + vertices[vertexIndex + 2] + vertices[vertexIndex + 3]) / 4;
                vertices[vertexIndex + j] = center + (vertices[vertexIndex + j] - center) * scaleFactor;
            }
        }

        // Update the mesh with new vertices
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMeshPro.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}
