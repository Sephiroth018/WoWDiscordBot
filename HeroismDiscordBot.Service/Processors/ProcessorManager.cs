using System;
using System.Threading.Tasks;
using System.Timers;
using HeroismDiscordBot.Service.Logging;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace HeroismDiscordBot.Service.Processors
{
    public class ProcessorManager<T> : IProcessorManager
        where T : class, IProcessor
    {
        private readonly ILogger _logger;
        private readonly Container _container;
        private Timer _timer;

        public ProcessorManager(ILogger logger, Container container)
        {
            _logger = logger;
            _container = container;
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

        private void ExecuteProcessor()
        {
            try
            {
                using (AsyncScopedLifestyle.BeginScope(_container))
                {
                    var processor = _container.GetInstance<T>();
                    processor.DoWork();
                    _timer.Interval = processor.GetNextOccurence().TotalMilliseconds;
                }
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}