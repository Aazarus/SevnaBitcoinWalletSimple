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
      const string fileName = "Serialize_ShouldCreateADefaultJsonFileWhenNoParametersSent.json";
      var filePath = Path.Combine(Environment.CurrentDirectory, fileName);

      if (File.Exists(filePath))
      {
        File.Delete(filePath);
      }

      File.Exists(filePath).Should().BeFalse();

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
        testCanSpendUnconfirmed.ToString(),
        filePath);

      // Assert
      File.Exists(filePath).Should().BeTrue();
      File.ReadAllText(filePath).Should().Be(expectedFileContents);

      // Clean up
      File.Delete(filePath);
    }

    /// <summary>
    /// Tests the Deserialize method deserializes the wallet information from JSON.
    /// </summary>
    [Fact]
    public void Deserialize_ShouldReturnAConfigurationFileSerializerObjectForExistingConfigurationFile()
    {
      // Arrange
      const string fileName = "Deserialize_ShouldReturn_ForExistingConfigurationFile.json";
      var filePath = Path.Combine(Environment.CurrentDirectory, fileName);

      const string walletFileName = "TestWallet.json";
      var testNetwork = Network.Main;
      const ConnectionType testConnectionType = ConnectionType.Http;
      const bool testCanSpendUnconfirmed = false;
      const string expectedFileContents = "{\"WalletFileName\":\"TestWallet.json\",\"Network\":\"Main\",\"ConnectionType\":\"Http\",\"CanSpendUnconfirmed\":\"False\"}";

      if (File.Exists(filePath))
      {
        File.Delete(filePath);
      }

      File.WriteAllText(filePath, expectedFileContents);
      File.Exists(filePath).Should().BeTrue();

      // Act
      var result = ConfigurationFileSerializer.Deserialize(filePath);

      // Assert
      result.Should().NotBeNull();
      result.WalletFileName.Should().Be(walletFileName);
      result.Network.Should().Be(testNetwork.ToString());
      result.ConnectionType.Should().Be(testConnectionType.ToString());
      result.CanSpendUnconfirmed.Should().Be(testCanSpendUnconfirmed.ToString());

      // Clean up
      File.Delete(filePath);
    }

    /// <summary>
    /// Tests the Deserialize method thrown a FileNotFoundException if configuration file does not exist.
    /// </summary>
    [Fact]
    public void Deserialize_ShouldThrowAnExceptionWhenFileDoesNotExist()
    {
      // Arrange
      const string fileName = "Deserialize_ShouldThrowAnExceptionWhenFileDoesNotExist.json";
      var filePath = Path.Combine(Environment.CurrentDirectory, fileName);

      if (File.Exists(filePath))
      {
        File.Delete(filePath);
      }

      File.Exists(filePath).Should().BeFalse();

      // Act
      try
      {
        ConfigurationFileSerializer.Deserialize(filePath);

        // Should not get here.
        File.Exists(filePath).Should().BeFalse();
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
