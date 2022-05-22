using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class DamageComponent : MonoBehaviour
    {
        [SerializeField] private int _damage;        
        [SerializeField] private int _heal;
        public void ApplyDamage(GameObject target)
        {
            // при такой работе с компонентами нельзя использовать конструкцию healthComponent?.метод
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.ApplyDamage(_damage);
            }            
        }        

        public void ApplyHeal(GameObject target)
        {
            // при такой работе с компонентами нельзя использовать конструкцию healthComponent?.метод
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.ApllyHeal(_heal);
            }
        }
    }
}
