using System;
using System.IO;

public class Books
{
    public void ListBook()
    {
        Console.Clear();
        // Uygulamanın çalıştığı dizin
        string uygulamaDizini = AppDomain.CurrentDomain.BaseDirectory;

        // Dosya adı ve uzantısı
        string bookList = @"BookArchive\BookList.txt";
        string takenBookPath = @"BookArchive\TakenBookList.txt";

        // Tam dosya yolunu birleştir
        string bookListPath = Path.Combine(uygulamaDizini, bookList);
        string takenBookPathPath = Path.Combine(uygulamaDizini, takenBookPath);

        // Dosyayı oku
        string bookListFolder = File.ReadAllText(bookListPath);

        // Dosyayı satırlara ayır
        string[] bookListLines = bookListFolder.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        int choose = 0;

        try
        {
            // Kitap listesini ekrana yazdır
            for (int index = 0; index < bookListLines.Length; index++)
            {
                string[] bookListLineArray = bookListLines[index].Split(',');
                Console.WriteLine($"[{index + 1}] Kitap Adı: " + bookListLineArray[0].Trim());
            }

            Console.WriteLine("\n[0] Geri");

            // Kullanıcı ya 0 ya da listedeki bir kitabın numarasını girebilir
            bool isAcceptableRange = false;
            while (!isAcceptableRange)
            {
                Console.Write("\nİşlem yapmak istediğiniz kitabın numarasını girin: ");
                if (int.TryParse(Console.ReadLine(), out choose))
                {
                    if (choose > 0 && choose <= bookListLines.Length)
                    {
                        isAcceptableRange = true;
                    }
                    else if (choose == 0)
                    {
                        isAcceptableRange = true;
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz sayı. Lütfen doğru bir sayı girin.");
                    }
                }
                else
                {
                    Console.WriteLine("Geçersiz sayı. Lütfen doğru bir sayı girin.");
                }
            }

            // Eğer kitap numarası girildiyse kitap bilgilerini ekrana yazdır ve kullanıcıya kitabı alıp almayacağını sor
            if (choose > 0)
            {
                Console.Clear();
                string choosenBookLine = bookListLines[choose - 1];
                string[] choosenBookLineArray = choosenBookLine.Split(',');

                Console.WriteLine("Kitap Adı: " + choosenBookLineArray[0].Trim());
                Console.WriteLine("Yazar: " + choosenBookLineArray[1].Trim());
                Console.WriteLine("ISBN: " + choosenBookLineArray[2].Trim());
                Console.WriteLine("Stok sayısı: " + choosenBookLineArray[3].Trim());

                Console.WriteLine("\n[1] Kitabı al");
                Console.WriteLine("[0] Geri");

                switch (Console.ReadLine())
                {
                    case "1":
                        // Kitabın stok sayısını azalt
                        int stock = int.Parse(choosenBookLineArray[3].Trim());

                        // Stok kontrolü: Eğer stokta kitap varsa işleme devam et
                        if (stock > 0)
                        {
                            stock--;
                            choosenBookLineArray[3] = stock.ToString();

                            bookListLines[choose - 1] = string.Join(",", choosenBookLineArray);

                            File.WriteAllLines(bookListPath, bookListLines);

                            using (StreamWriter sw = new StreamWriter(takenBookPathPath, true))
                            {
                                sw.WriteLine($"{choosenBookLine}, {DateTime.Now}");
                                Console.WriteLine("Kitap başarıyla alındı.");
                            }

                            DateTime today = DateTime.Now;
                            TimeSpan duration = TimeSpan.FromSeconds(1);

                            while (DateTime.Now - today < duration) { /* Bekleme süresi dolana kadar boş bir döngü yap */ }

                            // Güncellenmiş kitap listesini göster
                            ListBook();
                        }
                        else
                        {
                            Console.WriteLine("Stokta yeterli kitap bulunmamaktadır.");
                            ListBook();
                        }
                        break;

                    case "0":
                        // Kitap listesini göster
                        ListBook();
                        break;

                    default:
                        // Geçersiz giriş durumunda kitap listesini göster
                        ListBook();
                        break;
                }

            }

            // 0 girildiyse ek bir işlem yapma

        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    public void AddBook()
    {
        Console.Clear();
        Console.WriteLine("Yeni kitap eklemek için lütfen aşağıdaki bilgileri giriniz.");
        string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
        string bookList = @"BookArchive\BookList.txt";
        string bookListPath = Path.Combine(applicationPath, bookList);

        try
        {
            using (StreamWriter sw = new StreamWriter(bookListPath, true))
            {
                Console.Write("Kitap Adı: ");
                string? bookName = Console.ReadLine();
                while (string.IsNullOrEmpty(bookName))
                {
                    Console.WriteLine("Lütfen geçerli bir kitap adı girin.");
                    bookName = Console.ReadLine();
                }

                Console.Write("Yazar: ");
                string? author = Console.ReadLine();
                while (string.IsNullOrEmpty(author))
                {
                    Console.WriteLine("Lütfen geçerli bir yazar adı girin.");
                    author = Console.ReadLine();
                }

                Console.Write("Sıra Numarası: ");
                string? serialNumber = Console.ReadLine();
                while (string.IsNullOrEmpty(serialNumber))
                {
                    Console.WriteLine("Lütfen geçerli bir sıra numarası girin.");
                    serialNumber = Console.ReadLine();
                }

                Console.Write("Stok Sayısı: ");
                string? stockQuantityStr = Console.ReadLine();
                int stockQuantity;
                while (!int.TryParse(stockQuantityStr, out stockQuantity))
                {
                    Console.WriteLine("Lütfen geçerli bir stok sayısı girin.");
                    stockQuantityStr = Console.ReadLine();
                }

                Console.WriteLine("Girilen Kitap Adı: " + bookName);
                Console.WriteLine("Girilen Yazar: " + author);
                Console.WriteLine("Girilen Sıra Numarası: " + serialNumber);
                Console.WriteLine("Girilen Stok Sayısı: " + stockQuantity);

                // Yeni kitabı dosyaya ekle
                sw.WriteLine($"{bookName},{author},{serialNumber},{stockQuantity}");
            }

            Console.WriteLine("Kitap başarıyla eklendi.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        finally
        {
            Console.WriteLine("Executing finally block.");
        }
    }

    public void TakenBook()
    {
        Console.WriteLine("Ödünç alınan kitaplar listeleniyor...");
        Console.Clear();

        string uygulamaDizini = AppDomain.CurrentDomain.BaseDirectory;
        string bookList = @"BookArchive\BookList.txt";
        string takenBookPath = @"BookArchive\TakenBookList.txt";

        string bookListPath = Path.Combine(uygulamaDizini, bookList);
        string takenBookPathPath = Path.Combine(uygulamaDizini, takenBookPath);

        string takenBookFolder = File.ReadAllText(takenBookPathPath);
        string bookListFolder = File.ReadAllText(bookListPath);

        string[] takenBookLines = takenBookFolder.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        string[] bookListLines = bookListFolder.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        int choose = 0;

        try
        {
            for (int index = 0; index < takenBookLines.Length; index++)
            {
                string[] takenBookLinesArray = takenBookLines[index].Split(',');
                Console.WriteLine($"[{index + 1}] Kitap Adı: {takenBookLinesArray[0].Trim()} Ödünç tarihi: {takenBookLinesArray[4].Trim()}");
            }

            Console.WriteLine("\n[0] Geri");

            bool isAcceptableRange = false;
            while (!isAcceptableRange)
            {
                Console.Write("\nİşlem yapmak istediğiniz kitabın numarasını girin: ");
                if (int.TryParse(Console.ReadLine(), out choose))
                {
                    if (choose > 0 && choose <= takenBookLines.Length)
                    {
                        isAcceptableRange = true;
                    }
                    else if (choose == 0)
                    {
                        isAcceptableRange = true;
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz sayı. Lütfen doğru bir sayı girin.");
                    }
                }
                else
                {
                    Console.WriteLine("Geçersiz sayı. Lütfen doğru bir sayı girin.");
                }
            }

            if (choose > 0)
            {
                Console.Clear();
                string choosenTakenBookLine = takenBookLines[choose - 1];
                string[] choosenTakenBookLineArray = choosenTakenBookLine.Split(',');

                Console.WriteLine("Kitap Adı: " + choosenTakenBookLineArray[0].Trim());
                Console.WriteLine("Yazar: " + choosenTakenBookLineArray[1].Trim());
                Console.WriteLine("ISBN: " + choosenTakenBookLineArray[2].Trim());
                Console.WriteLine("Ödünç tarihi: " + choosenTakenBookLineArray[4].Trim());

                Console.WriteLine("\n[1] Kitabı iade et");
                Console.WriteLine("[0] Geri");

                switch (Console.ReadLine())
                {
                    case "1":
                        // İade edilecek kitap
                        string bookToReturn = choosenTakenBookLineArray[0].Trim();
                        int bookIndex = -1;

                        for (int i = 0; i < bookListLines.Length; i++)
                        {
                            string[] bookInfo = bookListLines[i].Split(',');
                            if (bookInfo[0].Trim() == bookToReturn)
                            {
                                bookIndex = i;
                                break;
                            }
                        }

                        if (bookIndex != -1)
                        {
                            // İade edilen kitabın stok sayısını arttır
                            string[] bookInfoArray = bookListLines[bookIndex].Split(',');
                            int currentStock = int.Parse(bookInfoArray[3].Trim());
                            currentStock++;
                            bookInfoArray[3] = currentStock.ToString();
                            bookListLines[bookIndex] = string.Join(",", bookInfoArray);

                            File.WriteAllLines(bookListPath, bookListLines);
                        }

                        // İade edilen kitabı listeden sil
                        List<string> updatedTakenBooks = new List<string>(takenBookLines);
                        updatedTakenBooks.RemoveAt(choose - 1);
                        File.WriteAllLines(takenBookPathPath, updatedTakenBooks);

                        DateTime today = DateTime.Now;
                        TimeSpan duration = TimeSpan.FromSeconds(1);

                        while (DateTime.Now - today < duration)
                        {
                            // Bekleme süresi dolana kadar boş bir döngü yap
                        }

                        TakenBook();
                        break;

                    case "0":
                        TakenBook();
                        break;
                    default:
                        TakenBook();
                        break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    public void SearchBook()
    {
        Console.Clear();

        string uygulamaDizini = AppDomain.CurrentDomain.BaseDirectory;
        string bookList = @"BookArchive\BookList.txt";
        string bookListPath = Path.Combine(uygulamaDizini, bookList);
        string bookListFolder = File.ReadAllText(bookListPath);

        string[] bookListLines = bookListFolder.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        Console.Write("Arama yapmak istediğiniz kitabın adını veya yazarını girin: ");
        string wantedBook = Convert.ToString(Console.ReadLine());

        bool found = false;

        foreach (string bookLine in bookListLines)
        {
            string[] bookListLinesArray = bookLine.Split(',');

            if (bookListLinesArray[0].Trim().Contains(wantedBook, StringComparison.OrdinalIgnoreCase) ||
                bookListLinesArray[1].Trim().Contains(wantedBook, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Kitap Adı: " + bookListLinesArray[0].Trim());
                Console.WriteLine("Yazar: " + bookListLinesArray[1].Trim());
                Console.WriteLine("ISBN: " + bookListLinesArray[2].Trim());
                Console.WriteLine("Stok sayısı: " + bookListLinesArray[3].Trim());
                Console.WriteLine("----------------------------------------------");

                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("Kitap bulunamadı.");
        }

        Console.WriteLine("\n[0] Geri");
        bool exits = false;


        while (!exits)
        {
            int choose = Convert.ToInt32(Console.ReadLine());
            if (choose == 0)
            {
                exits = true;
            }
        }
    }

    public void ExpiredBook()
    {
        Console.Clear();
        Console.WriteLine("Süresi geçmiş kitaplar listeleniyor...");
        string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
        string takenBookPath = @"BookArchive\TakenBookList.txt";

        string takenBookPathPath = Path.Combine(applicationPath, takenBookPath);

        string takenBookFolder = File.ReadAllText(takenBookPathPath);

        string[] takenBookLines = takenBookFolder.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        int index2 = 0;
        // Kitap listesini ekrana yazdır
        for (int index = 0; index < takenBookLines.Length; index++)
        {
            
            string[] takenBookLinesArray = takenBookLines[index].Split(',');
            DateTime takenDate = DateTime.Parse(takenBookLinesArray[4].Trim());
            DateTime Today = DateTime.Now;

            // 15 gün geçmişse
            if (Today.Subtract(takenDate).TotalDays > 15)
            {
                index2++;
                Console.WriteLine($"[{index2}] Kitap Adı: " + takenBookLinesArray[0].Trim());
            }
        }

        Console.WriteLine("\n[0] Geri");  
        bool exits = false;


        while (!exits)
        {
            int choose = Convert.ToInt32(Console.ReadLine());
            if (choose == 0) {
                exits = true;
            }
        }

    }

}
