using System;
using Xunit;
using FluentAssertions;
using VerticalHandoverPrediction.Mobile;

namespace UnitTests
{
    public class MobileTerminalTests
    {
        [Fact]
        public void CreateMobileTerminalTestFixturePass()
        {
            var mobileTerminal = MobileTerminal.CreateMobileTerminal(Modality.DualMode);
            
            //Assert
            mobileTerminal.Activated.Should().BeFalse();
            mobileTerminal.CallHistoryLogs.Should().BeEmpty();
            mobileTerminal.Modality.Should().Be(Modality.DualMode);
            mobileTerminal.SessionId.Should().Be(Guid.Empty);
            mobileTerminal.State.Should().Be(MobileTerminalState.Idle);
            mobileTerminal.MobileTerminalId.Should().NotBeEmpty();
        }
    }
}
