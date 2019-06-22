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
  }
}
