﻿// ==========================================================================
//  UpdateLanguage.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using System.Collections.Generic;
using Squidex.Infrastructure;

namespace Squidex.Domain.Apps.Write.Apps.Commands
{
    public sealed class UpdateLanguage : AppAggregateCommand, IValidatable
    {
        public Language Language { get; set; }

        public bool IsOptional { get; set; }

        public bool IsMaster { get; set; }

        public List<Language> Fallback { get; set; }

        public void Validate(IList<ValidationError> errors)
        {
            if (Language == null)
            {
                errors.Add(new ValidationError("Language cannot be null", nameof(Language)));
            }
        }
    }
}
