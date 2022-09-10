using System;
using Model.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace MonoBehaviours.Combat
{
    public class HealthSlider : MonoBehaviour
    {
        [SerializeReference] public IDamageable Damageable;
        [SerializeReference] public IHaveHealth TrackHealthOf;
        public Slider slider;
        private void Start()
        {
            slider ??= GetComponent<Slider>();
            SynchronizeCurrentHealth();
            BindListeners();
        }
        private void BindListeners()
        {
            Damageable.Damaged += _ =>
            {
                SynchronizeCurrentHealth();
            };
        }
        private void SynchronizeCurrentHealth()
        {
            if (TrackHealthOf == null) return;
            slider.value = TrackHealthOf.CurrentHealthNormalized();
        }
    }
}