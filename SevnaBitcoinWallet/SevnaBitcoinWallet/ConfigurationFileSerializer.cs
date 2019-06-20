// <copyright file="ConfigurationFileSerializer.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet
{
  using System;
  using System.IO;
  using Newtonsoft.Json;

  /// <summary>
  /// The following class serializes configuration information to file.
  /// </summary>
  public class ConfigurationFileSerializer
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationFileSerializer"/> class.
    /// </summary>
    /// <param name="walletFileName">Client provided wallet file name.</param>
    /// <param name="network">Client provided network.</param>
    /// <param name="connectionType">Client provided connection type.</param>
    /// <param name="canSpendUnconfirmed">Client provided bool to allow or disallow spending of unconfirmed coins.</param>
    [JsonConstructor]
    private ConfigurationFileSerializer(
      string walletFileName,
      string network,
      string connectionType,
      string canSpendUnconfirmed)
    {
      this.WalletFileName = walletFileName;
      this.Network = network;
      this.ConnectionType = connectionType;
      this.CanSpendUnconfirmed = canSpendUnconfirmed;
    }

    /// <summary>
    /// Gets the name of the Configuration File.
    /// </summary>
    public static string ConfigurationFileName { get; } = "Configuration.ini";

    /// <summary>
    /// Gets or sets the default file name for a Bitcoin wallet.
    /// </summary>
    public string WalletFileName { get; set; }

    /// <summary>
    /// Gets or sets the default Network for the Bitcoin wallet.
    /// </summary>
    public string Network { get; set; }

    /// <summary>
    ///  Gets or sets the default connection type for the Bitcoin wallet.
    /// </summary>
    public string ConnectionType { get; set; }

    /// <summary>
    ///  Gets or sets the default value for whether unconfirmed coins can be spent.
    /// </summary>
    public string CanSpendUnconfirmed { get; set; }

    /// <summary>
    /// Serializes Configuration information.
    /// </summary>
    /// <param name="walletFileName">Client provided wallet file name.</param>
    /// <param name="network">Client provided network.</param>
    /// <param name="connectionType">Client provided connection type.</param>
    /// <param name="canSpendUnconfirmed">Client provided bool to allow or disallow spending of unconfirmed coins.</param>
    internal static void Serialize(
      string walletFileName,
      string network,
      string connectionType,
      string canSpendUnconfirmed)
    {
      var serializedContent = JsonConvert.SerializeObject(
        new ConfigurationFileSerializer(
          walletFileName,
          network,
          connectionType,
          canSpendUnconfirmed));

      File.WriteAllText(ConfigurationFileName, serializedContent);
    }

    /// <summary>
    /// Deserializes Configuration information.
    /// </summary>
    /// <returns>A ConfigurationFileSerializer.</returns>
    internal static ConfigurationFileSerializer Deserialize()
    {
      if (File.Exists(ConfigurationFileName) == false)
      {
        throw new FileNotFoundException($"Configuration file does not exist. Create {Environment.CurrentDirectory}\\{ConfigurationFileName} and try again.");
      }

      var fileContents = File.ReadAllText(ConfigurationFileName);
      return JsonConvert.DeserializeObject<ConfigurationFileSerializer>(fileContents);
    }
  }
}