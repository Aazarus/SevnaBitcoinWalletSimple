// <copyright file="Configuration.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet
{
  using System.IO;
  using NBitcoin;
  using SevnaBitcoinWallet.Extensions;

  /// <summary>
  /// The following class creates a Configuration file which stores global settings.
  /// </summary>
  public static class Configuration
  {
    /// <summary>
    /// Initializes static members of the <see cref="Configuration"/> class.
    /// </summary>
    static Configuration()
    {
    }

    /// <summary>
    /// Gets the default file name for a Bitcoin wallet.
    /// </summary>
    public static string DefaultWalletFileName { get; private set; } = @"BitcoinWallet.json";

    /// <summary>
    /// Gets the default Network for the Bitcoin wallet.
    /// </summary>
    public static Network DefaultNetwork { get; private set; } = Network.Main;

    /// <summary>
    /// Gets the default connection type for the Bitcoin wallet.
    /// </summary>
    public static ConnectionType DefaultConnectionType { get; private set; } = ConnectionType.Http;

    /// <summary>
    /// Gets a value indicating whether unconfirmed coins can be spent.
    /// </summary>
    public static bool CanSpendUnconfirmed { get; private set; }

    /// <summary>
    /// Saves the Configuration information to file.
    /// </summary>
    /// <param name="configurationFileName">Client provided configuration file name.</param>
    public static void Save(string configurationFileName = "")
    {
      ConfigurationFileSerializer.Serialize(
        DefaultWalletFileName,
        DefaultNetwork.ToString(),
        DefaultConnectionType.ToString(),
        CanSpendUnconfirmed.ToString(),
        configurationFileName);
    }

    /// <summary>
    /// Loads the Configuration information from file.
    /// </summary>
    /// <param name="filePath">Path to file.</param>
    public static void Load(string filePath = "")
    {
      try
      {
        var configurationFileContents = ConfigurationFileSerializer.Deserialize(filePath);

        DefaultWalletFileName = configurationFileContents.WalletFileName;
        DefaultNetwork = DefaultNetwork.GetNetworkFromString(configurationFileContents.Network);
        DefaultConnectionType =
          DefaultConnectionType.GetConnectionTypeFromString(configurationFileContents.ConnectionType);
        CanSpendUnconfirmed = bool.Parse(configurationFileContents.CanSpendUnconfirmed);
      }
      catch (FileNotFoundException)
      {
        // ToDo: Log
      }
    }
  }
}
