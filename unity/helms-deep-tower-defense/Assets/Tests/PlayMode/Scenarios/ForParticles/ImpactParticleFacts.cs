using System.Collections;
using Model.Factories;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace Tests.PlayMode.Scenarios.ForParticles
{
    public class ImpactParticleFacts : ScenarioTest
    {
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/Particles/Impact");

        [UnityTest]
        public IEnumerator ImpactParticles_UsesA_ParticleSystem()
        {
            var prefabInstance = _prefabSpawner.Spawn();
            CleanupAtEnd(prefabInstance);
            TestCameraLookAt(prefabInstance.transform);
            yield return null;

            var particlesComponent = prefabInstance.GetComponent<ParticleSystem>();
            var shapeType = particlesComponent.shape.shapeType;
            var shapeRadius = particlesComponent.shape.radius;
            var emissionRateOverTime = particlesComponent.emission.rateOverTime.constant;
            var burstsCount = particlesComponent.emission.burstCount;
            var burstCountConstant = particlesComponent.emission.GetBurst(0).count.constant;
            var lifetime = particlesComponent.main.startLifetime.constant;
            var speed = particlesComponent.main.startSpeed.constant;
            var particleSystemRendererComponent =
                prefabInstance.GetComponent<ParticleSystemRenderer>(); // invisible component added w/ ParticleSystem
            var renderMode = particleSystemRendererComponent.renderMode;

            Assert.NotNull(particlesComponent, "particle system component not found within prefab");
            Assert.AreEqual(ParticleSystemShapeType.Sphere, shapeType, "particle shape needs to be a sphere shape");
            Assert.IsTrue(Utils.AreFloatsEqual(0.1f, shapeRadius, 10e-6f), $"shape radius {shapeRadius} expected to be 0.1f");
            Assert.AreEqual(0f, emissionRateOverTime, $"emission rate over time '{emissionRateOverTime}' expected to be 0");
            Assert.AreNotEqual(0, burstsCount, "emissions panel requires at least 1 burst profile");
            Assert.AreEqual(15f, burstCountConstant,
                $"emission bursts first profile count {burstCountConstant} expected to be 15");
            Assert.AreEqual(1f, particlesComponent.main.duration,
                $"time of {particlesComponent.time} expected to be 1 second");
            Assert.AreEqual(0.5f, lifetime, $"lifetime of {lifetime} expected to be 0.5");
            Assert.AreEqual(1.5f, speed, $"speed of {speed} expected to be 1.5");
            Assert.AreEqual(ParticleSystemRenderMode.Mesh, renderMode, $"render mode {renderMode} expected to be mesh");
            Assert.IsFalse(particlesComponent.main.loop, "impact particle should not loop");
            Assert.AreEqual(ParticleSystemStopAction.Destroy, particlesComponent.main.stopAction,
                "Impact should destroy self as stop action");
        }
    }
}