// <copyright file="CommandsTests.cs" company="Sevna Software LTD">
// Copyright (c) Sevna Software LTD. All rights reserved.
// </copyright>

namespace SevnaBitcoinWallet.Tests.Commands
{
  using System;
  using System.Collections.Generic;
  using FluentAssertions;
  using SevnaBitcoinWallet.Commands;
  using SevnaBitcoinWallet.Exceptions;
  using Xunit;

  /// <summary>
  /// Tests the CommandManager class.
  /// </summary>
  /// ToDo: Add tests to throw exception if a commands expected arguments are not present.
  public class CommandsTests
  {
    /// <summary>
    /// Tests the CommandManager class accepts the correct commands.
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
      CommandManager.ApprovedCommandSet.Count.Should().Be(approvedCommands.Count);
      CommandManager.ApprovedCommandSet.Should().BeEquivalentTo(approvedCommands);
    }

    /// <summary>
    /// Tests FindMatchingCommandWithArguments correctly matches client provided command, generate-wallet, and
    /// provides command with arguments.
    /// </summary>
    [Fact]
    public void FindMatchingCommandWithArguments_ShouldMatchGenerateWalletCorrectly()
    {
      // Arrange
      const string command = "generate-wallet";
      var commands = new List<string>
      {
        command,
        "wallet-file=test2.json",
        "help",
        "show-balances",
        "wallet-file=test2.json",
        "send", "btc=1.00", "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4", "wallet-file=test2.json",
      };

      var expected = new List<string>
      {
        command,
        "wallet-file=test2.json",
      };

      // Act
      var commandWithArgs = CommandManager.FindMatchingCommandWithArguments(command, commands);

      // Assert
      commandWithArgs.Should().Equal(expected);
    }

    /// <summary>
    /// Tests FindMatchingCommandWithArguments throws InvalidCommandArgumentFound if element does not contain expected argument for generate-wallet command.
    /// </summary>
    [Fact]
    public void FindMatchingCommandWithArguments_ShouldThrowCustomExceptionIfGenerateWalletArgumentNotPresent()
    {
      // Arrange
      const string command = "generate-wallet";
      var commands = new List<string>
      {
        command,
        "help", // <-- Should be argument for command e.g. "wallet-file=test2.json"
        "show-balances",
        "wallet-file=test2.json",
        "send", "btc=1.00", "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4", "wallet-file=test2.json",
      };

      // Act
      try
      {
        var commandWithArgs = CommandManager.FindMatchingCommandWithArguments(command, commands);
        commandWithArgs.Should().BeNull();
      }
      catch (InvalidCommandArgumentFound ex)
      {
        ex.Message.Should().Contain("Argument should be 'wallet-file=$NameOfWalletFile' actual is:");
      }
      catch (Exception ex)
      {
        // Should not get here.
        ex.Should().BeNull();
      }
    }

    /// <summary>
    /// Tests FindMatchingCommandWithArguments correctly matches client provided command, help, and
    /// provides command with arguments, which in this case is not arguments.
    /// </summary>
    [Fact]
    public void FindMatchingCommandWithArguments_ShouldMatchHelpCorrectly()
    {
      // Arrange
      const string command = "help";
      var commands = new List<string>
      {
        "help",
        "show-balances",
        "wallet-file=test2.json",
        "send", "btc=1.00", "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4", "wallet-file=test2.json",
      };

      var expected = new List<string>
      {
        "help",
      };

      // Act
      var commandWithArgs = CommandManager.FindMatchingCommandWithArguments(command, commands);

      // Assert
      commandWithArgs.Should().Equal(expected);
    }

    /// <summary>
    /// Tests FindMatchingCommandWithArguments correctly matches client provided command, receive, and
    /// provides command with arguments.
    /// </summary>
    [Fact]
    public void FindMatchingCommandWithArguments_ShouldMatchReceiveCorrectly()
    {
      // Arrange
      const string command = "receive";
      var commands = new List<string>
      {
        command,
        "wallet-file=test1.json",
        "generate-wallet",
        "wallet-file=test2.json",
        "help",
        "show-balances",
        "wallet-file=test2.json",
        "send", "btc=1.00", "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4", "wallet-file=test2.json",
      };

      var expected = new List<string>
      {
        command,
        "wallet-file=test1.json",
      };

      // Act
      var commandWithArgs = CommandManager.FindMatchingCommandWithArguments(command, commands);

      // Assert
      commandWithArgs.Should().Equal(expected);
    }

    /// <summary>
    /// Tests FindMatchingCommandWithArguments throws InvalidCommandArgumentFound if element does not contain expected argument for receive command.
    /// </summary>
    [Fact]
    public void FindMatchingCommandWithArguments_ShouldThrowCustomExceptionIfReceiveArgumentNotPresent()
    {
      // Arrange
      const string command = "receive";
      var commands = new List<string>
      {
        command,
        "generate-wallet", // <-- Should be argument for command e.g. "wallet-file=test1.json"
        "wallet-file=test2.json",
        "help",
        "show-balances",
        "wallet-file=test2.json",
        "send", "btc=1.00", "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4", "wallet-file=test2.json",
      };

      // Act
      try
      {
        var commandWithArgs = CommandManager.FindMatchingCommandWithArguments(command, commands);
        commandWithArgs.Should().BeNull();
      }
      catch (InvalidCommandArgumentFound ex)
      {
        ex.Message.Should().Contain("Argument should be 'wallet-file=$NameOfWalletFile' actual is:");
      }
      catch (Exception ex)
      {
        // Should not get here.
        ex.Should().BeNull();
      }
    }

    /// <summary>
    /// Tests FindMatchingCommandWithArguments correctly matches client provided command, recover-wallet, and
    /// provides command with arguments.
    /// </summary>
    [Fact]
    public void FindMatchingCommandWithArguments_ShouldMatchRecoverWalletCorrectly()
    {
      // Arrange
      const string command = "recover-wallet";
      var commands = new List<string>
      {
        command,
        "wallet-file=test.json",
        "receive",
        "wallet-file=test1.json",
        "generate-wallet",
        "wallet-file=test2.json",
        "help",
        "show-balances",
        "wallet-file=test2.json",
        "send", "btc=1.00", "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4", "wallet-file=test2.json",
      };

      var expected = new List<string>
      {
        command,
        "wallet-file=test.json",
      };

      // Act
      var commandWithArgs = CommandManager.FindMatchingCommandWithArguments(command, commands);

      // Assert
      commandWithArgs.Should().Equal(expected);
    }

    /// <summary>
    /// Tests FindMatchingCommandWithArguments throws InvalidCommandArgumentFound if element does not contain expected argument for receive command.
    /// </summary>
    [Fact]
    public void FindMatchingCommandWithArguments_ShouldThrowCustomExceptionIfRecoverWalletArgumentNotPresent()
    {
      // Arrange
      const string command = "recover-wallet";
      var commands = new List<string>
      {
        command,
        "receive",  // <-- Should be argument for command e.g. "wallet-file=test.json"
        "wallet-file=test1.json",
        "generate-wallet",
        "wallet-file=test2.json",
        "help",
        "show-balances",
        "wallet-file=test2.json",
        "send", "btc=1.00", "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4", "wallet-file=test2.json",
      };

      // Act
      try
      {
        var commandWithArgs = CommandManager.FindMatchingCommandWithArguments(command, commands);
        commandWithArgs.Should().BeNull();
      }
      catch (InvalidCommandArgumentFound ex)
      {
        ex.Message.Should().Contain("Argument should be 'wallet-file=$NameOfWalletFile' actual is:");
      }
      catch (Exception ex)
      {
        // Should not get here.
        ex.Should().BeNull();
      }
    }

    /// <summary>
    /// Tests FindMatchingCommandWithArguments correctly matches client provided command, send, and
    /// provides command with arguments.
    /// </summary>
    [Fact]
    public void FindMatchingCommandWithArguments_ShouldMatchSendCorrectly()
    {
      // Arrange
      const string command = "send";
      var commands = new List<string>
      {
        command,
        "btc=1.00",
        "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4",
        "wallet-file=test2.json",
        "recover-wallet",
        "wallet-file=test.json",
        "receive",
        "wallet-file=test1.json",
        "generate-wallet",
        "wallet-file=test2.json",
        "help",
        "show-balances",
        "wallet-file=test2.json",
        "send", "btc=1.00", "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4", "wallet-file=test2.json",
      };

      var expected = new List<string>
      {
        command,
        "btc=1.00",
        "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4",
        "wallet-file=test2.json",
      };

      // Act
      var commandWithArgs = CommandManager.FindMatchingCommandWithArguments(command, commands);

      // Assert
      commandWithArgs.Should().Equal(expected);
    }

    /// <summary>
    /// Tests FindMatchingCommandWithArguments throws InvalidCommandArgumentFound if element does not contain expected first argument for send command.
    /// </summary>
    [Fact]
    public void FindMatchingCommandWithArguments_ShouldThrowCustomExceptionIfSendFirstArgumentNotPresent()
    {
      // Arrange
      const string command = "send";
      var commands = new List<string>
      {
        command,
        "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4",  // <-- Should be argument for command e.g. "btc=1.00"
        "wallet-file=test2.json",
        "recover-wallet",
        "wallet-file=test.json",
        "receive",
        "wallet-file=test1.json",
        "generate-wallet",
        "wallet-file=test2.json",
        "help",
        "show-balances",
        "wallet-file=test2.json",
        "send", "btc=1.00", "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4", "wallet-file=test2.json",
      };

      // Act
      try
      {
        var commandWithArgs = CommandManager.FindMatchingCommandWithArguments(command, commands);
        commandWithArgs.Should().BeNull();
      }
      catch (InvalidCommandArgumentFound ex)
      {
        ex.Message.Should().Contain("Argument should be 'btc=$AmountOfBitcoin' actual is:");
      }
      catch (Exception ex)
      {
        // Should not get here.
        ex.Should().BeNull();
      }
    }

    /// <summary>
    /// Tests FindMatchingCommandWithArguments throws InvalidCommandArgumentFound if element does not contain expected second argument for send command.
    /// </summary>
    [Fact]
    public void FindMatchingCommandWithArguments_ShouldThrowCustomExceptionIfSendSecondArgumentNotPresent()
    {
      // Arrange
      const string command = "send";
      var commands = new List<string>
      {
        command,
        "btc=1.00",
        "wallet-file=test2.json", // <-- Should be argument for command e.g. "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4"
        "recover-wallet",
        "wallet-file=test.json",
        "receive",
        "wallet-file=test1.json",
        "generate-wallet",
        "wallet-file=test2.json",
        "help",
        "show-balances",
        "wallet-file=test2.json",
        "send", "btc=1.00", "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4", "wallet-file=test2.json",
      };

      // Act
      try
      {
        var commandWithArgs = CommandManager.FindMatchingCommandWithArguments(command, commands);
        commandWithArgs.Should().BeNull();
      }
      catch (InvalidCommandArgumentFound ex)
      {
        ex.Message.Should().Contain("Argument should be 'address=$BitcoinAddress' actual is:");
      }
      catch (Exception ex)
      {
        // Should not get here.
        ex.Should().BeNull();
      }
    }

    /// <summary>
    /// Tests FindMatchingCommandWithArguments correctly matches client provided command, show-balances, and
    /// provides command with arguments.
    /// </summary>
    [Fact]
    public void FindMatchingCommandWithArguments_ShouldMatchShowBalancesCorrectly()
    {
      // Arrange
      const string command = "show-balances";
      var commands = new List<string>
      {
        command,
        "wallet-file=test2.json",
        "send",
        "btc=1.00",
        "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4",
        "wallet-file=test2.json",
        "recover-wallet",
        "wallet-file=test.json",
        "receive",
        "wallet-file=test1.json",
        "generate-wallet",
        "wallet-file=test2.json",
        "help",
        "send", "btc=1.00", "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4", "wallet-file=test2.json",
      };

      var expected = new List<string>
      {
        command,
        "wallet-file=test2.json",
      };

      // Act
      var commandWithArgs = CommandManager.FindMatchingCommandWithArguments(command, commands);

      // Assert
      commandWithArgs.Should().Equal(expected);
    }

    /// <summary>
    /// Tests FindMatchingCommandWithArguments throws InvalidCommandArgumentFound if element does not contain expected argument for show-balances command.
    /// </summary>
    [Fact]
    public void FindMatchingCommandWithArguments_ShouldThrowCustomExceptionIfShowBalancesArgumentNotPresent()
    {
      // Arrange
      const string command = "show-balances";
      var commands = new List<string>
      {
        command,
        "send", // <-- Should be argument for command e.g. "wallet-file=test2.json"
        "btc=1.00",
        "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4",
        "wallet-file=test2.json",
        "recover-wallet",
        "wallet-file=test.json",
        "receive",
        "wallet-file=test1.json",
        "generate-wallet",
        "wallet-file=test2.json",
        "help",
        "send", "btc=1.00", "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4", "wallet-file=test2.json",
      };

      // Act
      try
      {
        var commandWithArgs = CommandManager.FindMatchingCommandWithArguments(command, commands);
        commandWithArgs.Should().BeNull();
      }
      catch (InvalidCommandArgumentFound ex)
      {
        ex.Message.Should().Contain("Argument should be 'wallet-file=$NameOfWalletFile' actual is:");
      }
      catch (Exception ex)
      {
        // Should not get here.
        ex.Should().BeNull();
      }
    }

    /// <summary>
    /// Tests FindMatchingCommandWithArguments correctly matches client provided command, show-history, and
    /// provides command with arguments.
    /// </summary>
    [Fact]
    public void FindMatchingCommandWithArguments_ShouldMatchShowHistoryCorrectly()
    {
      // Arrange
      const string command = "show-history";
      var commands = new List<string>
      {
        command,
        "wallet-file=test.json",
        "send",
        "btc=1.00",
        "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4",
        "wallet-file=test2.json",
        "recover-wallet",
        "wallet-file=test.json",
        "receive",
        "wallet-file=test1.json",
        "generate-wallet",
        "wallet-file=test2.json",
        "help",
        "send", "btc=1.00", "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4", "wallet-file=test2.json",
      };

      var expected = new List<string>
      {
        command,
        commands[1],
      };

      // Act
      var commandWithArgs = CommandManager.FindMatchingCommandWithArguments(command, commands);

      // Assert
      commandWithArgs.Should().Equal(expected);
    }

    /// <summary>
    /// Tests FindMatchingCommandWithArguments throws InvalidCommandArgumentFound if element does not contain expected argument for show-history command.
    /// </summary>
    [Fact]
    public void FindMatchingCommandWithArguments_ShouldThrowCustomExceptionIfShowHistoryArgumentNotPresent()
    {
      // Arrange
      const string command = "show-history";
      var commands = new List<string>
      {
        command,
        "send", // <-- Should be argument for command e.g. "wallet-file=test.json"
        "btc=1.00",
        "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4",
        "wallet-file=test2.json",
        "recover-wallet",
        "wallet-file=test.json",
        "receive",
        "wallet-file=test1.json",
        "generate-wallet",
        "wallet-file=test2.json",
        "help",
        "send", "btc=1.00", "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4", "wallet-file=test2.json",
      };

      // Act
      try
      {
        var commandWithArgs = CommandManager.FindMatchingCommandWithArguments(command, commands);
        commandWithArgs.Should().BeNull();
      }
      catch (InvalidCommandArgumentFound ex)
      {
        ex.Message.Should().Contain("Argument should be 'wallet-file=$NameOfWalletFile' actual is:");
      }
      catch (Exception ex)
      {
        // Should not get here.
        ex.Should().BeNull();
      }
    }

    /// <summary>
    /// Tests FindMatchingCommandWithArguments throws CommandNotFoundException for unknown command.
    /// </summary>
    [Fact]
    public void FindMatchingCommandWithArguments_ShouldThrowCustomExceptionForUnknownCommand()
    {
      // Arrange
      const string command = "Unknown-command";
      var commands = new List<string>
      {
        "help",
        "show-balances",
        "wallet-file=test2.json",
        "send", "btc=1.00", "address=mq6fK8fkFyCy9p53m4GG4fiE2XCKvcwgi4", "wallet-file=test2.json",
      };

      // Act
      try
      {
        var commandWithArgs = CommandManager.FindMatchingCommandWithArguments(command, commands);
      }
      catch (CommandNotFoundException ex)
      {
        ex.Message.Should().Contain("Unknown command provided:");
      }
      catch (Exception ex)
      {
        // Should not get here.
        ex.Should().BeNull();
      }
    }
  }
}
