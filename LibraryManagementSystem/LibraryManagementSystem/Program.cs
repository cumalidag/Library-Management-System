// See https://aka.ms/new-console-template for more information
Console.ForegroundColor = ConsoleColor.Green;
while (true)
{
    Console.Clear();
    Console.WriteLine("\t Kütüphane Sistemine Hoş geldiniz!\n");
    Console.WriteLine("[1] Kütüphanedeki tüm kitapların listesini görüntüleyin");
    Console.WriteLine("[2] Kütüphaneye yeni bir kitap ekleyin.");
    Console.WriteLine("[3] Süresi geçmiş kitaplarla ilgili bilgileri görüntüleyin.");
    Console.WriteLine("[4] Ödünç alınan kitaplar.");
    Console.WriteLine("[5] Kitap araması yapın.");
    Console.WriteLine("\n[0] Çıkış yapın.");

    Console.Write("\nLütfen yapmak istediğiniz işlemi seçiniz: ");
    int secim = Convert.ToInt32(Console.ReadLine());
    Books books = new Books();


    switch (secim)
    {
        case 0:
            Console.WriteLine("Çıkış yapılıyor...");
            Environment.Exit(0);
            break;
        case 1:
            Console.WriteLine("Kütüphanedeki tüm kitaplar listeleniyor...");
            books.ListBook();
            break;
        case 2:
            Console.WriteLine("Yeni kitap eklemek için lütfen aşağıdaki bilgileri giriniz.");
            books.AddBook();
            break;
        case 3:
            Console.WriteLine("Süresi geçmiş kitaplar listeleniyor...");
            books.ExpiredBook();
            break;
        case 4:
            Console.WriteLine("Ödünç alınan kitaplar listeleniyor...");
            books.TakenBook();
            break;
        case 5:
            Console.WriteLine("Kitap araması yapılıyor...");
            books.SearchBook();
            break;
        default:
            Console.WriteLine("Hatalı seçim yaptınız!");
            break;
    }
}


