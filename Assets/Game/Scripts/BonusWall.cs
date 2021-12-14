using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using FateGames;

public class BonusWall : MonoBehaviour
{
    [Range(1, 100)]
    [SerializeField] private float power = 10;
    [SerializeField] private List<GameObject> confettiPositions;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private Material[] mainMaterials, edgeMaterials, darkMaterials;
    [SerializeField] private WallColor color = WallColor.GREEN;
    private float currentPower = 10;
    public float radius = 5.0F;
    public float _power = 10.0F;

    public float CurrentPower { get => currentPower; }

    private void Awake()
    {
        Initiate();
    }

    public void Explode()
    {
        for (int i = 0; i < confettiPositions.Count; i++)
        {
            ConfettiManager.Instance.CreateConfettiDirectional(confettiPositions[i].transform.position, confettiPositions[i].transform.rotation.eulerAngles, Vector3.one * 3);
        }
        canvas.gameObject.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
        Vector3 explosionPos = transform.position;
        explosionPos.z -= 2;
        explosionPos.y += 2;
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(_power * 150, explosionPos, radius, 1.0F, ForceMode.Impulse);
        }
        LeanTween.delayedCall(6, () => { gameObject.SetActive(false); });
    }
    public bool GetPenetrated(float powerDiff)
    {
        if (currentPower <= 0) return false;
        currentPower -= powerDiff;
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
    public void SetWallPower()
    {
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
        WallMaterialSetter setter = GetComponentInChildren<WallMaterialSetter>();
        /*if (power < 10)
            color = WallColor.GREEN;
        else if (power < 20)
            color = WallColor.YELLOW;
        else if (power < 30)
            color = WallColor.RED;
        else
            color = WallColor.PURPLE;*/
        int index = (int)color;
        setter.SetMaterials(mainMaterials[index], edgeMaterials[index], darkMaterials[index]);
    }

    public enum WallColor { GREEN, YELLOW, RED, PURPLE }
}
