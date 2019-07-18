// <copyright file="ExtensionMethodsTests.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Tests.Extensions
{
  using System;
  using FluentAssertions;
  using NBitcoin;
  using SevnaBitcoinWallet.Exceptions;
  using SevnaBitcoinWallet.Extensions;
  using Xunit;

  /// <summary>
  /// Tests the ExtensionMethods class.
  /// </summary>
  public class ExtensionMethodsTests
  {
    /// <summary>
    /// Tests the GetNetworkFromString extension method returns Main Network value.
    /// </summary>
    [Fact]
    public void GetNetworkFromString_ShouldReturnCorrectNetworkValueMain()
    {
      // Arrange
      const string networkAsString = "main";
      var network = Network.RegTest;

      // Act
      network = network.GetNetworkFromString(networkAsString);

      // Assert
      network.Should().Be(Network.Main);
    }

    /// <summary>
    /// Tests the GetNetworkFromString extension method returns RegTest Network value.
    /// </summary>
    [Fact]
    public void GetNetworkFromString_ShouldReturnCorrectNetworkValueRegTest()
    {
      // Arrange
      const string networkAsString = "RegTest";
      var network = Network.Main;

      // Act
      network = network.GetNetworkFromString(networkAsString);

      // Assert
      network.Should().Be(Network.RegTest);
    }

    /// <summary>
    /// Tests the GetNetworkFromString extension method returns TestNet Network value.
    /// </summary>
    [Fact]
    public void GetNetworkFromString_ShouldReturnCorrectNetworkValueTestNet()
    {
      // Arrange
      const string networkAsString = "TestNet";
      var network = Network.Main;

      // Act
      network = network.GetNetworkFromString(networkAsString);

      // Assert
      network.Should().Be(Network.TestNet);
    }

    /// <summary>
    /// Tests the GetNetworkFromString extension method returns TestNet Network value.
    /// </summary>
    [Fact]
    public void GetNetworkFromString_ShouldThrowExceptionForUnknownNetwork()
    {
      // Arrange
      const string networkAsString = "Fake";
      var network = Network.Main;

      try
      {
        // Act
        network.GetNetworkFromString(networkAsString);

        // Should not get here - Force a fail if we do
        network.Should().BeNull();
      }
      catch (NetworkNoMatchException ex)
      {
        // Assert
        ex.Message.Should().Contain(networkAsString);
      }
      catch (Exception ex)
      {
        // Should not get here.
        ex.Should().BeNull();
      }
    }

    /// <summary>
    /// Tests the GetConnectionTypeFromString extension method returns ConnectionType.FullNode when given
    /// "fullnode".
    /// </summary>
    [Fact]
    public void GetConnectionTypeFromString_ShouldReturnFullNodeWhenGivenCorrespondingString()
    {
      // Assign
      var testConnectionType = ConnectionType.Http;
      const string connectionTypeAsString = "fullnode";

      // Act
      testConnectionType = testConnectionType.GetConnectionTypeFromString(connectionTypeAsString);

      // Assert
      testConnectionType.Should().BeEquivalentTo(ConnectionType.FullNode);
    }

    /// <summary>
    /// Tests the GetConnectionTypeFromString extension method returns ConnectionType.Http when given
    /// "http".
    /// </summary>
    [Fact]
    public void GetConnectionTypeFromString_ShouldReturnHttpWhenGivenCorrespondingString()
    {
      // Assign
      var testConnectionType = ConnectionType.FullNode;
      const string connectionTypeAsString = "http";

      // Act
      testConnectionType = testConnectionType.GetConnectionTypeFromString(connectionTypeAsString);

      // Assert
      testConnectionType.Should().BeEquivalentTo(ConnectionType.Http);
    }

    /// <summary>
    /// Tests the GetConnectionTypeFromString extension method throws ConnectionTypeNoMatchException
    /// when unknown ConnectionType passed.
    /// </summary>
    [Fact]
    public void GetConnectionTypeFromString_ShouldThrowCustomExceptionWhenUnknownConnectionTypeProvided()
    {
      // Assign
      var testConnectionType = ConnectionType.FullNode;
      const string connectionTypeAsString = "unknown";

      // Act
      try
      {
        testConnectionType = testConnectionType.GetConnectionTypeFromString(connectionTypeAsString);

        // Should not get here - Force a fail if we do
        testConnectionType.Should().BeNull();
      }
      catch (ConnectionTypeNoMatchException ex)
      {
        ex.Message.Should().Contain("No match found for provided string representation of ConnectionType:");
      }
    }
  }
}
