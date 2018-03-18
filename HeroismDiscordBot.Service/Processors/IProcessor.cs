using System;

namespace HeroismDiscordBot.Service.Processors
{
    public interface IProcessor
    {
        void DoWork();
        TimeSpan GetNextOccurence();
    }
}