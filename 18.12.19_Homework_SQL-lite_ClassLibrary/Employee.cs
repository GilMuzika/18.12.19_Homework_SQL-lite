using System;

namespace _18._12._19_Homework_SQL_lite_ClassLibrary
{

    // POCO -- plain old clr object
    public class Employee
    {
        public int ID { get; set; } = -2;
        public string NAME { get; set; } = "DEFAULT NAME";
        public string ADDRESS { get; set; } = "DEFAULT ADDRESS";
        public double SALARY { get; set; } = -10000;
        public int AGE { get; set; } = -800;

        public Employee(string nAME, string aDDRESS, double sALARY, int aGE)
        {
            NAME = nAME;
            ADDRESS = aDDRESS;
            SALARY = sALARY;
            AGE = aGE;
        }

        public Employee(int iD, string nAME, string aDDRESS, double sALARY, int aGE)
        {
            ID = iD;
            NAME = nAME;
            ADDRESS = aDDRESS;
            SALARY = sALARY;
            AGE = aGE;
        }

        public Employee() {}

        public override string ToString()
        {
            //return $"{ID} {NAME} {ADDRESS} {SALARY} {AGE}";
            string str = string.Empty;
            foreach (var s in this.GetType().GetProperties()) str += $"{s.Name}: {s.GetValue(this)}\n";
            str += "-----------------\n";
            return str;
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, null) && ReferenceEquals(obj, null)) return true;

            if (ReferenceEquals(this, null) || ReferenceEquals(obj, null)) return false;

            return this.ID == (obj as Employee).ID;
        }
        public override int GetHashCode()
        {
            return this.ID;
        }

    }
}
