using SaveLoad;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI
{
    public class LevelLifetimeScope : LifetimeScope
    {
        [SerializeField] private AbilityConfig _abilityConfig;
        [SerializeField] private BombAbilityObject _bombAbilityObjectPrefab;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<CameraEffectsSystem>(Lifetime.Singleton).AsSelf().WithParameter(Camera.main);
            
            builder.RegisterInstance(_abilityConfig.AbilityData).AsSelf();

            RegisterLevelUI(builder);

            builder.RegisterComponentInHierarchy<MergeGameSystem>().AsSelf();

            RegisterAbilitySystem(builder);
            
            builder.RegisterEntryPoint<LevelSaver>().AsSelf();
            
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
