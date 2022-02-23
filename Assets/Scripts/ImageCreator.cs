using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ImageCreator : MonoBehaviour
{
    [SerializeField] private Sprite _starImage;

    private ParticleSystem _particlesSystem;
    private ParticleSystem.Particle[] _particles = new ParticleSystem.Particle[10];
    private float _scaleFactor = 1.2f;
    private int _count;
    private Vector3 _screenPosition;
    private Canvas _canvas;

    private void OnEnable()
    {
        _canvas = FindObjectOfType<Canvas>();
        _particlesSystem = GetComponent<ParticleSystem>();

        InvokeRepeating(nameof(ShowParticlesData), 0.65f,0.65f); //lifetime of parent ps + lifetime of current ps AAA
    }

    private void ShowParticlesData()
    {
        _count = _particlesSystem.GetParticles(_particles);

        Debug.Log("Count particles : " + _count);

        for (int i = 0; i < _count; i++)
        {
            _screenPosition = Camera.main.WorldToScreenPoint(_particles[i].position);
            var area = _canvas.GetComponent<RectTransform>();

            Debug.Log("Start Life Time : " + _particles[i].startLifetime);
            Debug.Log(i + " position : " + _particles[i].position);
            Debug.Log(i + " screen position : " + _screenPosition);

            Vector2 localPointInRec;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(area, _screenPosition, Camera.main, out localPointInRec))
                CreateImage(localPointInRec, _starImage);
        }
    }

    private void CreateImage(Vector2 localPointInRec, Sprite sprite)
    {
        GameObject go = new GameObject();
        Image star = go.AddComponent<Image>();
        star.sprite = _starImage;

        var rectTransform = go.GetComponent<RectTransform>();
        rectTransform.SetParent(_canvas.transform);
        rectTransform.localPosition = localPointInRec;
        rectTransform.localScale = _scaleFactor * new Vector3(1, 1, 1);

        rectTransform.DOLocalMove(new Vector2(0, 885.5f), 1.35f);
    }
}