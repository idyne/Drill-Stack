using UnityEngine;
using TMPro;
using System.Linq;

public class Wall : MonoBehaviour
{
    [Range(1, 100)]
    [SerializeField] private float power = 10;
    [SerializeField] private bool randomPower = false;
    [SerializeField] private float min = 10, max = 20;
    [SerializeField] private bool isRight = false;
    [SerializeField] private GameObject currentWallObject = null;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private Transform meshContainer;
    [SerializeField] private GameObject leftWall, rightWall;
    [SerializeField] private Material[] mainMaterials, edgeMaterials, darkMaterials;
    private WallColor color = WallColor.GREEN;
    private float currentPower = 10;
    public float radius = 5.0F;
    public float _power = 10.0F;

    public float CurrentPower { get => currentPower; }

    private void Awake()
    {
        SetWallSide();
        Initiate();
    }

    public void Explode()
    {
        canvas.gameObject.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
        Vector3 explosionPos = transform.position;
        explosionPos.z -= 2;
        explosionPos.y += 3;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(_power * 150, explosionPos, radius, 0.1F, ForceMode.Impulse);
        }
        LeanTween.delayedCall(6, () => { gameObject.SetActive(false); });
    }
    public bool GetPenetrated(float powerDiff)
    {
        if (currentPower <= 0) return false;
        currentPower -= powerDiff;
        print(powerDiff);
        if (currentPower <= 0)
        {
            Explode();
        }
        SetPowerText();
        return true;
    }
    public void Initiate()
    {
        SetWallPower();
    }
    private void SetWallSide()
    {
        if (currentWallObject) DestroyImmediate(currentWallObject);
        currentWallObject = Instantiate(isRight ? rightWall : leftWall, meshContainer);
    }
    public void SetWallPower()
    {
        
        if (randomPower)
            power = Random.Range(min, max);
        power = Mathf.Ceil(power);
        currentPower = power;
        SetPowerText();
        SetColor();
    }

    private void SetPowerText()
    {
        powerText.text = Mathf.CeilToInt(currentPower).ToString();
    }

    private void SetColor()
    {
        WallMaterialSetter setter = currentWallObject.GetComponent<WallMaterialSetter>();
        if (power < 10)
            color = WallColor.GREEN;
        else if (power < 20)
            color = WallColor.YELLOW;
        else if (power < 30)
            color = WallColor.RED;
        else
            color = WallColor.PURPLE;
        int index = (int)color;
        setter.SetMaterials(mainMaterials[index], edgeMaterials[index], darkMaterials[index]);
    }

    public enum WallColor { GREEN, YELLOW, RED, PURPLE }
}
