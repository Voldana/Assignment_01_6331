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
            Container.BindInstance(harbors).AsSingle();
        }
    }
}