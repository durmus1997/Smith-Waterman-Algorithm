using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Needleman_Wunch
{
    class Program
    {
        public static void Main(string[] args)
        {
            string[] seqSokuma = File.ReadAllLines("seqS.txt");
            string[] seqTokuma = File.ReadAllLines("SeqT.txt");
            string SeqS = seqSokuma[1];
            string SeqT = seqTokuma[1];
            int GAP;
            int MATCH;
            int MISSMATCH;
            int SeqSCnt = SeqS.Length + 1;
            int SeqTCnt = SeqT.Length + 1;
            int[,] NwMatrix = new int[SeqTCnt, SeqSCnt];

            //kullanıcıdan Gap Match ve Missmatch deperlerini alalım

            Console.WriteLine("Gap penalty:");
            GAP = int.Parse(Console.ReadLine());
            Console.WriteLine("Match point:");
            MATCH = int.Parse(Console.ReadLine());
            Console.WriteLine("MissMatch penalty:");
            MISSMATCH = int.Parse(Console.ReadLine());
            char[] SeqSArray = SeqS.ToCharArray();
            char[] SeqTArray = SeqT.ToCharArray();


            //ilk başta tabloyu sıfırlar ile doldurdum.
            for (int i = 0; i < SeqTCnt; i++)
            {

                for (int j = 0; j < SeqSCnt; j++)
                {
                    NwMatrix[i, j] = 0;
                }
            }




            //Matris'in elemanlarının doldurulması.
            //Her bir elemanın solu , üstü ve sol çaprazı kontrol edilip max olan sayı yazılacak.
            //Eşleşme değeri = 10    eşleşeme değeri = -2   boş olma durumu ise = -5 
            for (int i = 1; i < SeqTCnt; i++)
            {
                for (int j = 1; j < SeqSCnt; j++)
                {
                    int scoretemp = 0;
                    if (SeqS.Substring(j - 1, 1) == SeqT.Substring(i - 1, 1))//eğer eşleşirse
                    {
                        scoretemp = NwMatrix[i - 1, j - 1] + MATCH;
                    }
                    else                                               //eşleşmez ise
                    {
                        scoretemp = NwMatrix[i - 1, j - 1] + MISSMATCH;
                    }
                    int ScoreLeft = NwMatrix[i, j - 1] + GAP;
                    int ScoreUp = NwMatrix[i - 1, j] + GAP;
                    int ScoreZero = 0;
                    int maxScore = Math.Max(Math.Max(scoretemp, ScoreLeft), Math.Max(ScoreZero, ScoreUp));
                    NwMatrix[i, j] = maxScore;
                }
            }
            //doldurma evresi sonu.




            //Matrix'in tabloya yazılma fonksiyonu.
            for (int i = 0; i < SeqTCnt; i++)
            {
                if (i != 0)
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.Write(" ");
                }

                for (int j = 0; j < SeqSCnt; j++)
                {

                    if (j != 0)
                    {
                        if (NwMatrix[i, j] >= 10)
                        {
                            Console.Write(" ");
                        }
                        else if (NwMatrix[i, j] < 0)
                        {
                            Console.Write(" ");
                        }
                        else
                        {
                            Console.Write("  ");
                        }
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                    Console.Write(NwMatrix[i, j]);
                }
                Console.Write(Environment.NewLine);
            }
            //Console.ReadLine();
            //Matrix son.

            int temp = 0;
            int refi = 0;
            int refj = 0;
            for (int i = 0; i < SeqTCnt; i++)
            {
                for (int j = 0; j < SeqSCnt; j++)
                {
                    if (i == 0 && j == 0)
                        temp = NwMatrix[i, j];

                    if (NwMatrix[i, j] > temp)
                    {
                        temp = NwMatrix[i, j];
                    }
                }
            }
            int count = 0;
            for (int i = 0; i < SeqTCnt; i++)
            {
                for (int j = 0; j < SeqSCnt; j++)
                {
                    if (NwMatrix[i, j] == temp)
                        count++;
                }
            }
            int[] reference = new int[count * 2];
            int val = 0;
            for (int i = 0; i < SeqTCnt; i++)
            {
                for (int j = 0; j < SeqSCnt; j++)
                {
                    if (NwMatrix[i, j] == temp)
                    {
                        reference[val] = i;
                        val++;
                        reference[val] = j;
                        val++;

                    }
                }
            }
            //TraceBack evresi
            //denem
            string NewSeqA1 = string.Empty; // s için
            string NewSeqB1 = string.Empty; // t için
            for (int i = 0; i < val; i = i + 2)
            {
                refi = reference[i];
                refj = reference[i + 1];
                Traceback(refi, refj, NewSeqA1, NewSeqB1, SeqSArray, SeqTArray, NwMatrix);

            }

            // deneme son




            string NewSeqA = string.Empty; // s için
            string NewSeqB = string.Empty; // t için
            int b = SeqSCnt - 1; //s için
            int a = SeqTCnt - 1; //t için

            //Display the result
            Console.Write(Environment.NewLine);
            Console.ReadLine();

        }
        static void Traceback(int refi, int refj, string S, string T, char[] Sarray, char[] Tarray, int[,] Matrix)
        {
            while (Matrix[refi, refj] != 0)
            {
                if (refi == 0 && refj > 0)
                {
                    S = S[refj - 1] + S;
                    T = "-" + T;
                    refj = refj - 1;
                }
                else if (refj == 0 && refi > 0)
                {
                    S = "-" + S;
                    T = T[refi - 1] + T;
                    refi = refi - 1;
                }
                else
                {
                    if (Matrix[refi - 1, refj - 1] >= Matrix[refi - 1, refj] && Matrix[refi - 1, refj - 1] >= Matrix[refi - 1, refj])
                    {
                        S = Sarray[refj - 1] + S;
                        T = Sarray[refj - 1] + T;
                        refi = refi - 1;
                        refj = refj - 1;
                    }
                    else if (Matrix[refi, refj - 1] >= Matrix[refi - 1, refj] && Matrix[refi, refj - 1] >= Matrix[refi - 1, refj - 1])
                    {
                        S = Sarray[refj - 1] + S;
                        T = "-" + T;
                        refj = refj - 1;
                    }
                    else if (Matrix[refi - 1, refj] >= Matrix[refi - 1, refj - 1] && Matrix[refi - 1, refj] >= Matrix[refi, refj - 1])
                    {
                        S = "-" + S;
                        T = Tarray[refi - 1] + T;
                        refi = refi - 1;
                    }
                }
            }
            Console.WriteLine(S);
            Console.WriteLine(T);
        }
    }

}