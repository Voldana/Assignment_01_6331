using System.Collections.Generic;
using Project.Scripts.Environment;
using UnityEngine;
using Zenject;

namespace Project.Scripts
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private List<Harbor> harbors;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.BindInstance(harbors).AsSingle();

            Container.DeclareSignal<GameEvents.OnPirateDestroy>();
            Container.DeclareSignal<GameEvents.OnCollision>();
            Container.DeclareSignal<GameEvents.OnGameEnd>();
        }
    }
}