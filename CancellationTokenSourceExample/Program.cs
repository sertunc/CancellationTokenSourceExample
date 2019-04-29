using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancellationTokenSourceExample
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cancel = new CancellationTokenSource();

            Program p = new Program();

            //Sayma işlemini gerçekleştirecek Task'ı
            //başlatıyoruz. Dikkat ederseniz Task'a
            //parametre olarak "İptal" işini kontrol
            //eden CancellationTokenSource tipinde
            //yarattığımız nesneyi veriyoruz.
            var task1 = Task.Factory.StartNew(() => p.CountNumbers(cancel.Token), cancel.Token);

            //t zamanında "DUR" diyecek olan diğer
            //Task'ı başlatıyoruz.1-5 saniye arasında
            //bir zamanda CountNumbers metodunun
            //durmasına sebep olacak.
            var task2 = Task.Factory.StartNew(() => p.Stop(cancel));

            Console.ReadLine();
        }

        protected void CountNumbers(CancellationToken cancellationToken)
        {
            int number = 0;

            for (int i = 0; i < 100000; i++)
            {
                number = i;

                //Sayıları ekrana yazdırıyoruz.
                Console.WriteLine("Person X: " + number.ToString());
                Thread.Sleep(80);

                //CancelationTokenSource tipindeki nesnemizin
                //CancelationToken tipindeki Token özelliğininden
                //IsCancellationRequested değerine bakıyoruz.
                //Bu değer eğer CancelationTokenSource nesnemizin
                //Cancel() metodu çağırılsa, iptal işleminin
                //gerçekleşmesi gerektiğini bildirmek adına
                //true olacaktır.
                if (cancellationToken.IsCancellationRequested)
                {
                    //İptal işlemi gerçekleşti
                    //Mevcut son sayıyı ekrana yazdıralım.
                    Console.WriteLine("Current number is:" + number.ToString());
                    break;
                }
                else
                {
                    Console.Clear();
                }
            }

        }

        //Sayma işlemini İptal edeceğimiz Task'ın metodu
        protected void Stop(CancellationTokenSource tokenSource)
        {
            //t zamanda iptal işlemini gerçekleştirmek
            //için Random olarak bir saniye değeri
            //üretiyoruz.
            Random waitTime = new Random();
            int seconds = waitTime.Next(1 * 1000, 6 * 1000);

            //Ürettiğimiz saniye kadar bekliyoruz.
            Thread.Sleep(seconds);
            Console.WriteLine("Person Y: STOP!!!!!!");

            //CancellationTokenSource objemizin Cancel()
            //metodunu çağırarak,CancellationTokenSource
            //nesnesine sahip olan diğer Task
            //işlemlerinin iptal edilmesi için ilgili
            //bildirimi yapıyoruz.
            tokenSource.Cancel();
        }
    }
}
