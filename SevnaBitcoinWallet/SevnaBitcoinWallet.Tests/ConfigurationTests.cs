// <copyright file="ConfigurationTests.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Tests
{
  using System;
  using System.IO;
    using System.Threading;
    using FluentAssertions;
  using NBitcoin;
  using Xunit;

  /// <summary>
  /// Tests the Configuration class.
  /// </summary>
  public class ConfigurationTests
  {
    /// <summary>
    /// Tests the Save method correctly saves the information to file.
    /// </summary>
    [Fact]
    public void Save_CorrectlySerializesInformationToFile()
    {
      Thread.Sleep(1000); // This delay is to avoid conflicts with the file access. Needs reviewing and improving.

      // Arrange
      var fileName = $"Save_CorrectlySerializesInformationToFile{DateTime.Now:HH-mm-ss}.json";
      var filePath = Path.Combine(Environment.CurrentDirectory, fileName);

      // Act
      try
      {
        Configuration.Save(filePath);
      }
      catch (IOException ex)
      {
        // Default to a failing test, this should not happen.
        ex.Should().BeNull();
      }

      // Assert
      File.Exists(filePath).Should().BeTrue();

      var results = File.ReadAllText(filePath);
      results.Should().Be("{\"WalletFileName\":\"BitcoinWallet.json\",\"Network\":\"Main\",\"ConnectionType\":\"Http\",\"CanSpendUnconfirmed\":\"False\"}");

      // Clean up
      File.Delete(filePath);
    }

    /// <summary>
    /// Tests the Load method correctly loads configuration from file when using the same hardcoded configuration values.
    /// </summary>
    [Fact]
    public void Load_CorrectlyLoadsConfigurationDataFromFileWithDefaults()
    {
      Thread.Sleep(1000); // This delay is to avoid conflicts with the file access. Needs reviewing and improving.

      // Arrange
      const string fileName = "Load_CorrectlyLoadsConfigurationDataFromFile.json";
      var filePath = Path.Combine(Environment.CurrentDirectory, fileName);

      const string walletFileName = "BitcoinWallet.json";
      var testNetwork = Network.Main;
      const ConnectionType testConnectionType = ConnectionType.Http;
      const bool testCanSpendUnconfirmed = false;
      const string expectedFileContents = "{\"WalletFileName\":\"BitcoinWallet.json\",\"Network\":\"Main\",\"ConnectionType\":\"Http\",\"CanSpendUnconfirmed\":\"False\"}";

      if (File.Exists(filePath))
      {
        File.Delete(filePath);
      }

      File.WriteAllText(filePath, expectedFileContents);

      // Act
      Configuration.Load(filePath);

      // Assert
      Configuration.DefaultWalletFileName.Should().Be(walletFileName);
      Configuration.DefaultNetwork.Should().Be(testNetwork);
      Configuration.DefaultConnectionType.Should().Be(testConnectionType);
      Configuration.CanSpendUnconfirmed.Should().Be(testCanSpendUnconfirmed);

      // Clean
      File.Delete(filePath);
    }

    /// <summary>
    /// Tests the Load method correctly loads configuration from file when using custom valid values.
    /// </summary>
    [Fact]
    public void Load_CorrectlyLoadsConfigurationDataFromFileWithCustom()
    {
      // Arrange
      const string fileName = "Load_CorrectlyLoadsConfigurationDataFromFile.json";
      var filePath = Path.Combine(Environment.CurrentDirectory, fileName);

      const string walletFileName = "TestWallet.json";
      var testNetwork = Network.RegTest;
      const ConnectionType testConnectionType = ConnectionType.FullNode;
      const bool testCanSpendUnconfirmed = true;
      const string expectedFileContents = "{\"WalletFileName\":\"TestWallet.json\",\"Network\":\"RegTest\",\"ConnectionType\":\"FullNode\",\"CanSpendUnconfirmed\":\"True\"}";

      if (File.Exists(filePath))
      {
        File.Delete(filePath);
      }

      File.WriteAllText(filePath, expectedFileContents);

      // Act
      Configuration.Load(filePath);

      // Assert
      Configuration.DefaultWalletFileName.Should().Be(walletFileName);
      Configuration.DefaultNetwork.Should().Be(testNetwork);
      Configuration.DefaultConnectionType.Should().Be(testConnectionType);
      Configuration.CanSpendUnconfirmed.Should().Be(testCanSpendUnconfirmed);

      // Clean
      File.Delete(filePath);
    }
  }
}
