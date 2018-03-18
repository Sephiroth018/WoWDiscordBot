using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using HeroismDiscordBot.Service.Entities.DAL;
using HeroismDiscordBot.Service.Logging;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace HeroismDiscordBot.Service.Processors
{
    public class ProcessorManager<T> : IProcessorManager
        where T : class, IProcessor
    {
        private readonly Container _container;
        private readonly ILogger _logger;
        private readonly Func<IRepository> _repositoryFactory;
        private Timer _timer;

        public ProcessorManager(ILogger logger, Container container, Func<IRepository> repositoryFactory)
        {
            _logger = logger;
            _container = container;
            _repositoryFactory = repositoryFactory;
        }

        public void Start()
        {
            _timer = new Timer();
            _timer.Elapsed += (sender, args) => Task.Factory.StartNew(ExecuteProcessor);
            _timer.Interval = new TimeSpan(1, 0, 0).TotalMilliseconds;
            _timer.AutoReset = true;
            _timer.Start();
            Task.Factory.StartNew(ExecuteProcessor);
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void ExecuteProcessor()
        {
            try
            {
                using (var repository = _repositoryFactory.Invoke())
                {
                    using (AsyncScopedLifestyle.BeginScope(_container))
                    {
                        var processor = _container.GetInstance<T>();
                        var state = repository.ProcessorStates.FirstOrDefault(s => s.ProcessorType == processor.ProcessorType);

                        if (state == null)
                        {
                            state = repository.ProcessorStates.Create();
                            state.ProcessorType = processor.ProcessorType;
                            repository.ProcessorStates.Add(state);
                        }

                        state.State = ProcessorStates.Running;
                        state.LastStarted = DateTimeOffset.Now;
                        state.NextExecution = DateTimeOffset.Now.Add(processor.GetNextOccurence());

                        repository.SaveChanges();

                        try
                        {
                            processor.DoWork();

                            state.State = ProcessorStates.Waiting;
                            state.LastCompleted = DateTimeOffset.Now;
                        }
                        catch (Exception)
                        {
                            state.State = ProcessorStates.Failed;

                            throw;
                        }
                        finally
                        {
                            var nextOccurence = processor.GetNextOccurence();
                            _timer.Interval = nextOccurence.TotalMilliseconds;
                            state.NextExecution = DateTimeOffset.Now.Add(nextOccurence);

                            repository.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }
    }
}