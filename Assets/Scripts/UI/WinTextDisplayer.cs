using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(TMP_Text))]
public class WinTextDisplayer : MonoBehaviour
{
    private readonly float posiitionYFactor = 0.1f; 
    private readonly float scaleFactor = 1.5f;
    private TMP_Text _winText;
    private Vector3 _startPosition;
    private Vector3 _startScale;
    private List<string> _words = new List<string>() {"cool", "good"};

    public void ShowRandomWinText()
    {
        _winText = GetComponent<TMP_Text>();
        _winText.text = _words[Random.Range(0, _words.Count)];
        _winText.gameObject.SetActive(true);
        _startPosition = transform.position;
        _startScale = transform.localScale;
        
        var showingTime = 3 * ParamsController.Level.DelayBeforeEndLevel / 4; //AAA
        _winText.DOFade(0, 0);
        _winText.DOFade(1, ParamsController.Level.DelayBeforeEndLevel / 2);
        _winText.transform.DOMoveY(transform.position.y + posiitionYFactor, 3 * showingTime / 4);
        _winText.transform.DOScale(transform.localScale * scaleFactor, 3 * showingTime / 4);
        Invoke(nameof(DisableText), showingTime);
    }

    private void DisableText()
    {
        transform.position = _startPosition;
        transform.localScale = _startScale;
        _winText.gameObject.SetActive(false);
    }
}