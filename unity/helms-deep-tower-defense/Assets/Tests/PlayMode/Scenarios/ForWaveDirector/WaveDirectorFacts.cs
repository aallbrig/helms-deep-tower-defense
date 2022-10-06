using System.Collections;
using Model.Factories;
using NUnit.Framework;
using UnityEngine.Playables;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForWaveDirector
{
    public class WaveDirectorFacts
    {
        private readonly PrefabSpawner _waveDirectorSpawner = new PrefabSpawner("Prefabs/Directors/Wave Director");
        [UnityTest]
        public IEnumerator WaveDirector_Exists_AndIsAThing()
        {
            var waveDirector = _waveDirectorSpawner.Spawn();
            var directorComponent = waveDirector.GetComponent<PlayableDirector>();
            yield return null;
            Assert.IsNotNull(waveDirector);
            Assert.IsNotNull(directorComponent);
            Assert.IsTrue(directorComponent.playOnAwake);

            /*
            // TODO: I should be able to assert that bound game objects start off deactivated
            var gameObject1 = new GameObject();
            var gameObject2 = new GameObject();
            var gameObject3 = new GameObject();
            Selection.activeGameObject = waveDirector;
            using var playableAssetOutputs = directorComponent.playableAsset.outputs.GetEnumerator();
            directorComponent.SetGenericBinding(playableAssetOutputs.Current.sourceObject, gameObject1);
            playableAssetOutputs.MoveNext();
            directorComponent.SetGenericBinding(playableAssetOutputs.Current.sourceObject, gameObject2);
            playableAssetOutputs.MoveNext();
            directorComponent.SetGenericBinding(playableAssetOutputs.Current.sourceObject, gameObject3);
            directorComponent.RebindPlayableGraphOutputs();

            directorComponent.time = directorComponent.initialTime;
            Assert.IsFalse(gameObject3.activeSelf);
            Assert.IsFalse(gameObject2.activeSelf);
            Assert.IsFalse(gameObject1.activeSelf);
            */
        }
    }
}
