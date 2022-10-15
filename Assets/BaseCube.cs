using TMPro;
using UnityEngine;

public class BaseCube : MonoBehaviour
{
    [SerializeField] private TextMeshPro valueText;
    public int CubeValue { get; private set; }

    private void Start()
    {
        CubeValue = Random.Range(1, 5);
        valueText.text = CubeValue.ToString();
    }

    private void IncreaseCubeValue(int amount)
    {
        CubeValue += amount;
    }
}
