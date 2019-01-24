using System;
using System.Threading.Tasks;
using DocWorks.Common.SEDAEvents.Examples.Helper;
using DocWorks.Common.SEDAEvents.Implementation.Sedav2;
using DocWorks.Common.SEDAEvents.Implementation.Sedav2.Publishing;
using DocWorks.Common.SEDAEvents.Implementation.Sedav2.Utilities;
using DocWorks.Common.SEDAEvents.MongoDb;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DocWorks.Common.SEDAEvents.Examples.Examples
{
    [Collection("MongoRunner Collection")]
    public class ParallelEventTest
    {
        private const int tasks = 5000;
        private readonly TestInitialization testInitialization;
        private EventBusService ebs;
        
        public ParallelEventTest(Mongo2GoRunnerFixture fixture)
        {
            testInitialization = new TestInitialization(fixture.Runner.ConnectionString);
        }
        
        [Fact]
        public async Task MassChildEventTest()
        {
            ebs = await TestSetupEbs.SetupAndConfigure(x=>
            {
                x.AddSingleton(typeof(IMongoClientFactory),
                    new MongoClientFactory(testInitialization.settings));
            }, false, typeof(H1), typeof(H2));
            await ebs.processMessage(new M1().ProduceOriginal());
            Assert.Equal(H2.counter, tasks);
        }
        
        public class M1 : EventPayloadBase{}
        public class M2 : EventPayloadBase{}
        
        public class H1:IHandleMessages<M1>{
            private readonly IChildEventPublisher<H1, M2, M1> _publisher;
            private readonly IServiceProvider _serviceProvider;

            public H1(IChildEventPublisher<H1, M2, M1> publisher, IServiceProvider serviceProvider)
            {
                _publisher = publisher;
                _serviceProvider = serviceProvider;
            }
            public async Task Handle(M1 message)
            {
                Parallel.For(0, tasks, (i, y) =>
                {
                    _serviceProvider.GetService<IChildEventPublisher<H1, M2, M1>>().Publish(new M2(), message, PublishingScope.Local).GetAwaiter().GetResult();
                });
                await Task.CompletedTask;
            }
        }
        
        public class H2:IHandleMessages<M2>
        {
            public static int counter;
            public async Task Handle(M2 message)
            {
                lock ("x")
                {
                    counter++;
                }

                await Task.CompletedTask;
            }
        }
        
    }
}