using TMPro;
using UnityEngine;

[System.Serializable]
public class TextInfo
{
    #region FIELDS
    [SerializeField]
    private string name = string.Empty;
    [SerializeField]
    private TextMeshProUGUI textMesh = null;
    [SerializeField]
    private Color color = Color.white;
    #endregion

    #region PROPERTIES
    public string Name { get => name; set => name = value; }
    public TextMeshProUGUI TextMesh { get => textMesh; set => textMesh = value; }
    public Color Color { get => color; set => color = value; }
    #endregion

    #region CONSTRUCTOR
    public TextInfo(string name, TextMeshProUGUI textMesh, Color color)
    {
        SetUpValues(name, textMesh, color);
    }
    #endregion

    #region METHODS
    private void SetUpValues(string name, TextMeshProUGUI textMesh, Color color)
    {
        this.Name = name;
        this.TextMesh = textMesh;
        this.Color = color;

        this.TextMesh.color = this.Color;
    }
    #endregion
}