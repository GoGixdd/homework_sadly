using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace deloproject
{
    public class sqlmanager
    {
        private string _dbname;
        private bool _isInitialized;

        public sqlmanager()
        {
        }

        public sqlmanager(string dbname)
        {
            _dbname = dbname;
        }

        public bool isExistsDBFile(string dbfile)
        {
            return File.Exists(dbfile);
        }

        public SQLiteConnection con = null;
        public SQLiteCommand cmd = null;

        public void InitializeDB()
        {
            if (_isInitialized)
                return;
            bool isDBFistTimeUsed = !isExistsDBFile("projectdb.db");
            string dbfile = @"URI=file:projectdb.db";
            con = new SQLiteConnection(dbfile);
            con.Open();
            cmd = new SQLiteCommand(con);
            if (isDBFistTimeUsed)
            {
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS users(id INTEGER PRIMARY KEY, login TEXT, password TEXT)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS tasks(id INTEGER PRIMARY KEY, description TEXT, date TEXT, worker TEXT, status TEXT, login TEXT)";
                cmd.ExecuteNonQuery();
            }
            _isInitialized = true;
        }

        public bool isCorrectLogin(string login, string password)
        {
            if (!isLoginExists(login))
            {
                MessageBox.Show("Пользователя не существует!");
                return false;
            }

            string stm = "SELECT * FROM users WHERE login = '" + login + "' AND password = '" + password + "' LIMIT 1";

            var cmd = new SQLiteCommand(stm, con);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            return rdr.Read();
        }

        public bool isLoginExists(string login)
        {
            string stm = "SELECT * FROM users WHERE login = '" + login + "'";

            var cmd = new SQLiteCommand(stm, con);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            return rdr.Read();
        }

        public bool isRegistered(string login, string password)
        {
            if (isLoginExists(login))
                return false;

            cmd.CommandText = "INSERT INTO users(login, password) VALUES('" + login + "','" + password + "')";
            int c = cmd.ExecuteNonQuery();
            if (c > 0)
                return true;
            return false;
        }

        public bool isCreatedTask(string description, string date, string worker, string status, string login)
        {
            cmd.CommandText = "INSERT INTO tasks(description, date, worker, status, login) VALUES('" + description + "','" + date + "','" + worker + "','" + status + "','" + login + "')";
            int c = cmd.ExecuteNonQuery();
            if (c > 0)
                return true;
            return false;
        }

        public bool isDeletedTask(int id)
        {
            cmd.CommandText = "DELETE FROM tasks WHERE id = '" + id + "'";
            int c = cmd.ExecuteNonQuery();
            if (c > 0)
                return true;
            return false;
        }

        public bool isMovedTask(int id, string newstatus)
        {
            //UPDATE tasks SET 'date' = '1' WHERE id = '3'
            cmd.CommandText = "UPDATE tasks SET 'status' = '" + newstatus + "' WHERE id = '" + id + "'";
            int c = cmd.ExecuteNonQuery();
            if (c > 0)
                return true;
            return false;
        }

        public (int, string, string, string, string)[] isGetAllTask(string login)
        {
            string stm = "SELECT * FROM tasks WHERE login = '" + login + "'";

            var cmd = new SQLiteCommand(stm, con);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            (int, string, string, string, string)[] kek = new (int, string, string, string, string)[0];
            //MessageBox.Show($"{rdr.GetInt32(0)} {rdr.GetString(1)}  {rdr.GetString(2)}  {rdr.GetString(3)}  {rdr.GetString(4)}  {rdr.GetString(5)}");
            while (rdr.Read())
            {
                //Console.WriteLine($"{rdr.GetInt32(0)} {rdr.GetString(1)}  {rdr.GetString(2)}  {rdr.GetString(3)}  {rdr.GetString(4)}  {rdr.GetString(5)}");
                var tuple = (rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4));
                kek = new List<(int, string, string, string, string)>(kek) { tuple }.ToArray();
            }
            return kek;
        }

        public (int, string, string, string, string) isGetTask(int id)
        {
            string stm = "SELECT * FROM tasks WHERE id =" + id;

            var cmd = new SQLiteCommand(stm, con);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            (int, string, string, string, string) tuple = (0, null, null, null, null);
            while (rdr.Read())
            {
                tuple = (rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4));
            }
            return tuple;
        }
    }
}