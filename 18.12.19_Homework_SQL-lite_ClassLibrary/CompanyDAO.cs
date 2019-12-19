using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace _18._12._19_Homework_SQL_lite_ClassLibrary
{
    public enum DAOAcsessModifier
    {
        GetEmployeesTotalNumber = 0,
        InsertEmployee = 1,        
        DeleteEmployee = 2,
        GetOneEmployee = 3,
        GetAllEmployees = 4,
        UpdateEmployee = 5,
    }
    public static class CompanyDAO
    {
        private static string _dBpath = $"{Directory.GetCurrentDirectory()}\\_database\\Sql-lite_db.db";
        private static SQLiteConnection _connection;



        static SQLiteConnection OpenConection()
        {
            var conn = new SQLiteConnection($"Data source ={_dBpath}; Version = 3;");
            conn.Open();
            return conn;
        }
        public static bool IsInDataBase<T>(T t) where T: class
        {
            try
            {
                return IsInDataBaseInternal(t);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.GetType().Name}\n\n{ex.Message}\n\n Path to the DataBase file: \n{_dBpath}\n\n{ex.StackTrace}");
            }
        }
        public static int GetEmployeesTotalNumber(DAOAcsessModifier modifier)
        {
            try
            {
                switch (modifier)
                {
                    case DAOAcsessModifier.GetEmployeesTotalNumber:
                        return GetEmployeesTotalNumberInternal();
                }
                throw new Exception($"'DAOAcsessModifier' must be {DAOAcsessModifier.GetEmployeesTotalNumber}");
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.GetType().Name}\n\n{ex.Message}\n\n Path to the DataBase file: \n{_dBpath}\n\n{ex.StackTrace}");
            }
        }
        public static T GetData<T>(DAOAcsessModifier modifier) where T: class
        {
            try
            {
                //T returnValue;
                switch(modifier)
                {
                    case DAOAcsessModifier.GetAllEmployees:
                        return GetEmployeesInternal() as T;                        
                }
                throw new Exception($"'DAOAcsessModifier' must be {DAOAcsessModifier.GetEmployeesTotalNumber}");
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.GetType().Name}\n\n{ex.Message}\n\n Path to the DataBase file: \n{_dBpath}\n\n{ex.StackTrace}");
            }
        }
        public static void SetData<T>(T t, DAOAcsessModifier modifier)
        {
            try
            {
                switch(modifier)
                {
                    case DAOAcsessModifier.InsertEmployee:
                        AddEmployeeInternal<T>(t);
                        break;
                    case DAOAcsessModifier.DeleteEmployee:
                        DeleteEmployeeInternal<T>(t);
                        break;
                    case DAOAcsessModifier.UpdateEmployee:
                        UpdateEmployeeInternal<T>(t);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.GetType().Name}\n\n{ex.Message}\n\n Path to the DataBase file: \n{_dBpath}\n\n{ex.StackTrace}");
            }
        }


        private static List<Employee> GetEmployeesInternal()
        {


                List<Employee> results = new List<Employee>();
            
            using (_connection = OpenConection())
            {
                    using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM COMPANY", _connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                            
                                Employee e = new Employee
                                {
                                    ID = reader.GetInt32(0),
                                    NAME = reader.GetString(1),
                                    AGE = reader.GetInt32(2),
                                    ADDRESS = reader.GetString(3),
                                    SALARY = reader.GetDouble(4)
                                };

                            results.Add(e);                                
                            }
                        }
                    }

                }

                return results;
        }

        private static void AddEmployeeInternal<T>(T e)
        {
            //int ID = GetEmployeesTotalNumber(DAOAcsessModifier.GetEmployeesTotalNumber);
            int ID = Statics.UniqeNumber(GetEmployeesTotalNumber(DAOAcsessModifier.GetEmployeesTotalNumber));

            using (_connection = OpenConection())
            {               
                ID++;
                var NAME = e.GetType().GetProperty("NAME").GetValue(e);
                var AGE = e.GetType().GetProperty("AGE").GetValue(e);
                var ADDRESS = e.GetType().GetProperty("ADDRESS").GetValue(e);
                var SALARY = e.GetType().GetProperty("SALARY").GetValue(e);
                using (SQLiteCommand command = new SQLiteCommand($"INSERT INTO COMPANY (ID, NAME, AGE, ADDRESS, SALARY) VALUES ({ID}, '{NAME}', {AGE}, '{ADDRESS}', {SALARY})", _connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        private static void UpdateEmployeeInternal<T>(T e)
        {
            int ID = Statics.UniqeNumber(GetEmployeesTotalNumber(DAOAcsessModifier.GetEmployeesTotalNumber));
            using (_connection = OpenConection())
            {                
                ID++;
                var EmployeeID = e.GetType().GetProperty("ID").GetValue(e);
                var NAME = e.GetType().GetProperty("NAME").GetValue(e);
                var AGE = e.GetType().GetProperty("AGE").GetValue(e);
                var ADDRESS = e.GetType().GetProperty("ADDRESS").GetValue(e);
                var SALARY = e.GetType().GetProperty("SALARY").GetValue(e);
                using (SQLiteCommand command = new SQLiteCommand($"INSERT INTO UPDATED_EMPLOYEES (ID, EmplpyeeID, NAME, AGE, ADDRESS, SALARY) VALUES ({ID}, {EmployeeID}, '{NAME}', {AGE}, '{ADDRESS}', {SALARY})", _connection))
                {
                    command.ExecuteNonQuery();
                }                
                using (SQLiteCommand command = new SQLiteCommand($"UPDATE COMPANY SET NAME = '{NAME}', AGE = {AGE}, ADDRESS = '{ADDRESS}', SALARY = {SALARY} WHERE ID = {EmployeeID}", _connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void DeleteEmployeeInternal<T>(T e)
        {
            using (_connection = OpenConection())
            {
                using (SQLiteCommand command = new SQLiteCommand($"DELETE FROM COMPANY WHERE ID = {e.GetType().GetProperty("ID").GetValue(e)}", _connection))                
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        private static bool IsInDataBaseInternal<T>(T e) where T : class
        {
            bool returnValue = false;
            using (_connection = OpenConection())
            {                
                using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM COMPANY WHERE ID = {(int)e.GetType().GetProperty("ID").GetValue(e)}", _connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int readerValue = reader.GetInt32(0);
                            returnValue = (int)e.GetType().GetProperty("ID").GetValue(e) == readerValue;
                            if (!reader.HasRows) break;
                        }
                    }
                }
            }
            return returnValue;
        }

        private static int GetEmployeesTotalNumberInternal()
        {
            int rows = -2;
            using (_connection = OpenConection())
            {
                using (SQLiteCommand command = new SQLiteCommand("SELECT COUNT(ID) FROM COMPANY", _connection))
                {
                    
                    rows = Convert.ToInt32(command.ExecuteScalar());

                }
                _connection.Close();
            }
            return rows;
        }



    
    

        //private SQLiteConnection con = new SQLiteConnection($"Data source ={_dBpath}; Version = 3;");

    }
}
