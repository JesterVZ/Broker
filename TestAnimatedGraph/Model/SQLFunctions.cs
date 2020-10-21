using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace TestAnimatedGraph.Model
{
    class SQLFunctions
    {
        public void Init()
        {
            var DataBase = File.Exists("DataBase.db");
            if (!DataBase)
            {
                SQLiteConnection.CreateFile("DataBase.db");
            }
        }
        public void Link()
        {
            using SQLiteConnection Connect = new SQLiteConnection(@"Data Source=DataBase.db; Version=3;");
            Command(Connect, "CREATE TABLE IF NOT EXISTS [Main_Table] ( [id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, [Sale_Cost] DOUBLE, [Buy_Cost] DOUBLE)");
            Command(Connect, "CREATE TABLE IF NOT EXISTS [Operation_Value_Table] ( [id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, [Person] VARCHAR(10), [Operation] VARCHAR(10))");
        }

        private void Command(SQLiteConnection Connect, string command)
        {
            SQLiteCommand Command = new SQLiteCommand(command, Connect);
            Connect.Open(); // открыть соединение
            Command.ExecuteNonQuery(); // выполнить запрос
            Connect.Close(); // закрыть соединение
        }
        public void InsertSaleAndCostValues(double saleCostValue, double buyCostValue)
        {
            using SQLiteConnection Connect = new SQLiteConnection(@"Data Source=DataBase.db; Version=3;");
            string commandText = "INSERT INTO [Main_Table] ([Sale_Cost], [Buy_Cost]) VALUES(@sale_cost, @buy_cost)";
            SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
            Command.Parameters.AddWithValue("@sale_cost", saleCostValue); // присваиваем переменной значение
            Command.Parameters.AddWithValue("@buy_cost", buyCostValue);
            Connect.Open();
            Command.ExecuteNonQuery();
            Connect.Close();
        }
        public void InsertOperationValue(string personValue, string operationValue)
        {
            using SQLiteConnection Connect = new SQLiteConnection(@"Data Source=DataBase.db; Version=3;");
            string commandText = "INSERT INTO [Operation_Value_Table] ([Person], [Operation]) VALUES(@Person, @Operation)";
            SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
            Command.Parameters.AddWithValue("@Person", personValue); 
            Command.Parameters.AddWithValue("@Operation", operationValue);
            Connect.Open();
            Command.ExecuteNonQuery();
            Connect.Close();
        }
    }
}
