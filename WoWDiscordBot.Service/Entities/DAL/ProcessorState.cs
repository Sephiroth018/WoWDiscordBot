﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace WoWDiscordBot.Service.Entities.DAL
{
    [UsedImplicitly(ImplicitUseKindFlags.Access)]
    public class ProcessorState : BaseEntity
    {
        [Index(IsUnique = true)]
        public ProcessorTypes ProcessorType { get; set; }

        public DateTimeOffset? LastCompleted { get; set; }

        public DateTimeOffset LastStarted { get; set; }

        public DateTimeOffset NextExecution { get; set; }

        public ProcessorStates State { get; set; }
    }
}