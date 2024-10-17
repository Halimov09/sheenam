﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

namespace Sheenam.Api.Brokers.Loggings
{
    public interface IloggingBroker
    {
        void LogError (Exception exception);
        void LogCritical (Exception exception);
    }
}