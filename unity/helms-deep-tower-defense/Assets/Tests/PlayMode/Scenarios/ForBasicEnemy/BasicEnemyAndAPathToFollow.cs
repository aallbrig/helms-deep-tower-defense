using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForBasicEnemy
{
    public class BasicEnemyAndAPathToFollow
    {
        private void Setup(in List<GameObject> destroyList, out GameObject sutInstance)
        {
            sutInstance = new GameObject();
            destroyList.Add(sutInstance);
        }

        private void Teardown(List<GameObject> gameObjects)
        {
            for (int i = gameObjects.Count - 1; i >= 0; i--)
                Object.Destroy(gameObjects[i]);
            gameObjects.Clear();
        }

        [UnityTest]
        public IEnumerator BasicEnemy_CanFollow_APath()
        {
            var destroyList = new List<GameObject>();
            Setup(destroyList, out var enemy);
            var path = new GameObject();

            yield return null;
            Teardown(destroyList);

            Assert.NotNull(enemy);
        }
    }
}