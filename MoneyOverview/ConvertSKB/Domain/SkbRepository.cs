﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertSKB.Domain
{
    public class SkbRepository
    {
        List<AccountLine> lines = new List<AccountLine>();
        internal void Add(AccountLine line)
        {
            lines.Add(line);
        }

        internal List<AccountLine> GetAll()
        {
            return new List<AccountLine>(lines);
        }

        internal void Remove(AccountLine item)
        {
            lines.Remove(item);
        }
    }
}
