using UI;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI
{
    public class MainMenuLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<MainMenuUI>().AsSelf();
        }
    }
}
