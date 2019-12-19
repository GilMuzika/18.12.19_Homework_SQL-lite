using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18._12._19_Homework_SQL_lite
{
    class ComboItem<T>
    {
        public T Value { get; set; } = default(T);

        public ComboItem(T value)
        {
            Value = value;
        }
        public override string ToString()
        {
            return Value.GetType().GetProperty("NAME").GetValue(Value).ToString();
        }
    }
}
