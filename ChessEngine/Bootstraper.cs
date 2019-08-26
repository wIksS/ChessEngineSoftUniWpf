namespace ChessEngine
{
    using Autofac;
    using ChessEngine.Services.BoardGenerator;
    using ChessEngine.Services.BoardGenerator.Contracts;
    using ChessEngine.Services.Checkmate;
    using ChessEngine.Services.Checkmate.Contracts;
    using ChessEngine.Services.Engine;
    using ChessEngine.Services.Engine.Contracts;
    using System.Linq;
    using System.Reflection;
    public static class Bootstraper
    {
        public static IContainer Container { get; set; }

        public static void Init()
        {
            var builder = new ContainerBuilder();

            //builder.RegisterType<ChessGridViewModel>().AsSelf();
            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            //    .Where(t => t.Name.EndsWith("Service"))
            //    .AsImplementedInterfaces();

            builder.RegisterType<ChessRulesService>().As<IChessRulesService>();
            builder.RegisterType<CheckmateService>().As<ICheckmateService>();
            builder.RegisterType<BoardGeneratorService>().As<IBoardGeneratorService>();
            builder.RegisterType<ChessGameService>().As<IChessGameService>();
            builder.RegisterType<EmptyBoardGeneratorService>().As<IEmptyBoardGeneratorService>();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("ViewModel"))
                .AsSelf();
            //builder.RegisterType<BoardGeneratorService>()
            //    .As<IBoardGeneratorService>();

            Container = builder.Build();
        }
    }
}
