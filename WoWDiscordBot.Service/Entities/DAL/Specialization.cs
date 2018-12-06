﻿using JetBrains.Annotations;

namespace WoWDiscordBot.Service.Entities.DAL
{
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
    public class Specialization : BaseEntity
    {
        public string Name { get; set; }

        public int ItemLevel { get; set; }

        public string Role { get; set; }

        public virtual Character Character { get; set; }

        public string GetDescription()
        {
            return $"{Name}-{Role} ({ItemLevel})";
        }
    }
}