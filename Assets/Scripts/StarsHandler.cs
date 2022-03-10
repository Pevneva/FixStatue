using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StarsHandler : MonoBehaviour
{
    [SerializeField] private Sprite _starImage;

    private ParticleSystem _particlesSystem;
    private ParticleSystem.Particle[] _particles = new ParticleSystem.Particle[10];
    private readonly float _scaleFactor = 1.15f;
    private readonly float _delayDestroying = 0.85f;
    private int _count;
    private Vector3 _screenPosition;
    private RectTransform _parent;
    private RectTransform _target;

    private void OnEnable()
    {
        _particlesSystem = GetComponent<ParticleSystem>();

        Invoke(nameof(CreateStarImages), _particlesSystem.main.startLifetime.constantMax); 
    }

    public void Init(RectTransform parent, RectTransform target)
    {
        _parent = parent;
        _target = target;
    }

    private void CreateStarImages()
    {
        _count = _particlesSystem.GetParticles(_particles);

        for (int i = 0; i < _count; i++)
        {
            _screenPosition = Camera.main.WorldToScreenPoint(_particles[i].position);
            
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_parent, _screenPosition, Camera.main, out Vector2 localPointInRec))
                ProcessImage(localPointInRec, _starImage);
        }
    }

    private void ProcessImage(Vector2 localPointInRec, Sprite sprite)
    {
        GameObject go = new GameObject();
        Image star = go.AddComponent<Image>();
        star.sprite = sprite;

        var rectTransform = go.GetComponent<RectTransform>();
        rectTransform.SetParent(_parent);
        rectTransform.localPosition = localPointInRec;
        rectTransform.localScale = _scaleFactor * new Vector3(1, 1, 1);
        rectTransform.localRotation = Quaternion.Euler(rectTransform.localRotation.x, rectTransform.localRotation.y, rectTransform.localRotation.z + 10);
        rectTransform.DOLocalMove(_target.localPosition, ParamsController.Star.FlyingTime);  
        StartCoroutine(WaitBeforeDoPulse(ParamsController.Star.FlyingTime - 0.35f));
    }

    private IEnumerator WaitBeforeDoPulse(float delay)
    {
        yield return new WaitForSeconds(delay);
        _target.GetComponent<StarAnimation>().DoPulse();

        foreach (var child in _parent.GetComponentsInChildren<Image>())
        {
            Destroy(child.gameObject, _delayDestroying);
        }
    }
}