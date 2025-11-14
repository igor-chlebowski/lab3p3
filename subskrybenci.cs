using System;
using System.Collections.Generic;
using System.Linq;

// Klasa PolkaSklepowa jest teraz Subskrybentem
public class PolkaSklepowa
{
    public List<Produkt> Produkty { get; private set; } = new List<Produkt>();
    public float LacznaWartoscPolki { get; private set; } = 0;

    public void DodajProdukt(Produkt produkt)
    {
        if (produkt == null) return;
        
        Produkty.Add(produkt);

        // Krok 4: Subskrypcja!
        // Gdy dodajemy produkt na półkę, zaczynamy "słuchać"
        // czy jego cena się nie zmieni.
        produkt.CenaZmieniona += OnCenaProduktuZmieniona;

        // Aktualizujemy łączną wartość
        PrzeliczWartoscPolki();
    }

    // Krok 5: Metoda obsługi zdarzenia (Event Handler)
    // Ta metoda zostanie automatycznie wywołana przez event produktu.
    private void OnCenaProduktuZmieniona(object sender, CenaZmienionaEventArgs e)
    {
        Console.WriteLine($"[PÓŁKA SKLEPOWA]: Zauważyłam zmianę ceny produktu {e.ZmienionyProdukt.Nazwa}!");
        
        // Zamiast przeliczać wszystko od nowa, możemy być sprytni:
        LacznaWartoscPolki -= e.StaraCena;
        LacznaWartoscPolki += e.NowaCena;
        
        Console.WriteLine($"[PÓŁKA SKLEPOWA]: Nowa łączna wartość półki: {LacznaWartoscPolki:C}");
    }

    // Metoda pomocnicza
    private void PrzeliczWartoscPolki()
    {
        LacznaWartoscPolki = Produkty.Sum(p => p.Cena);
    }
    
    // ... można też dodać metodę UsuńProdukt, która odpisuje event (+=) ...
}

// Inny, zupełnie niezależny subskrybent
public class KasaSklepowa
{
    // Metoda obsługi, która pasuje do eventu
    public void ZaktualizujCeneWSysCentralnym(object sender, CenaZmienionaEventArgs e)
    {
        Console.WriteLine($"  [KASA SKLEPOWA]: Wysyłam aktualizację ceny ({e.NowaCena:C}) dla '{e.ZmienionyProdukt.Nazwa}' do centrali...");
        // ... logika wysyłania do bazy danych ...
    }
}

// Trzeci, niezależny subskrybent
public class SystemPromocji
{
    public void SprawdzNowePromocje(object sender, CenaZmienionaEventArgs e)
    {
        if (e.NowaCena < 5.0f && e.StaraCena >= 5.0f)
        {
            Console.WriteLine($"    [SYSTEM PROMOCJI]: Produkt '{e.ZmienionyProdukt.Nazwa}' właśnie wszedł do promocji 'Poniżej 5 zł!'");
        }
    }
}
