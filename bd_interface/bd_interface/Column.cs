using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bd_interface
{
    internal abstract class Column
    {
        public string Name { get; set; }
        public virtual string Type { get; } = "";
        public Column(string name)
        {
            Name = name;
        }
        public abstract bool Validate(string value);

    }
    internal class IntColumn : Column
    {
        public override string Type { get; } = "INT";
        public IntColumn(string name) : base(name) { }

        public override bool Validate(string value) => int.TryParse(value, out _);
    }
    internal class RealColumn : Column
    {
        public override string Type { get; } = "REAL";
        public RealColumn(string name) : base(name) { }

        public override bool Validate(string value) => double.TryParse(value, out _);
    }
    internal class CharColumn : Column
    {
        public override string Type { get; } = "CHAR";
        public CharColumn(string name) : base(name) { }

        public override bool Validate(string value) => char.TryParse(value, out _);
    }
    internal class StringColumn : Column
    {
        public override string Type { get; } = "STRING";
        public StringColumn(string name) : base(name) { }

        public override bool Validate(string value) => true;
    }

    internal class TimeColumn : Column
    {
        public override string Type { get; } = "TIME";
        public TimeColumn(string name) : base(name) { }

        public override bool Validate(string value) => DateTime.TryParse(value, out _); 
    }
    internal class IntIntervalColumn : Column
    {
        public override string Type { get; } = "INT INTERVAL";
        public IntIntervalColumn(string name) : base(name) { }

        public override bool Validate(string value)
        {
            string[] buf = value.Replace(" ", "").Split(',');

            return buf.Length == 2 && int.TryParse(buf[0], out int a) &&
              int.TryParse(buf[1], out int b) && a < b;
        }
    }


}
