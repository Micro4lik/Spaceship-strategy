using UnityEngine;

public class SpaceshipGraphics : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    
    public void SetSpaceshipColor(Color32 color)
    {
        mesh.material.color = color;
    }
}
