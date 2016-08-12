// <copyright file="TestBoolCommand.cs" company="Florian Mücke">
// Copyright (c) Florian Mücke. All rights reserved.
// Licensed under the BSD license. See LICENSE file in the project root for full license information.
// </copyright>

namespace fmdev.ArgsParser.Test
{
    using System;
    using System.Collections.Generic;

    [System.ComponentModel.Description("Test command with a optional bool option")]
    public class TestBoolCommand : Command
    {
        [CommandArg(HelpText = "toggles something on")]
        public bool On { get; set; }
    }
}