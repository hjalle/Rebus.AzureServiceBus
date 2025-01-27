﻿using System;
using Azure.Messaging.ServiceBus;
using NUnit.Framework;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Exceptions;
using Rebus.Tests.Contracts;

namespace Rebus.AzureServiceBus.Tests;

[TestFixture]
public class FailsWhenSendingToNonExistentQueue : FixtureBase
{
    static readonly string ConnectionString = AsbTestConfig.ConnectionString;

    [Test]
    public void YesItDoes()
    {
        var activator = new BuiltinHandlerActivator();

        Using(activator);

        Configure.With(activator)
            .Transport(t => t.UseAzureServiceBus(ConnectionString, "bimmelim"))
            .Start();

        var exception = Assert.ThrowsAsync<RebusApplicationException>(async () =>
        {
            await activator.Bus.Advanced.Routing.Send("yunoexist", "hej med dig min ven!");
        });

        Console.WriteLine(exception);

        var notFoundException = (ServiceBusException) exception.InnerException;

        Console.WriteLine(notFoundException);

        var bimse = notFoundException.ToString();


    }
}