// <copyright file="ExtensionMethods.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Extensions
{
  using NBitcoin;
  using SevnaBitcoinWallet.Exceptions;

  /// <summary>
  /// Defines extension methods.
  /// </summary>
  public static class ExtensionMethods
  {
    /// <summary>
    /// Matches a string to a Network type.
    /// </summary>
    /// <param name="network">Class to add extension method to.</param>
    /// <param name="parameters">The string representation of the Network.</param>
    /// <returns>A Network value.</returns>
    /// <exception cref="NetworkNoMatchException">Not matching NetworkType found.</exception>
    public static Network GetNetworkFromString(this Network network, params object[] parameters)
    {
      var networkToProcess = parameters[0].ToString();

      switch (networkToProcess.ToLower())
      {
        case "main":
          return Network.Main;

        case "regtest":
          return Network.RegTest;

        case "testnet":
          return Network.TestNet;

        default:
          throw new NetworkNoMatchException($"No match found for provided string representation of Network: {networkToProcess}");
      }
    }

    /// <summary>
    /// Matches a string to a ConnectionType.
    /// </summary>
    /// <param name="connectionType">Class to add extension method to.</param>
    /// <param name="parameters">The string representation of the ConnectionType.</param>
    /// <returns>A ConnectionType value.</returns>
    /// <exception cref="ConnectionTypeNoMatchException">No matching ConnectionType value found.</exception>
    public static ConnectionType GetConnectionTypeFromString(this ConnectionType connectionType, params object[] parameters)
    {
      var connectionTypeToProcess = parameters[0].ToString();

      switch (connectionTypeToProcess.ToLower())
      {
        case "fullnode":
          return ConnectionType.FullNode;
        case "http":
          return ConnectionType.Http;
        default:
          throw new ConnectionTypeNoMatchException($"No match found for provided string representation of ConnectionType: {connectionTypeToProcess}");
      }
    }
  }
}