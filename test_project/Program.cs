using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devart.Data.SQLite;
namespace test_project
{
    class Program
    {
      static readonly int[] ColumnsWidth = { 50, 14, 20, 20, 109 };
        
      static  void Opencon()
        {
            SQLiteConnectionStringBuilder connSB = new SQLiteConnectionStringBuilder();
            connSB.DataSource = @"C:\Users\Виктория\Documents\Visual Studio 2017\test_project\test_project\db\library.db";
            connSB.FailIfMissing = false;
            connSB.Locking = LockingMode.Exclusive;
            connSB.AutoVacuum = AutoVacuumMode.Full;
            connSB.ConnectionTimeout = 20;
            SQLiteConnection sqLiteConnection1 = new SQLiteConnection(connSB.ConnectionString);
            sqLiteConnection1.Open();
        }
        
      static  void Closecon()
        {
            SQLiteConnection sqLiteConnection1 = new SQLiteConnection();
            sqLiteConnection1.ConnectionString = @"Data Source=C:\Users\Виктория\Documents\Visual Studio 2017\test_project\test_project\db\library.db";
            sqLiteConnection1.Open();
           
            sqLiteConnection1.Close();
        }

        static void Insert()
        {
            string name;
            Console.WriteLine("Введите название книги,которое вы хотите ввести:");
            name = Console.ReadLine();
            int year=0;bool l = true;
            do { 
            Console.WriteLine("Введите год написания книги:");
            
                try { year = Convert.ToInt32(Console.ReadLine());l = false; }
                catch { Console.WriteLine("Вы ввели неверные данные."); l=true; }
            } while (l == true);
            SQLiteConnection conn = new SQLiteConnection(@"Data Source=C:\Users\Виктория\Documents\Visual Studio 2017\test_project\test_project\db\library.db;FailIfMissing=True;");
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.CommandText = "INSERT INTO book (name,year) VALUES ('"+name+"',"+year+")";
            cmd.Connection = conn;
            conn.Open();
            try
            {
                int aff = cmd.ExecuteNonQuery();
                Console.WriteLine("Книга успешно введена!");
            }
            catch
            {
                Console.WriteLine("Ошибка во время выполнения внесения данных");
            }
            finally
            {
                conn.Close();
            }
            string k;
            Console.WriteLine("Есть ли у это книги автор? Да/Нет");
            k = Console.ReadLine();
            if (k.ToLower() == "да")
            { bool f = true;
                Console.WriteLine("Существующие автора:");
                SQLiteConnection con2 = new SQLiteConnection(@"Data Source=C:\Users\Виктория\Documents\Visual Studio 2017\test_project\test_project\db\library.db;FailIfMissing=True;");

                SQLiteCommand cmd2 = con2.CreateCommand();
                cmd2.CommandText = "Select lname_a from author";
                con2.Open();
                using (SQLiteDataReader reader = cmd2.ExecuteReader())
                {
                   
                    Console.Write(Environment.NewLine);
                    
                    while (reader.Read())
                    {  
                        for (int i = 0; i < reader.FieldCount; i++)
                            Console.Write(reader.GetValue(i).ToString() + ", ");
                        Console.Write(Environment.NewLine);
                    }

                }
                do
                {
                    string lname;
                    bool p = true;
                    do
                    {
                        Console.WriteLine("Введите фамилию автора этой книги:");
                        lname = Console.ReadLine();

                        SQLiteConnection con4 = new SQLiteConnection(@"Data Source=C:\Users\Виктория\Documents\Visual Studio 2017\test_project\test_project\db\library.db;FailIfMissing=True;");

                        SQLiteCommand cmd4 = con4.CreateCommand();
                        cmd4.CommandText = "Select lname_a from author where lname_a='" + lname + "'";
                        con4.Open();
                        using (SQLiteDataReader reader = cmd4.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                Console.WriteLine("Вы ввели несуществующего автора");
                                p = true;
                            }
                            else p = false;
                        }
                    } while (p == true);

                        SQLiteConnection con1 = new SQLiteConnection(@"Data Source=C:\Users\Виктория\Documents\Visual Studio 2017\test_project\test_project\db\library.db;FailIfMissing=True;");
                        SQLiteCommand cmd1 = new SQLiteCommand();
                        cmd1.CommandText = "INSERT INTO book_author (id_a,id_b) VALUES ((select id_a from author where lname_a='" + lname + "'),(select id_b from book where name='" + name + "'))";
                        cmd1.Connection = con1;
                        con1.Open();
                        try
                        {
                            int aff = cmd1.ExecuteNonQuery();
                           
                            Console.WriteLine("Автор успешно был привязан к книге!");
                        }
                        catch
                        {
                            Console.WriteLine("Ошибка во время выполнения внесения данных");
                        }
                        finally
                        {
                            con1.Close();
                        }
                        string c;
                        Console.WriteLine("Вы хотите добавить еще автора? Да/Нет");
                        c = Console.ReadLine();
                        if (c.ToLower() == "нет") { f = false; }
                    
                } while (f == true);
            }
            else
            {
                SQLiteConnection con1 = new SQLiteConnection(@"Data Source=C:\Users\Виктория\Documents\Visual Studio 2017\test_project\test_project\db\library.db;FailIfMissing=True;");
                SQLiteCommand cmd1 = new SQLiteCommand();
                cmd1.CommandText = "INSERT INTO book_author (id_a,id_b) VALUES (4,(select id_b from book where name='" + name + "'))";
                cmd1.Connection = con1;
                con1.Open();
                try
                {
                    int aff = cmd1.ExecuteNonQuery();
                    Console.WriteLine(aff + " rows were affected.");
                    
                }
                catch
                {
                    Console.WriteLine("Ошибка во время выполнения внесения данных.");
                }
                finally
                {
                    con1.Close();
                }

            }
        }
        static void SelectBook()
        {
            Console.WriteLine("Существующие книги:");
            SQLiteConnection con2 = new SQLiteConnection(@"Data Source=C:\Users\Виктория\Documents\Visual Studio 2017\test_project\test_project\db\library.db;FailIfMissing=True;");

            SQLiteCommand cmd2 = con2.CreateCommand();
            cmd2.CommandText = "Select name from book";
            con2.Open();
            using (SQLiteDataReader reader = cmd2.ExecuteReader())
            {
                
                Console.Write(Environment.NewLine);
                
                while (reader.Read())
                {  
                    for (int i = 0; i < reader.FieldCount; i++)
                        Console.Write(reader.GetValue(i).ToString() + ", ");
                    Console.Write(Environment.NewLine);
                }

            }
        }
       static string CheckBook(string name)
        {
            bool p=true;
            do
            {
                name = Console.ReadLine();
                SQLiteConnection con4 = new SQLiteConnection(@"Data Source=C:\Users\Виктория\Documents\Visual Studio 2017\test_project\test_project\db\library.db;FailIfMissing=True;");

                SQLiteCommand cmd4 = con4.CreateCommand();
                cmd4.CommandText = "Select name from book where name='" + name + "'";
                con4.Open();
                using (SQLiteDataReader reader = cmd4.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        Console.WriteLine("Вы ввели несуществующую книгу. \nВведите книгу заново:");
                        p = true;
                    }
                    else p = false;    
                }
            } while (p == true);
            return name;
        }
      
        static void Delete()
        {
            SelectBook();
            string name="";
            Console.WriteLine("Какую книгу вы хоите удалить?");
            name = CheckBook(name);
                SQLiteConnection conn = new SQLiteConnection(@"Data Source=C:\Users\Виктория\Documents\Visual Studio 2017\test_project\test_project\db\library.db;FailIfMissing=True;");
            SQLiteCommand cmd= new SQLiteCommand();
            cmd.CommandText = "Delete from book_author where id_b=(select id_b from book where name='" + name + "')";
            
            cmd.Connection = conn;
            conn.Open();
            try
            {
                int aff = cmd.ExecuteNonQuery();
              
            }
            catch
            {
                Console.WriteLine("Ошибка во время выполнения внесения данных");
            }
            finally
            {
                conn.Close();
            }
            SQLiteConnection conn1 = new SQLiteConnection(@"Data Source=C:\Users\Виктория\Documents\Visual Studio 2017\test_project\test_project\db\library.db;FailIfMissing=True;");
            SQLiteCommand cmd1 = new SQLiteCommand();
            cmd1.CommandText = "Delete from book where name='" + name + "'";
            cmd1.Connection = conn1;
            conn1.Open();
            try
            {
                int aff = cmd1.ExecuteNonQuery();
                
                Console.WriteLine("Книга удалена!");
            }
            catch
            {
                Console.WriteLine("Ошибка во время выполнения внесения данных");
            }
            finally
            {
                conn1.Close();
            }
        }
        static void Update()
        {
            int year;
            string book="";
            SelectBook();
            Console.WriteLine("Для какой книги вы хотите изменить год написания?");
            book = CheckBook(book);
            Console.WriteLine("Введите новый год для книги");
            year = Convert.ToInt32(Console.ReadLine());
            SQLiteConnection conn = new SQLiteConnection(@"Data Source=C:\Users\Виктория\Documents\Visual Studio 2017\test_project\test_project\db\library.db;FailIfMissing=True;");
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.CommandText = "Update book set year="+year+" where name='"+book+"'";
            cmd.Connection = conn;
            conn.Open();
            try
            {
                int aff = cmd.ExecuteNonQuery();
              
                Console.WriteLine("Год этой книги был изменен!");
            }
            catch
            {
                Console.WriteLine("Ошибка во время выполнения внесения данных");
            }
            finally
            {
                conn.Close();
            }

        }
        static void Select()
        {
            SQLiteConnection conn = new SQLiteConnection(@"Data Source=C:\Users\Виктория\Documents\Visual Studio 2017\test_project\test_project\db\library.db;FailIfMissing=True;");
           
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "Select b.name as'Название книги',b.year as 'Год издания',a.name_a as 'Имя автора',a.lname_a as 'Фамилия автора' from book b natural join book_author ba natural join author a ";
            conn.Open();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // printing the column names
                Console.WriteLine("\n".PadRight(ColumnsWidth[4], '-'));
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string header = reader.GetName(i).ToString();
                    Console.Write(header.PadRight(ColumnsWidth[i],' ')+"|");
                }
                Console.WriteLine("\n".PadRight(ColumnsWidth[4], '-'));
               
                Console.Write(Environment.NewLine);
               
                while (reader.Read())
                {  
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader.GetValue(i).ToString().PadRight(ColumnsWidth[i], ' ') + "|");
                    }
                    Console.Write(Environment.NewLine);
                }
                Console.WriteLine("\n".PadRight(ColumnsWidth[4], '-'));
            }
        }
    
        static void Main(string[] args)
        { 
            Opencon();
            bool f = true;
            do
            {
                int k;
                Console.WriteLine("Здравствуйте!\nМеню программы:\n1-Добавить книгу,\n2-Просмотреть все книги и их авторов,\n3-Удалить книгу, \n4-Изменить год издания книги,\n  Сделайте ваш выбор!");
                k = Convert.ToInt32(Console.ReadLine());
                switch (k)
                {
                    case 1: { Insert(); break; };
                    case 2: { Select();break; }
                    case 3: { Delete();break; }
                    case 4: { Update();break; }
                    default: { Console.WriteLine("Вы ввели неверные данные"); break; }
                }
                string s;
                Console.WriteLine("Хотите вернуться в начало меню?Да/Нет");
               
                s = Console.ReadLine();
                if (s.ToLower() == "нет") { f = false; }
            } while (f == true);
         
            Closecon();
            Console.ReadKey();
        }
    }
}
