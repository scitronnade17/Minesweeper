using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IConfigDataService>().To<ConfigDataService>().AsSingle().NonLazy();
        Container.Bind<INeighbourMineCounter>().To<NeighbourMineCounter>().AsSingle();
        Container.Bind<IMinePlacer>().To<MinePlacer>().AsSingle();
        Container.Bind<IGridFactory>().To<GridFactory>().AsSingle();
        Container.Bind<IEmptyCellOpener>().To<EmptyCellOpener>().AsSingle();
        Container.Bind<IGameResultChecker>().To<GameResultChecker>().AsSingle();
        Container.Bind<IGameStateService>().To<GameStateService>().AsSingle();
        Container.BindInterfacesAndSelfTo<TimerService>().AsSingle().NonLazy();
        Container.Bind<CellInputHandler>().AsSingle();
    }
}