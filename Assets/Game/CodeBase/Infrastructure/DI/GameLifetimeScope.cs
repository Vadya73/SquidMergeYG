using Audio;
using MyInput;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private AudioConfig _audioConfig;
        protected override void Configure(IContainerBuilder builder)
        {
            DontDestroyOnLoad(this.gameObject);
            
            builder.RegisterComponentInHierarchy<MonoHelper>().AsSelf();
            builder.RegisterComponentInHierarchy<LoadScreen>().AsSelf();
            builder.RegisterComponentInHierarchy<AudioManager>().AsSelf();

            builder.RegisterInstance(_audioConfig.AudioData).AsSelf();
            
            builder.Register<ProjectInput>(Lifetime.Singleton).AsSelf();
        }
    }
}
