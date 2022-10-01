using System;

namespace DotNetDesignPatternDemos.Concurrency.Lock.ImmutableObject
{
    /*
     * In questo esempio il primo modello concorrente per avere un accesso condiviso
     * tra più thread viene sviluppato con un tipo di oggetto che è di tipo immutabile.
     * 
     * Una volta creato un oggetto immutabile, il suo stato (inclusi i dati) non può essere 
     * modificato. Quindi possiamo tranquillamente condividerlo tra i thread.
     * Una classe immutabile ben nota in Java e c# è il tipo string. 
     * Una volta che proviamo a modificare un oggetto stringa attraverso metodi 
     * come replace, verrà generata e restituita una nuova copia mentre l'oggetto 
     * originale rimane invariato.
     * In genere, possiamo definire una classe immutabile dichiarando tutti i suoi campi 
     * come privati, oltre a disabilitare tutti i metodi setter.
     * La classe collections fornisce alcuni metodi utili per creare oggetti immutabili, 
     * ad esempio il metodo unmodifiableList restituisce una visualizzazione immutabile 
     * di un determinato elenco. In realtà, la classe collections genera un wrapper 
     * dell'elenco specificato, con tutti i metodi setter disabilitati. 
     * Se si tenta di chiamare un metodo setter nella visualizzazione immutabile restituita, 
     * verrà generata un'eccezione InvalidOperationException.
     * 
     * Il frammento di codice seguente proviene dal codice sorgente della classe collections. 
     * La classe UnmodifiableList è un wrapper di un elenco. 
     * Tutte le operazioni di modifica, incluso il metodo di ordinamento, sono disabilitate 
     * per assicurarsi che questo elenco sia immutabile.
     * 
     */

    /* Java code
     * Immutable Object
     
        static class UnmodifiableList <E> extends UnmodifiableCollection <E>
          implements List <E> {
            private static final long serialVersionUID = -283967356065247728 L;

            final List < ? extends E > list;

            UnmodifiableList(List < ? extends E > list) {
              super(list);
              this.list = list;
            }

            public boolean equals(Object o) {
              return o == this || list.equals(o);
            }
            public int hashCode() {
              return list.hashCode();
            }

            public E get(int index) {
              return list.get(index);
            }
            public E set(int index, E element) {
              throw new UnsupportedOperationException();
            }
            public void add(int index, E element) {
              throw new UnsupportedOperationException();
            }
            public E remove(int index) {
              throw new UnsupportedOperationException();
            }
            public int indexOf(Object o) {
              return list.indexOf(o);
            }
            public int lastIndexOf(Object o) {
              return list.lastIndexOf(o);
            }
            public boolean addAll(int index, Collection < ? extends E > c) {
              throw new UnsupportedOperationException();
            }

            @Override
            public void replaceAll(UnaryOperator < E > operator) {
              throw new UnsupportedOperationException();
            }
            @Override
            public void sort(Comparator < ? super E > c) {
              throw new UnsupportedOperationException();
            }
          }
    */


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
