// <copyright file="ConfigurationFileSerializerTests.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Tests
{
  using System;
  using System.IO;
  using FluentAssertions;
  using NBitcoin;
  using Xunit;
  using Xunit.Abstractions;

  /// <summary>
  /// Tests the ConfigurationFileSerializer class.
  /// </summary>
  public class ConfigurationFileSerializerTests
  {
    /// <summary>
    /// Tests the Serialize method serializes to JSON the wallet information.
    /// </summary>
    [Fact]
    public void Serialize_ShouldCreateADefaultJsonFileWhenNoParametersSent()
    {
      // Arrange
      var currentDirectory = Environment.CurrentDirectory;

      if (File.Exists($"{currentDirectory}\\{ConfigurationFileSerializer.ConfigurationFileName}"))
      {
        File.Delete($"{currentDirectory}\\{ConfigurationFileSerializer.ConfigurationFileName}");
      }

      File.Exists($"{currentDirectory}\\{ConfigurationFileSerializer.ConfigurationFileName}").Should().BeFalse();

      const string walletFileName = "TestWallet.json";
      var testNetwork = Network.Main;
      const ConnectionType testConnectionType = ConnectionType.Http;
      const bool testCanSpendUnconfirmed = false;
      const string expectedFileContents = "{\"WalletFileName\":\"TestWallet.json\",\"Network\":\"Main\",\"ConnectionType\":\"Http\",\"CanSpendUnconfirmed\":\"False\"}";

      // Act
      ConfigurationFileSerializer.Serialize(
        walletFileName,
        testNetwork.ToString(),
        testConnectionType.ToString(),
        testCanSpendUnconfirmed.ToString());

      // Assert
      File.Exists($"{currentDirectory}\\{ConfigurationFileSerializer.ConfigurationFileName}").Should().BeTrue();
      File.ReadAllText($"{currentDirectory}\\{ConfigurationFileSerializer.ConfigurationFileName}").Should().Be(expectedFileContents);

      // Clean up
      File.Delete($"{currentDirectory}\\{ConfigurationFileSerializer.ConfigurationFileName}");
    }

    /// <summary>
    /// Tests the Deserialize method deserializes the wallet information from JSON.
    /// </summary>
    [Fact]
    public void Deserialize_ShouldReturnAConfigurationFileSerializerObjectForExistingConfigurationFile()
    {
      // Arrange
      const string walletFileName = "TestWallet.json";
      var testNetwork = Network.Main;
      const ConnectionType testConnectionType = ConnectionType.Http;
      const bool testCanSpendUnconfirmed = false;
      const string expectedFileContents = "{\"WalletFileName\":\"TestWallet.json\",\"Network\":\"Main\",\"ConnectionType\":\"Http\",\"CanSpendUnconfirmed\":\"False\"}";
      var currentDirectory = Environment.CurrentDirectory;

      if (File.Exists($"{currentDirectory}\\{ConfigurationFileSerializer.ConfigurationFileName}") == false)
      {
        File.WriteAllText($"{currentDirectory}\\{ConfigurationFileSerializer.ConfigurationFileName}", expectedFileContents);
      }

      File.Exists($"{currentDirectory}\\{ConfigurationFileSerializer.ConfigurationFileName}").Should().BeTrue();

      // Act
      var result = ConfigurationFileSerializer.Deserialize();

      // Assert
      result.Should().NotBeNull();
      result.WalletFileName.Should().Be(walletFileName);
      result.Network.Should().Be(testNetwork.ToString());
      result.ConnectionType.Should().Be(testConnectionType.ToString());
      result.CanSpendUnconfirmed.Should().Be(testCanSpendUnconfirmed.ToString());

      // Clean up
      File.Delete($"{currentDirectory}\\{ConfigurationFileSerializer.ConfigurationFileName}");
    }

    /// <summary>
    /// Tests the Deserialize method thrown a FileNotFoundException if configuration file does not exist.
    /// </summary>
    [Fact]
    public void Deserialize_ShouldThrowAnExceptionWhenFileDoesNotExist()
    {
      // Arrange
      var currentDirectory = Environment.CurrentDirectory;

      if (File.Exists($"{currentDirectory}\\{ConfigurationFileSerializer.ConfigurationFileName}"))
      {
        File.Delete($"{currentDirectory}\\{ConfigurationFileSerializer.ConfigurationFileName}");
      }

      File.Exists($"{currentDirectory}\\{ConfigurationFileSerializer.ConfigurationFileName}").Should().BeFalse();

      // Act
      try
      {
        var result = ConfigurationFileSerializer.Deserialize();

        // Should not get here.
        File.Exists($"{currentDirectory}\\{ConfigurationFileSerializer.ConfigurationFileName}").Should().BeFalse();
      }
      catch (FileNotFoundException ex)
      {
        // Assert
        ex.Message.Should().Contain("Configuration file does not exist. Create ");
      }
      catch (Exception ex)
      {
        // Something else went wrong
        ex.Should().BeNull();
      }
    }

  }
}
