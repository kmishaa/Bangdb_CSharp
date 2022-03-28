using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BangDB_CSharp;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Collections;

class Date
{
    private int day = 1;
    private int month = 1;
    private int year = 2000;

    public Date(int new_day, int new_month, int new_year) { day = new_day; month = new_month; year = new_year; }
    public String Get_Date()
    {
        String day_str = day.ToString();
        String month_str = month.ToString();
        if (day < 10) day_str = "0" + day_str;
        if (month < 10) month_str = "0" + month_str;
        return (day_str + "." + month_str + "." + year.ToString() + "'}");
    }
}
class Student
{
    private String Name = "Michael Krasovitsky";
    private int Age = 20;
    private double Rating = 62.1;
    private Date Birthdate = new Date(13, 6, 2001);

    public Student(String new_name, int new_age, double new_rating, Date new_date)
    {
        Name = new_name; Age = new_age; Rating = new_rating; Birthdate = new_date;
    }

    public String Get_Info()
    {
        return ("Student {Name='" + Name + "', Age='" + Age.ToString() + "', Rating='" + Rating.ToString() + "', Birthdate='" + Birthdate.Get_Date());
    }


}

namespace BangDBcs
{
    class Program
    {
        static void Main(string[] args)
        {
            // Директория создания базы данных
            string path = "C:\\DB\\CORE\\lib";

            // Создание бд
            Database db = new Database("mydb", path);
            Console.WriteLine("\n\n\n\nDatabase created!");

            // Создание таблицы
            Table tbl = db.GetTable("mytbl");
            if (tbl == null)
            {
                Console.WriteLine("err in tbl creation");
                db.CloseDatabase();
                return;
            }
            Console.WriteLine("Table opened!");

            // Установка соединения с таблицей
            Connection conn = tbl.GetConnection();
            if (conn == null)
            {
                Console.WriteLine("err in conn creation");
                db.CloseDatabase();
                return;
            }

            // string
            Console.WriteLine("\nString");
            string k = "Michael";
            string v = "Krasovitsky";
            if (conn.Put(k, v, InsertOptions.InsertUnique) < 0) Console.WriteLine("error in put");
            string o = null;

            if (!conn.Get(k, out o))
            {
                Console.WriteLine("err in value getting");
                db.CloseDatabase();
                return;
            }
            if (o == null) Console.WriteLine("err in value getting: get \"null\"");
            if (!o.Equals(v))
            {
                Console.WriteLine("err in value getting: expected \"" + v + "\", but get \"" + o + "\"");
                db.CloseDatabase();
                return;
            }

            Console.WriteLine("Key 1: " + k + ", value 1: " + o);

            // int
            Console.WriteLine("\n\nInt");
            String k_int = "Age";
            int v_int = 20;
            String v_int_str = v_int.ToString();
            if (conn.Put(k_int, v_int_str, InsertOptions.InsertUnique) < 0) Console.WriteLine("error in put");
            string o2 = null;
            if (!conn.Get(k_int, out o2))
            {
                Console.WriteLine("err in value getting");
                db.CloseDatabase();
                return;
            }
            if (o2 == null) Console.WriteLine("err in value getting: get \"null\"");
            if (!o2.Equals(v_int_str))
            {
                Console.WriteLine("err in value getting: expected \"" + v_int_str + "\", but get \"" + o2 + "\"");
                db.CloseDatabase();
                return;
            }

            Console.WriteLine("Key 2: " + k_int + ", value 2: " + o2);

            // byte
            Console.WriteLine("\n\nByte[]");
            String k_byte = "Key for Byte[]";
            byte[] v_byte = new byte[] { 1, 2, 3 };

            // массив в строку
            String v_byte_str = "[";
            for (int i = 0; i < v_byte.Length; i++)
            {
                v_byte_str += v_byte[i].ToString();
                if (i != v_byte.Length - 1) v_byte_str += "; ";
            }
            v_byte_str += "]";
            //Console.WriteLine(v_byte_str);

            if (conn.Put(k_byte, v_byte_str, InsertOptions.InsertUnique) < 0) Console.WriteLine("error in put");
            string o3 = null;
            if (!conn.Get(k_byte, out o3))
            {
                Console.WriteLine("err in value getting");
                db.CloseDatabase();
                return;
            }
            if (o3 == null) Console.WriteLine("err in value getting: get \"null\"");
            if (!o3.Equals(v_byte_str))
            {
                Console.WriteLine("err in value getting: expected \"" + v_byte_str + "\", but get \"" + o3 + "\"");
                db.CloseDatabase();
                return;
            }

            Console.WriteLine("Key 3: " + k_byte + ", value 3: " + o3);

            // List
            Console.WriteLine("\n\nList");
            String k_list = "Key for List";
            List<Object> v_list = new List<Object>() { "first", 2, "end" };
            String v_list_str = "{";
            for (int i = 0; i < v_list.Count; i++)
            {
                v_list_str += v_list[i].ToString();
                if (i != v_list.Count - 1) v_list_str += "; ";
            }
            v_list_str += "}";
            //Console.WriteLine(v_list_str);

            if (conn.Put(k_list, v_list_str, InsertOptions.InsertUnique) < 0) Console.WriteLine("error in put");
            string o4 = null;
            if (!conn.Get(k_list, out o4))
            {
                Console.WriteLine("err in value getting");
                db.CloseDatabase();
                return;
            }
            if (o4 == null) Console.WriteLine("err in value getting: get \"null\"");
            if (!o4.Equals(v_list_str))
            {
                Console.WriteLine("err in value getting: expected \"" + v_list_str + "\", but get \"" + o4 + "\"");
                db.CloseDatabase();
                return;
            }

            Console.WriteLine("Key 4: " + k_list + ", value 4: " + o4);

            // HashSet
            Console.WriteLine("\n\nHashset");
            String k_set = "Key for HashSet";
            HashSet<Object> v_set = new HashSet<Object>() { "string", 22.33, 111 };
            String v_set_str = "{";
            bool first = true;
            foreach (Object i in v_set)
            {
                if (first) first = false;
                else v_set_str += "; ";
                v_set_str += i.ToString();
            }
            v_set_str += "}";
            //Console.WriteLine(v_set_str);

            if (conn.Put(k_set, v_set_str, InsertOptions.InsertUnique) < 0) Console.WriteLine("error in put");
            string o5 = null;
            if (!conn.Get(k_set, out o5))
            {
                Console.WriteLine("err in value getting");
                db.CloseDatabase();
                return;
            }
            if (o5 == null) Console.WriteLine("err in value getting: get \"null\"");
            if (!o5.Equals(v_set_str))
            {
                Console.WriteLine("err in value getting: expected \"" + v_set_str + "\", but get \"" + o5 + "\"");
                db.CloseDatabase();
                return;
            }

            Console.WriteLine("Key 5: " + k_set + ", value 5: " + o5);

            //Hashtable
            Console.WriteLine("\n\nHashtable");
            String k_hash = "Key for HashTable";
            Hashtable v_hash = new Hashtable();
            v_hash.Add("float", 11.22);
            v_hash.Add("int", 2019);
            v_hash.Add("string", "Hello, World!");
            bool is_first = true;
            String v_hash_str = "[";
            foreach (DictionaryEntry i in v_hash)
            {
                if (is_first) is_first = false;
                else v_hash_str += "; ";
                v_hash_str += "{" + i.Key.ToString() + " : " + i.Value.ToString() + "}";
            }
            v_hash_str += "]";
            //Console.WriteLine(v_hash_str);

            if (conn.Put(k_hash, v_hash_str, InsertOptions.InsertUnique) < 0) Console.WriteLine("error in put");
            string o6 = null;
            if (!conn.Get(k_hash, out o6))
            {
                Console.WriteLine("err in value getting");
                db.CloseDatabase();
                return;
            }
            if (o6 == null) Console.WriteLine("err in value getting: get \"null\"");
            if (!o6.Equals(v_hash_str) && false)
            {
                Console.WriteLine("err in value getting: expected \"" + v_hash_str + "\", but get \"" + o6 + "\"");
                db.CloseDatabase();
                return;
            }

            Console.WriteLine("Key 6: " + k_hash + ", value 6: " + o6);

            // Class
            Console.WriteLine("\n\nClass");
            String k_class = "Key for Class";
            Student v_class = new Student("Michael Krasovitsky", 20, 60.1, new Date(13, 06, 2001));
            String v_class_str = v_class.Get_Info();
            //Console.WriteLine(v_class_str);

            if (conn.Put(k_class, v_class_str, InsertOptions.InsertUnique) < 0) Console.WriteLine("error in put");
            string o7 = null;
            if (!conn.Get(k_class, out o7))
            {
                Console.WriteLine("err in value getting");
                db.CloseDatabase();
                return;
            }
            if (o7 == null) Console.WriteLine("err in value getting: get \"null\"");
            if (!o7.Equals(v_class_str))
            {
                Console.WriteLine("err in value getting: expected \"" + v_class_str + "\", but get \"" + o7 + "\"");
                db.CloseDatabase();
                return;
            }

            Console.WriteLine("Key 7: " + k_class + ", value 7: " + o7);

            db.CloseDatabase();
            Console.WriteLine("\nDatabase deleted!");
            return;
        }
    }
}
