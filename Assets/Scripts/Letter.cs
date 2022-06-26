using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Letter : MonoBehaviour
{
    public Material[] myMats;

    public void SetColor(int color)
    {
        GetComponent<MeshRenderer>().material = myMats[color];
    }
    public void HitColor(int color)
    {
        StartCoroutine(ChangingColor(color));
    }

    IEnumerator ChangingColor(int color)
    {
        var mat = GetComponent<MeshRenderer>().material;
        var curColor = mat.color;
        var curEmissionColor = mat.GetColor("_EmissionColor");
        float dur = GameManager.Instance.colorDuration;
        mat.DOColor(GameManager.Instance.hitColors[color], dur);
        mat.SetColor("_EmissionColor",GameManager.Instance.hitColors[color]);
        yield return new WaitForSeconds(dur);
        mat.SetColor("_EmissionColor",curEmissionColor);
        mat.DOColor(curColor, dur);
    }
}
