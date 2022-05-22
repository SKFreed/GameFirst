using PixelCrew;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class AddCoinsComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _coin;
        [SerializeField] private Hero _hero;
        
        public void AddCoin()
        {
            if (_coin.tag == "SilverCoin" && _coin != null)
            {                
                _hero.AddCoinsToHero(1f);
            }
            else if (_coin.tag == "GoldCoin" && _coin != null)
            {               
                _hero.AddCoinsToHero(10f);
            }
        }
        public void CheatCoin(float _cheatCoin)
        {
            _hero.AddCoinsToHero(_cheatCoin);
        }
    }

}
