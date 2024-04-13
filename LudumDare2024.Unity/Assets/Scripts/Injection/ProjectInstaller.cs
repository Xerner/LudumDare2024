using Zenject;

public class ProjectInstaller : Installer<ProjectInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<ISummoningService>().To<SummoningService>().AsSingle().NonLazy();
    }
}