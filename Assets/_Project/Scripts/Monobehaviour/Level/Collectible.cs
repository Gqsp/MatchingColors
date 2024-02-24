using System;
using System.Collections;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] Collider2D _collider;
    [SerializeField] GameObject _collectibleObject;
    [SerializeField] LevelHandler _level;
    [SerializeField] bool _waitOnlyForGrounded;
    
    private Vector3 _defaultPosition;
    private bool _followState;

    private void Start()
    {
        _defaultPosition = _collectibleObject.transform.position;
        _level = FindObjectOfType<LevelHandler>();
        PlayerDeathHandler.OnPlayerDeath += OnPlayerDied;
    }

    private void OnDestroy()
    {
        PlayerDeathHandler.OnPlayerDeath -= OnPlayerDied;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_followState) return;
        _followState = true;
        _collider.enabled = false;
        
        StartCoroutine(FollowTarget(other.transform.Find("PlayerFollowPoint"), other.GetComponent<EntityCollisionSolver>()));
    }
    
    Vector3 _velocity = Vector3.zero;
    
    IEnumerator FollowTarget(Transform target, EntityCollisionSolver solver)
    {
        while (_followState)
        {
            if (solver.CollisionData.CheckCollisionDown(_level.TilemapLayer.value))
            {
                if (_waitOnlyForGrounded || _enteredValidationZone)
                {
                    Collect();
                    yield break;
                }
            }
            else
            {
                Debug.Log(_level.TilemapLayer.value);
            }
            
            _collectibleObject.transform.position = Vector3.SmoothDamp(_collectibleObject.transform.position, target.position, ref _velocity, 0.5f);
            yield return null;
        }
    }
    
    IEnumerator LerpToDefaultPosition(Action callback)
    {
        float t = 0;
        Vector3 startPosition =  _collectibleObject.transform.position;
        
        while (t < 1)
        {
            t += Time.deltaTime;
            _collectibleObject.transform.position = Vector3.Lerp(startPosition, _defaultPosition, t);
            yield return null;
        }
        
        callback?.Invoke();
    }
    
    private bool _enteredValidationZone;
    public void OnPlayerEnterValidationZone()
    {
        if (!_followState) return;
        _enteredValidationZone = true;
    }

    private void Collect()
    {
        GameData.collectables++;
        Destroy(gameObject);
    }
    
    private void OnPlayerDied()
    {
        if (!_followState) return;
        _followState = false;
        _enteredValidationZone = false;
        
        StartCoroutine(LerpToDefaultPosition(() =>
        {
            _collider.enabled = true;
        }));
    }
}
