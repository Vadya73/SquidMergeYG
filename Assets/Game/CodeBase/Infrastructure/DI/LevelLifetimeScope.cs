using MyInput;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI
{
    public class LevelLifetimeScope : LifetimeScope
    {
        [SerializeField] private BombAbilityObject _bombAbilityObjectPrefab;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ProjectInput>(Lifetime.Singleton).AsSelf();
            builder.RegisterComponentInHierarchy<LevelUI>().AsSelf();
            builder.RegisterComponentInHierarchy<MergeGameSystem>().AsSelf();
            
            RegisterAbilitySystem(builder);
        }

        private void RegisterAbilitySystem(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<AbilitySystem>().AsSelf().WithParameter(_bombAbilityObjectPrefab);
        }
    }
}
