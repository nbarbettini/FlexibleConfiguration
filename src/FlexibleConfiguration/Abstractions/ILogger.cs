// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace FlexibleConfiguration.Abstractions
{
    public interface ILogger
    {
        void Log(LogEntry entry);
    }
}
