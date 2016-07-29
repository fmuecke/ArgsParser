using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FluentAssertions;

namespace FMDev.ArgsParser.Test
{
    [TestClass]
    public class ArgsParserTest
    {
        [TestMethod]
        public void BasicParserTest()
        {
            var p = new ArgsParser(new List<Command>() { new TestCommand(), new OtherTestCommand() });
            p.Result.Should().BeNull();
            p.Parse(new string[] { "Test" }).Should().BeFalse();

            p.Parse(new string[] { "Test", "-RequiredStringParam" }).Should().BeFalse();
            p.Result.Should().BeNull();

            p.Parse(new string[] { "Test", "-RequiredStringParam", "hodor" }).Should().BeTrue();
            p.Result.Should().NotBeNull();
            p.Result.Should().BeOfType<TestCommand>();

            var cmd = p.Result as TestCommand;
            cmd.RequiredStringParam.ShouldBeEquivalentTo("hodor");
            cmd.OptionalStringParam.Should().BeNullOrWhiteSpace();
            cmd.Switch.Should().BeFalse();
        }

        [TestMethod]
        public void BoolArgTest()
        {
            var p = new ArgsParser(new List<Command> { new TestBoolCommand() });
            p.Parse(new string[] { "TestBool", "-On" }).Should().BeTrue("this is a valid command line");
            var cmd = p.Result as TestBoolCommand;
            cmd.On.Should().BeTrue("the option is set");

            p.Parse(new string[] { "TestBool" }).Should().BeTrue("this is a valid command line");
            cmd = p.Result as TestBoolCommand;
            cmd.On.Should().BeFalse("the bool option is actually not set");
        }

        [TestMethod]
        public void RequiredBoolArgTest()
        {
            var p = new ArgsParser(new List<Command> { new TestRequiredBoolCommand() });
            p.Parse(new string[] { "TestRequiredBool", "-On" }).Should().BeTrue("this is a valid command line");
            var cmd = p.Result as TestRequiredBoolCommand;
            cmd.On.Should().BeTrue("the bool option is actually set");

            p.Parse(new string[] { "TestRequiredBool" }).Should().BeFalse("a required bool param is missing");
        }
    }

    [System.ComponentModel.Description("Test command")]
    public class TestCommand : Command
    {
        [CommandArg(HelpText = "toggles something on and off")]
        public bool Switch { get; set; }

        [CommandArg(HelpText = "foo bar blub blub", IsRequired = true)]
        public string RequiredStringParam { get; set; }

        [CommandArg(HelpText = "some things are iptional")]
        public string OptionalStringParam { get; set; }
    }

    [System.ComponentModel.Description("Test command with a optional bool option")]
    public class TestBoolCommand : Command
    {
        [CommandArg(HelpText = "toggles something on")]
        public bool On { get; set; }
    }

    [System.ComponentModel.Description("Test command with a required bool option")]
    public class TestRequiredBoolCommand : Command
    {
        [CommandArg(HelpText = "toggles something on", IsRequired = true)]
        public bool On { get; set; }
    }

    [System.ComponentModel.Description("Other test command")]
    public class OtherTestCommand : Command
    {
        [CommandArg(HelpText = "dispay additional information")]
        public bool? Verbose { get; set; }

        [CommandArg(HelpText = "foo bar blub blub", IsRequired = true)]
        public string RequiredStringParam { get; set; }

        [CommandArg(HelpText = "some things are iptional")]
        public string OptionalStringParam { get; set; }
    }
}