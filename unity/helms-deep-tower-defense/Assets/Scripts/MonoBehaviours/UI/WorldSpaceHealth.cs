using Model.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace MonoBehaviours.UI
{
    public class WorldSpaceHealth : MonoBehaviour
    {
        public GameObject trackTarget;
        public Slider healthBarSlider;
        private IDamageable _damageable;
        private IHaveHealth _healthHaver;
        private void Start()
        {
            healthBarSlider ??= GetComponent<Slider>();
            var targetGameObject = trackTarget != null ? trackTarget : gameObject;
            if (targetGameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                _damageable = damageable;
                _damageable.Damaged += _ => RenderCurrentHealth();
            }
            if (targetGameObject.TryGetComponent<IHaveHealth>(out var healthHaver))
                _healthHaver = healthHaver;
            RenderCurrentHealth();
        }
        private void RenderCurrentHealth()
        {
            if (_healthHaver == null) return;
            if (healthBarSlider == null) return;

            healthBarSlider.value = _healthHaver.CurrentHealthNormalized();
        }
    }
}