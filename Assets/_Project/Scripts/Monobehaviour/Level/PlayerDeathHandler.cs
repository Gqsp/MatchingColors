using System;
using System.Collections;
using GaspDL.Audio;
using UnityEngine;

// Script horrible fait à l'arrache pour le prototype, à refaire proprement
public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] InputHandler _inputHandler;
    [SerializeField] AnimationHandler _animationHandler;
    [SerializeField] RespawnHandler _respawnHandler;
    [SerializeField] private GameObject _player;
    [SerializeField] private EntityMovementCore _movementCore;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] EntityAbilityManager _abilityManager;
    [SerializeField] ParticleSystem _deathParticles;
    
    public static event Action OnPlayerDeath;

    bool _dead;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_dead) return;
        if (other.CompareTag("KillZone"))
        {
            _dead = true;
            _inputHandler.LockInputs = true;
            _collider.enabled = false;
            _abilityManager.PauseAbilities(true);
            
            GameData.deaths++;
            
            _animationHandler.SetTrigger("Death");
            
            Timer.PauseEvent?.Invoke(true);
            
            var particle = Instantiate(_deathParticles, null);
            particle.transform.position = _player.transform.position;
            OnPlayerDeath?.Invoke();
            AudioManager.Instance.PlaySound("Death");

            StartCoroutine(WaitForDeathAnimation());
        }
    }

    private IEnumerator WaitForDeathAnimation()
    {
        float time = 0;

        while (time < 1f)
        {
            time += Time.deltaTime;

            var v = _movementCore.LastFrameVelocity;

            v.x = Mathf.Max(Mathf.Abs(v.x), 10f) * Mathf.Sign(v.x);
            v.y = 0;
            _movementCore.Velocity = v;
            yield return new WaitForFixedUpdate();
        }

        _respawnHandler.RespawnPlayer(_player);

        _movementCore.Velocity = Vector2.zero;
        _inputHandler.LockInputs = false;
        _dead = false;
        _animationHandler.SetTrigger("Revive");
        _abilityManager.PauseAbilities(false);
        _collider.enabled = true;
        Timer.PauseEvent?.Invoke(false);
    }
}