using UnityEngine;
using TMPro;

public class PlanetGraphics : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private TMP_Text text;
    
    private readonly int _outline = Shader.PropertyToID("_Outline");

    private void Awake()
    {
        mesh.material = new Material(outlineMaterial);
    }

    public void SetPlanetColor(Color32 color)
    {
        mesh.material.color = color;
    }
    
    public void UpdateText(string txt)
    {
        text.text = txt;
    }
    
    public void SetGraphicState(bool state)
    {
        mesh.material.SetFloat(_outline,  state ? 0.25f : 0f);
    }
    
}