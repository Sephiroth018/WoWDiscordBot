using System;
using HeroismDiscordBot.Service.Entities.DAL;

namespace HeroismDiscordBot.Service.Processors
{
    public interface IProcessor
    {
        ProcessorTypes ProcessorType { get; }

        void DoWork();

        TimeSpan GetNextOccurence();
    }
}