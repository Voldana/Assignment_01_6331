using System.Collections.Generic;
using Project.Scripts.Environment;
using Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private List<Harbor> harbors;
        [SerializeField] private LevelMenu levelMenu;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.BindInstance(harbors).AsSingle();
            
            Container.BindFactory<LevelMenu, LevelMenu.Factory>().FromComponentInNewPrefab(levelMenu).AsSingle();
            
            Container.DeclareSignal<GameEvents.OnPirateDestroy>();
            Container.DeclareSignal<GameEvents.OnCollision>();
            Container.DeclareSignal<GameEvents.OnGameEnd>();
        }
    }
}