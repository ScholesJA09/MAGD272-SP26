using UnityEngine;

namespace Utility
{
    public static class Utility 
    {
        private static GameObject _player;
        private static PlayerHealth _playerHealth;
        
        /// <summary>
        ///  Returns first Player Object found in the scene
        /// </summary>
        /// <returns></returns>
        public static GameObject FindPlayer()
        {
            if (_player)
            {
                return _player;
            }

            // var players = Object.FindObjectsOfType<PlayerHealth>();
            var players = GameObject.FindGameObjectsWithTag("Player");
            if(players.Length > 0)
            {
                return _player = players[0].gameObject;
            }

            Debug.LogError("No player found in the scene!");
            return null;
        }
        
        public static PlayerHealth FindPlayerHealth()
        {
            if (_playerHealth)
            {
                return _playerHealth;
            }
            return _playerHealth = FindPlayer().GetComponent<PlayerHealth>();
        }
    }
}
