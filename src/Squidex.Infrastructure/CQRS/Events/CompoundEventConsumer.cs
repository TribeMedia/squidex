﻿// ==========================================================================
//  CompoundEventConsumer.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using System.Linq;
using System.Threading.Tasks;

namespace Squidex.Infrastructure.CQRS.Events
{
    public sealed class CompoundEventConsumer : IEventConsumer
    {
        private readonly IEventConsumer[] inners;

        public string Name { get; }

        public string EventsFilter { get; }

        public CompoundEventConsumer(IEventConsumer first, params IEventConsumer[] inners)
            : this(first?.Name, first, inners)
        {
        }

        public CompoundEventConsumer(string name, IEventConsumer first, params IEventConsumer[] inners)
        {
            Guard.NotNull(first, nameof(first));
            Guard.NotNull(inners, nameof(inners));
            Guard.NotNullOrEmpty(name, nameof(name));

            this.inners = new[] { first }.Union(inners).ToArray();

            Name = name;

            EventsFilter = string.Join("|", this.inners.Where(x => !string.IsNullOrWhiteSpace(x.EventsFilter)).Select(x => $"({x.EventsFilter})"));
        }

        public Task ClearAsync()
        {
            return Task.WhenAll(inners.Select(i => i.ClearAsync()));
        }

        public async Task On(Envelope<IEvent> @event)
        {
            foreach (var inner in inners)
            {
                await inner.On(@event);
            }
        }
    }
}
