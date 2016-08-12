// <copyright file="TestCommand.cs" company="Florian Mücke">
// Copyright (c) Florian Mücke. All rights reserved.
// Licensed under the BSD license. See LICENSE file in the project root for full license information.
// </copyright>

namespace fmdev.ArgsParser.Test
{
    using System;
    using System.Collections.Generic;

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
}