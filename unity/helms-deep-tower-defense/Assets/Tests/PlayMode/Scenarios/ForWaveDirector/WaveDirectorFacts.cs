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
        }
    }
}
