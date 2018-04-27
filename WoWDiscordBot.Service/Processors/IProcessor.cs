using System;
using WoWDiscordBot.Service.Entities.DAL;

namespace WoWDiscordBot.Service.Processors
{
    public interface IProcessor
    {
        ProcessorTypes ProcessorType { get; }

        void DoWork();

        TimeSpan GetNextOccurence();
    }
}