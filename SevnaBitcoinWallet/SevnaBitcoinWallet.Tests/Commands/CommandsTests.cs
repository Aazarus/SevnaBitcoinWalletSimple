// <copyright file="CommandsTests.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Tests.Commands
{
  using System.Collections.Generic;
  using FluentAssertions;
  using SevnaBitcoinWallet.Commands;
  using Xunit;

  /// <summary>
  /// Tests the Commands class.
  /// </summary>
  public class CommandsTests
  {
    /// <summary>
    /// Tests the Commands class accepts the correct commands.
    /// </summary>
    [Fact]
    public void Commands_ShouldHoldCorrectAcceptedCommands()
    {
      // Arrange
      var approvedCommands = new List<string>
      {
        "help",
        "generate-wallet",
        "recover-wallet",
        "show-balances",
        "show-history",
        "receive",
        "send",
      };

      // Act
      // Assert
      Commands.ApprovedCommands.Count.Should().Be(approvedCommands.Count);
      Commands.ApprovedCommands.Should().BeEquivalentTo(approvedCommands);
    }
  }
}
