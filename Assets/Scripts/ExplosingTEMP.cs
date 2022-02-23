using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ExplosingTEMP : MonoBehaviour
{
    private readonly float _delayBeforeKinematic = 0.05f;
    private float _radius = 50.0f;
    private float _power = 100f;
    private Collider[] _colliders;
    private Coroutine _waitingKinematic;
    private float _angle = -180;
    private float _force = 0.1f;

    void OnEnable()
    {
        Vector3 explosionPos = transform.position;
        _colliders = Physics.OverlapSphere(explosionPos, _radius);
        var angleStep = 360 / (_colliders.Length - 1);
        foreach (Collider hit in _colliders)
        {
            Rigidbody rigidBody = hit.GetComponent<Rigidbody>();

            if (rigidBody != null)
            {
                Debug.Log("angle : " + _angle);
                Debug.Log("Cos : " + 10 * Mathf.Cos(_angle * Mathf.Deg2Rad));
                Debug.Log("Sin : " + 10 * Mathf.Sin(_angle * Mathf.Deg2Rad));
                Vector3 direction = new Vector3(transform.position.x +Mathf.Cos(_angle * Mathf.Deg2Rad), 0,
                    transform.position.z + Mathf.Sin(_angle * Mathf.Deg2Rad));
                rigidBody.AddForce(_force * direction, ForceMode.Impulse);
                // rigidBody.AddForce(_force * new Vector3(10 * Mathf.Cos(_angle * Mathf.Deg2Rad),0,10 * Mathf.Sin(_angle * Mathf.Deg2Rad)), ForceMode.Impulse);
                _angle += angleStep;
            }

            //     rigidBody.AddExplosionForce(_power, explosionPos, _radius);

            _waitingKinematic = StartCoroutine(WaitBeforeDoingKinematic(rigidBody, _delayBeforeKinematic));
        }
    }

    private IEnumerator WaitBeforeDoingKinematic(Rigidbody rigidbody, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Do Kinematic !");
        if (rigidbody != null)
            rigidbody.isKinematic = true;
    }
}