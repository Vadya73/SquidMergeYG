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
            
            RegisterLevelUI(builder);
            
            builder.RegisterComponentInHierarchy<MergeGameSystem>().AsSelf();
            
            RegisterAbilitySystem(builder);
        }

        private void RegisterLevelUI(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<LevelUI>().AsSelf();
            builder.RegisterEntryPoint<LevelUIPresenter>().AsSelf();
        }

        private void RegisterAbilitySystem(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<AbilitySystem>().AsSelf().WithParameter(_bombAbilityObjectPrefab);
        }
    }
}
