using BoardTower.Boot.Data.Entity;
using BoardTower.Boot.Domain.Ports;
using BoardTower.Boot.Domain.UseCase;
using BoardTower.Boot.Presentation.Presenter;
using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace BoardTower.Boot.Installer
{
    public sealed class BootInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();

            // Entity
            builder.Register<BootStateEntity>(Lifetime.Scoped);

            // Ports
            builder.Register<BootStatePorts>(Lifetime.Scoped);

            // UseCase
            builder.Register<BootStateUseCase>(Lifetime.Scoped);

            // Presenter
            builder.UseEntryPoints(Lifetime.Scoped, entryPoints =>
            {
                entryPoints.Add<BootStatePresenter>();
            });
        }
    }
}