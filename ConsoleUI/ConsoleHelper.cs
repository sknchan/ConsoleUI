using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleUI
{
    public class ConsoleHelper
    {
        private static readonly Queue<KeyValuePair<string, bool>> _WriteQue = new Queue<KeyValuePair<string, bool>>();
        private static readonly object _processingWriteQueLock = new object();
        private static readonly object _lockWriteQue = new object();
        private static bool _processWriteQues = false;
        public static void ConsoleClear()
        {
            TempLogPointor = LogPointor;
            TempExceptionPointor = ExceptionPointor;
            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
            Console.Clear();
            WriteDeafult();
        }
        public static void WriteDeafult()
        {
            Console.ResetColor();
            Console.Title = "Edge ManageLog Console";
            Console.WriteLine("Edge Panel Version 1.0");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("----- INFORMATION PANEL -----");
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write("USERNAME :");
            Console.ResetColor();
            Console.WriteLine(" " + "Name");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("-----------------------------");
            int count = 1;
            Console.WriteLine();
            for (int i = 0; i < 2; i++)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write("ID " + count.ToString());
                Console.ResetColor();
                Console.WriteLine(" : " + "id" + count);
                count++;
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-------- Current Log --------");

            LogPointor = Console.CursorTop;
            TempLogPointor = LogPointor;
            Console.WriteLine(""); //12
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("--------- Exception ---------");
            ExceptionPointor = Console.CursorTop;
            TempExceptionPointor = ExceptionPointor;

            Console.WriteLine(""); // 19
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine(""); //23

        }
        static int TempLogPointor = 20;
        static int TempExceptionPointor = 20;
        static int LogPointor = 12;
        static int ExceptionPointor = 18;

        private static void ProcessWriteQues(object state)
        {
            while (true)
            {
                KeyValuePair<string, bool> log;
                lock (_WriteQue)
                {
                    if (_WriteQue.Count == 0)
                    {
                        lock (_processingWriteQueLock)
                        {
                            _processWriteQues = false;
                        }
                        return;
                    }
                    log = _WriteQue.Dequeue();
                }

                switch(log.Value)
                {
                    case true:
                        Console.SetCursorPosition(0, 0);
                        Console.ResetColor();
                        if (TempExceptionPointor != ExceptionPointor + 4)
                        {
                            Console.SetCursorPosition(0, TempExceptionPointor);
                            Console.Write(new string(' ', Console.WindowWidth));
                            Console.SetCursorPosition(0, TempExceptionPointor);
                            Console.BackgroundColor = ConsoleColor.DarkRed; Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("[Exception] " + DateTime.Now.ToString());
                            Console.ResetColor();
                            Console.WriteLine(" " + log.Key);
                            TempExceptionPointor++;
                        }
                        else
                        {
                            Console.SetCursorPosition(0, TempExceptionPointor);
                            Console.Write(new string(' ', Console.WindowWidth));
                            Console.SetCursorPosition(0, TempExceptionPointor);
                            Console.BackgroundColor = ConsoleColor.DarkRed; Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("[Exception] " + DateTime.Now.ToString());
                            Console.ResetColor();
                            Console.WriteLine(" " + log.Key);

                            TempExceptionPointor = ExceptionPointor;
                        }
                        Console.ResetColor();
                        break;
                    case false:
                        Console.SetCursorPosition(0, 0);
                        Console.ResetColor();
                        if (TempLogPointor != LogPointor + 4)
                        {
                            Console.SetCursorPosition(0, TempLogPointor);
                            Console.Write(new string(' ', Console.WindowWidth));
                            Console.SetCursorPosition(0, TempLogPointor);
                            Console.BackgroundColor = ConsoleColor.DarkCyan; Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("[Info] " + DateTime.Now.ToString());
                            Console.ResetColor();
                            Console.WriteLine(" " + log.Key);
                            TempLogPointor++;
                        }
                        else
                        {
                            Console.SetCursorPosition(0, TempLogPointor);
                            Console.Write(new string(' ', Console.WindowWidth));
                            Console.SetCursorPosition(0, TempLogPointor);
                            Console.BackgroundColor = ConsoleColor.DarkCyan; Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("[Info] " + DateTime.Now.ToString());
                            Console.ResetColor();
                            Console.WriteLine(" " + log.Key);
                            TempLogPointor = LogPointor;

                        }
                        break;
           

                }

            }

        }
        public static void WriteLog(string log)
        {
            lock (_WriteQue)
            {
                _WriteQue.Enqueue(new KeyValuePair<string, bool>(log, false));
            }
            lock (_processingWriteQueLock)
            {
                if (!_processWriteQues)
                {
                    _processWriteQues = true;
                    ThreadPool.QueueUserWorkItem(ProcessWriteQues);
                }
            }
        }

        public static void WriteException(string log)
        {
            lock (_WriteQue)
            {
                _WriteQue.Enqueue(new KeyValuePair<string, bool>(log, true));
            }
            lock (_processingWriteQueLock)
            {
                if(!_processWriteQues)
                {
                    _processWriteQues = true;
                    ThreadPool.QueueUserWorkItem(ProcessWriteQues);
                }
            }
        }
    }
}
