﻿// <copyright file="WalletManagerTests.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Tests
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using FluentAssertions;
  using SevnaBitcoinWallet.Exceptions;
  using Xunit;

  /// <summary>
  /// Tests the WalletManager class.
  /// </summary>
  public class WalletManagerTests
  {
    private static readonly string[] Args =
    {
      "recover-wallet", "wallet-file=test1.json",
      "show-balances", "wallet-file=test1.json",
      "send", "btc=0.0001", "address=mq6fK8fkFyCy9p53m4Gf4fiX2XCHvcwgi1", "wallet-file=test1.json",
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="WalletManagerTests"/> class.
    /// </summary>
    public WalletManagerTests()
    {
      try
      {
        // Ensure Wallets directory doesn't already exist
        Directory.Delete("Wallets", true);
      }
      catch (DirectoryNotFoundException)
      {
        // This is not a problem as we are deleting the directory anyway.
      }
    }

    /// <summary>
    /// Tests the WalletManager constructor correctly adds CommandIdentifier.
    /// </summary>
    [Fact]
    public void WalletManager_ShouldEnsureConfigurationLoadOnInstantiation()
    {
      // Arrange
      // Act
      var walletManager = new WalletManager();
      walletManager.AddCommands(Args);

      // Assert
      Configuration.DefaultWalletFileName.Should().NotBeNull();
      Configuration.DefaultNetwork.Should().NotBeNull();
      Configuration.DefaultConnectionType.Should().NotBeNull();
    }

    /// <summary>
    /// Tests the WalletManager constructor correctly adds CommandIdentifier.
    /// </summary>
    [Fact]
    public void AddCommands_ShouldAddCommandsAsDefined()
    {
      // Arrange
      var walletManager = new WalletManager();

      // Act
      walletManager.AddCommands(Args);

      // Assert
      walletManager.Commands.Should().BeEquivalentTo(Args);
    }

    /// <summary>
    /// Tests the AddCommands method adds the provided commands to the end of the list.
    /// </summary>
    [Fact]
    public void AddCommands_ShouldAddCommandsToEndOfCommandsCollection()
    {
      // Arrange
      var walletManager = new WalletManager();
      walletManager.AddCommands(Args);
      var newCommand = new List<string>
      {
        "show-balances",
        "wallet-file=test2.json",
        "send", "btc=1.00", "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4", "wallet-file=test2.json",
      };

      var expectedCommands = new List<string>();
      expectedCommands.AddRange(Args);
      expectedCommands.AddRange(newCommand);

      // Act
      walletManager.AddCommands(newCommand.ToArray());
      var actualWalletManagerCommands = walletManager.Commands;

      // Assert
      actualWalletManagerCommands.Should().BeEquivalentTo(expectedCommands);
    }

    /// <summary>
    /// Tests the AddCommands method throws an CommandArgumentNullOrEmptyException when no arguments provided.
    /// </summary>
    [Fact]
    public void AddCommands_ShouldThrowExceptionForNullArguments()
    {
      // Arrange
      // Act
      try
      {
        var walletManager = new WalletManager();
        walletManager.AddCommands(null);
      }
      catch (CommandArgumentNullOrEmptyException ex)
      {
        ex.Message.Should().Contain("Argument contains null or empty values.");
      }
      catch (Exception ex)
      {
        // Should not get here
        ex.Should().BeNull();
      }
    }

    /// <summary>
    /// Tests the AddCommands method throws an CommandArgumentNullOrEmptyException when empty arguments are provided.
    /// </summary>
    [Fact]
    public void AddCommands_ShouldThrowExceptionForEmptyArguments()
    {
      // Arrange
      // Act
      try
      {
        var commands = new[]
        {
          string.Empty,
          string.Empty,
        };
        var walletManager = new WalletManager();
        walletManager.AddCommands(commands);
      }
      catch (CommandArgumentNullOrEmptyException ex)
      {
        ex.Message.Should().Contain("Argument contains null or empty values.");
      }
      catch (Exception ex)
      {
        // Should not get here
        ex.Should().BeNull();
      }
    }

    /// <summary>
    /// Tests the AddCommands method throws an CommandArgumentNullOrEmptyException when empty arguments are provided.
    /// </summary>
    [Fact]
    public void AddCommands_ShouldThrowExceptionForArgumentsWithEmptyValues()
    {
      // Arrange
      // Act
      try
      {
        var commands = new[]
        {
          "show-balances",
          string.Empty,
        };
        var walletManager = new WalletManager();
        walletManager.AddCommands(commands);
      }
      catch (CommandArgumentNullOrEmptyException ex)
      {
        ex.Message.Should().Contain("Argument contains null or empty values.");
      }
      catch (Exception ex)
      {
        // Should not get here
        ex.Should().BeNull();
      }
    }

    /// <summary>
    /// Tests the AddCommands method throws an CommandArgumentNullOrEmptyException when null arguments are provided.
    /// </summary>
    [Fact]
    public void AddCommands_ShouldThrowExceptionForArgumentsWithNullValues()
    {
      // Arrange
      // Act
      try
      {
        var commands = new[]
        {
          "show-balances",
          null,
        };
        var walletManager = new WalletManager();
        walletManager.AddCommands(commands);
      }
      catch (CommandArgumentNullOrEmptyException ex)
      {
        ex.Message.Should().Contain("Argument contains null or empty values.");
      }
      catch (Exception ex)
      {
        // Should not get here
        ex.Should().BeNull();
      }
    }

    /// <summary>
    /// Tests the ProcessNextCommand method reduces the Command collection by removing the processed command and arguments.
    /// </summary>
    [Fact]
    public void ProcessNextCommand_ShouldRemoveTheProcessedCommandFromCollectionOnceComplete()
    {
      // Arrange
      string[] commandsAndArgs =
      {
        "generate-wallet", "wallet-file=BitcoinWallet.json",
        "recover-wallet", "wallet-file=test1.json",
        "show-balances", "wallet-file=test1.json",
        "send", "btc=0.0001", "address=mq6fK8fkFyCy9p53m4Gf4fiX2XCHvcwgi1", "wallet-file=test1.json",
      };

      var walletManager = new WalletManager();
      walletManager.AddCommands(commandsAndArgs);

      var currentCommandCount = walletManager.Commands.Count;

      // Act
      var result = walletManager.ProcessNextCommand();

      // Assert
      walletManager.Commands.Count.Should().NotBe(currentCommandCount);
      walletManager.Commands.Count.Should().Be(currentCommandCount - 2);
    }

    /// <summary>
    /// Tests the ProcessNextCommand method returns the Mnemonic of a newly generated wallet.
    /// </summary>
    [Fact]
    public void ProcessNextCommand_ShouldReturnTheMnemonicForProcessingGenerateNewWalletCommand()
    {
      // Arrange

      string[] commandsAndArgs =
      {
        "generate-wallet", "wallet-file=BitcoinWallet.json",
        "recover-wallet", "wallet-file=test1.json",
        "show-balances", "wallet-file=test1.json",
        "send", "btc=0.0001", "address=mq6fK8fkFyCy9p53m4Gf4fiX2XCHvcwgi1", "wallet-file=test1.json",
      };

      var walletManager = new WalletManager();
      walletManager.AddCommands(commandsAndArgs);

      // Act
      var result = walletManager.ProcessNextCommand();

      // Assert
      result.Should().NotBeNullOrEmpty();
    }

    /// <summary>
    /// Tests the ProcessCommands method throws CommandNotFoundException when no commands are available.
    /// </summary>
    [Fact]
    public void ProcessCommands_ShouldThrowCustomExceptionWhenNoCommandsAvailable()
    {
      // Arrange
      var walletManager = new WalletManager();
      walletManager.Commands.Should().BeEmpty();

      // Act
      try
      {
        var result = walletManager.ProcessCommands();
      }
      catch (CommandNotFoundException ex)
      {
        // Assert
        ex.Message.Should().Be("No CommandIdentifier found.");
      }
      catch (Exception ex)
      {
        // Should not get here
        ex.Should().BeNull();
      }
    }

    /// <summary>
    /// Tests the ProcessCommands method throws CommandNotFoundException when no command is available.
    /// </summary>
    [Fact]
    public void ProcessCommands_ShouldThrowCustomExceptionWhenCommandNotAvailable()
    {
      // Arrange
      var walletManager = new WalletManager();

      // Act
      try
      {
        var result = walletManager.ProcessCommands();
      }
      catch (CommandNotFoundException ex)
      {
        // Assert
        ex.Message.Should().Be("No CommandIdentifier Available.");
      }
      catch (Exception ex)
      {
        // Should not get here
        ex.Should().BeNull();
      }
    }
  }
}
