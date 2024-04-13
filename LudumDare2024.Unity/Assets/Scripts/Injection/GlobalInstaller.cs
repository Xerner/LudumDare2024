using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ISummoningService>().To<SummoningService>().AsSingle().NonLazy();
        Container.Bind<ILineScoringService>().To<LineScoringService>().AsSingle().NonLazy();
    }
}
