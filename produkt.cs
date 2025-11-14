using System;

// Krok 1: Definiujemy klasę przechowującą dane o zdarzeniu
public class CenaZmienionaEventArgs : EventArgs
{
    public float StaraCena { get; }
    public float NowaCena { get; }
    public Produkt ZmienionyProdukt { get; }

    public CenaZmienionaEventArgs(Produkt produkt, float staraCena, float nowaCena)
    {
        ZmienionyProdukt = produkt;
        StaraCena = staraCena;
        NowaCena = nowaCena;
    }
}

public class Produkt
{
    // Krok 2: Definiujemy event w klasie Produktu
    // Używamy standardowego delegatu EventHandler<T>
    public event EventHandler<CenaZmienionaEventArgs> CenaZmieniona;

    // Prywatne pole
    private float _cena;

    // Właściwości
    public string Nazwa { get; set; }
    public float Waga { get; set; }
    public string Producent { get; set; }
    public DateTime TerminWaznosci { get; set; }

    public float Cena
    {
        get => _cena;
        set
        {
            if (_cena == value) return; // Nic się nie zmieniło

            float staraCena = _cena;
            _cena = value; // Zmiana stanu

            // Krok 3: Wywołanie (podniesienie) eventu
            // Wysyłamy powiadomienie do wszystkich subskrybentów
            OnCenaZmieniona(new CenaZmienionaEventArgs(this, staraCena, _cena));
        }
    }
    
    // To jest nasza "zwykła metoda", o którą pytałeś.
    // Teraz jest "opakowana" we właściwość, ale robi to samo.
    public void ZmienCene(float nowaCena)
    {
        // Używamy właściwości 'Cena', aby automatycznie wywołać event
        this.Cena = nowaCena;
    }

    // Metoda pomocnicza do wywoływania eventu
    protected virtual void OnCenaZmieniona(CenaZmienionaEventArgs e)
    {
        // Sprawdzamy, czy ktoś subskrybuje (czy handler != null)
        // i wywołujemy event, przekazując 'this' (kto wywołał) i dane 'e'
        CenaZmieniona?.Invoke(this, e);
    }

    // Konstruktor dla prostoty
    public Produkt(string nazwa, float cena)
    {
        Nazwa = nazwa;
        _cena = cena;
    }
}
